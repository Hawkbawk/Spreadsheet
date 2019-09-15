using Dependencies;
using Formulas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;

namespace SS
{
    /// <summary>
    /// An inheritor of the AbstractSpreadsheet class. Has similar functions as
    /// (an extremely basic) Excel spreadsheet. Currently doesn't support
    /// actually obtaining values for a formula, but does allows for pulling out
    /// the contents of any given valid cell.
    /// Hello world!
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        /// <summary>
        /// A collection of all the cells in the spreadsheet.
        /// </summary>
        private Dictionary<string, Cell> cells;

        /// <summary>
        /// A collection of all dependencies in the current spreadsheet.
        /// </summary>
        private DependencyGraph dependencies;

        /// <summary>
        /// A regex that provides additional constraints for what constitutes a valid variable.
        /// </summary>
        private Regex IsValid { get; set; }

        /// <summary>
        /// Creates an empty Spreadsheet whose IsValid regular expression accepts every string.
        /// </summary>
        public Spreadsheet()
        {
            Changed = false;
            // IsValid is set to match any string.
            IsValid = new Regex(@"^.*$");
            cells = new Dictionary<string, Cell>();
            dependencies = new DependencyGraph();
        }

        /// <summary>
        /// Creates an empty Spreadsheet whose IsValid regular expression is provided as the parameter.
        /// </summary>
        /// <param name="isValid">A regex for validating cell names</param>
        public Spreadsheet(Regex isValid)
        {
            Changed = false;
            IsValid = isValid;
            cells = new Dictionary<string, Cell>();
            dependencies = new DependencyGraph();
        }

        /// <summary>
        /// Creates a Spreadsheet that is a duplicate of the spreadsheet saved in source.
        ///
        /// See the AbstractSpreadsheet.Save method and Spreadsheet.xsd for the file format specification.
        ///
        /// If there's a problem reading source, throws an IOException.
        ///
        /// Else if the contents of source are not consistent with the schema in Spreadsheet.xsd,
        /// throws a SpreadsheetReadException.
        ///
        /// Else if the IsValid string contained in source is not a valid C# regular expression,
        /// throws a SpreadsheetReadException. (If the exception is not thrown, this regex is
        /// referred to below as oldIsValid.)
        ///
        /// Else if there is a duplicate cell name in the source, throws a SpreadsheetReadException.
        /// (Two cell names are duplicates if they are identical after being converted to upper case.)
        ///
        /// Else if there is an invalid cell name or an invalid formula in the source, throws a
        /// SpreadsheetReadException. (Use oldIsValid in place of IsValid in the definition of cell
        /// name validity.)
        ///
        /// Else if there is an invalid cell name or an invalid formula in the source, throws a
        /// SpreadsheetVersionException. (Use newIsValid in place of IsValid in the definition of
        /// cell name validity.)
        ///
        /// Else if there's a formula that causes a circular dependency, throws a SpreadsheetReadException.
        ///
        /// Else, create a Spreadsheet that is a duplicate of the one encoded in source except that
        /// the new Spreadsheet's IsValid regular expression should be newIsValid.
        /// </summary>
        /// <param name="source">The source XML file</param>
        /// <param name="newIsValid">A new regex for validating cell names</param>
        public Spreadsheet(TextReader source, Regex newIsValid) : this()
        {
            XmlSchemaSet sc = new XmlSchemaSet();
            sc.Add(null, "Spreadsheet.xsd");

            // Configure validation.
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas = sc;
            settings.ValidationEventHandler += ValidateXML;
            using (XmlReader reader = XmlReader.Create(source, settings))
            {
                Regex oldIsValid = new Regex(@"^.*$");
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "spreadsheet":
                                // Move to the IsValid attribute to obtain the oldIsValid regex.
                                reader.MoveToNextAttribute();
                                try
                                {
                                    oldIsValid = new Regex(reader.Value);
                                }
                                // Throw if the regex isn't a valid regex.
                                catch (ArgumentException)
                                {
                                    throw new SpreadsheetReadException("Your IsValid regex isn't a valid regex!");
                                }
                                break;

                            case "cell":
                                // Move to the contents attribute and store it.
                                reader.MoveToNextAttribute();
                                string cellName = reader.Value;
                                // Check to see if the cell name is valid according to both validators.
                                IsValid = oldIsValid;
                                if (!IsValidCellName(cellName))
                                {
                                    throw new SpreadsheetReadException("The cell names provided don't match the old regex!");
                                }
                                IsValid = newIsValid;
                                if (!IsValidCellName(cellName))
                                {
                                    throw new SpreadsheetVersionException("The cell names provided don't match the new regex!");
                                }
                                // Check for duplicates. Throw if there are any.
                                if (cells.ContainsKey(cellName.ToUpper()))
                                {
                                    throw new SpreadsheetReadException("There are duplicate cells in your spreadsheet!");
                                }
                                // Otherwise obtain the cell name's contents.
                                reader.MoveToNextAttribute();
                                string cellContents = reader.Value;
                                // Try and set the contents of the cell.
                                try
                                {
                                    SetContentsOfCell(cellName, cellContents);
                                }
                                // Throw if that creates a circular exception.
                                catch (CircularException)
                                {
                                    throw new SpreadsheetReadException("Your spreadsheet contains circular dependencies!");
                                }
                                break;
                        }
                    }
                }
            }
            // Adjust our IsValid to the newIsValid.
            IsValid = newIsValid;
            // The spreadsheet hasn't been changed since it's creation, so set Changed to false.
            Changed = false;
        }

        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed { get; protected set; }

        /// <summary>
        /// Returns the cell contents associated with the provided cell name.
        /// </summary>
        /// <param name="name">The name of the cell whose contents will be retrieved.</param>
        /// <returns>The contents of the provided cell.</returns>
        public override object GetCellContents(string name)
        {
            // Checks to see if name is non-null and valid.
            if (name == null || !IsValidCellName(name))
            {
                throw new InvalidNameException();
            }
            // Tries to grab the value out of the Dictionary. Returns an empty string if no value is
            // associated with the provided cell name.
            if (!cells.TryGetValue(name.ToUpper(), out Cell contents))
            {
                return "";
            }
            return contents.cellContents;
        }

        /// <summary>
        /// Returns the value associated with the provided cell.
        /// </summary>
        /// <param name="name">The name of the cell whose value will be retrieved</param>
        /// <returns>The value associated with the passed in cell</returns>
        public override object GetCellValue(string name)
        {
            if (name == null || !IsValidCellName(name))
            {
                throw new InvalidNameException();
            }
            if (!cells.TryGetValue(name.ToUpper(), out Cell cell))
            {
                return "";
            }
            return cell.cellValue;
        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        /// <returns>Returns all the names of all the non-empty cells in the spreadsheet.</returns>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            List<string> nonEmptyCells = new List<string>(cells.Count);
            foreach (var cell in cells)
            {
                if (!cell.Value.cellContents.Equals(""))
                {
                    nonEmptyCells.Add(cell.Key);
                }
            }
            return nonEmptyCells;
        }

        /// <summary>
        /// Writes the contents of this spreadsheet to dest using an XML format.
        /// The XML elements should be structured as follows:
        ///
        /// <spreadsheet IsValid="IsValid regex goes here">
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        /// </spreadsheet>
        ///
        /// The value of the IsValid attribute should be IsValid.ToString()
        ///
        /// There should be one cell element for each non-empty cell in the spreadsheet.
        /// If the cell contains a string, the string (without surrounding double quotes) should be written as the contents.
        /// If the cell contains a double d, d.ToString() should be written as the contents.
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        ///
        /// If there are any problems writing to dest, the method should throw an IOException.
        /// </summary>
        /// <param name="dest">The desired filename</param>
        public override void Save(TextWriter dest)
        {
            using (XmlWriter writer = XmlWriter.Create(dest))
            {
                // Start the document off with an element called spreadsheet that has an attribute
                // IsValid, which is our regex expression.
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("IsValid", IsValid.ToString());
                foreach (var cellName in GetNamesOfAllNonemptyCells())
                {
                    // Create a new element for each cell in the spreadsheet.
                    Cell currentCell = cells[cellName];
                    writer.WriteStartElement("cell");
                    writer.WriteAttributeString("name", cellName);
                    if (cells[cellName].cellContents is Formula)
                    {
                        // Formulas should be preceded by '=' so make sure and add that.
                        writer.WriteAttributeString("contents", "=" + currentCell.cellContents.ToString());
                    }
                    else
                    {
                        // Otherwise just write the contents with the ToString method.
                        writer.WriteAttributeString("contents", currentCell.cellContents.ToString());
                    }
                    // End the element to indicate the end of a cell.
                    writer.WriteEndElement();
                }
                // End our IsValid element and the document.
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            // The document has now been saved, so Changed should be set to false.
            Changed = false;
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, the contents of the named cell becomes number. The method returns a set
        /// consisting of name plus the names of all other cells whose value depends, directly or
        /// indirectly, on the named cell.
        ///
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the set {A1, B1, C1}
        /// is returned.
        /// </summary>
        /// <param name="name">The name of the cell whose contents will be set</param>
        /// <param name="number">The desired contents for the cell</param>
        /// <returns>Returns a set of all cells that now depend on the provided cell</returns>
        protected override ISet<string> SetCellContents(string name, double number)
        {
            // No edge cases need to be checked specifically for numbers, so just set the cell
            // contents accordingly.
            return SetCellContentsGeneric(name, new Cell(number, number));
        }

        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        ///
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, the contents of the named cell becomes text. The method returns a set
        /// consisting of name plus the names of all other cells whose value depends, directly or
        /// indirectly, on the named cell.
        ///
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the set {A1, B1, C1}
        /// is returned.
        /// </summary>
        /// <param name="name">The name of the cell whose contents will be set.</param>
        /// <param name="text">The desired contents for the cell.</param>
        /// <returns>Returns a set of all cells that now depend on the provided cell.</returns>
        protected override ISet<string> SetCellContents(string name, string text)
        {
            // Simply set the cell contents.
            return SetCellContentsGeneric(name, new Cell(text, text));
        }

        /// <summary>
        /// Requires that all of the variables in formula are valid cell names.
        ///
        /// If name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a
        /// circular dependency, throws a CircularException.
        ///
        /// Otherwise, the contents of the named cell becomes formula. The method returns a Set
        /// consisting of name plus the names of all other cells whose value depends, directly or
        /// indirectly, on the named cell.
        ///
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the set {A1, B1, C1}
        /// is returned.
        /// </summary>
        /// <param name="name">The name of the cell whose contents will be set.</param>
        /// <param name="formula">The desired contents for the cell.</param>
        /// <returns>Returns a set of all cells that now depend on the provided cell.</returns>
        protected override ISet<string> SetCellContents(string name, Formula formula)
        {
            // Add in all of our new dependencies.
            foreach (string s in formula.GetVariables())
            {
                dependencies.AddDependency(name, s);
            }
            // Try and evaluate the formula.
            object value;
            try
            {
                value = formula.Evaluate(EvalLookup);
            }
            // The value will simply be a FormulaError if the formula can't be evaluated.
            catch (FormulaEvaluationException e)
            {
                value = new FormulaError(e.Message);
            }
            // Set the cell contents.
            try
            {
                return SetCellContentsGeneric(name, new Cell(formula, value));

            }
            catch (CircularException e)
            {
                foreach(string s in formula.GetVariables())
                {
                    dependencies.RemoveDependency(name, s);
                }
                throw e;
            }
        }

        /// <summary>
        /// If content is null, throws an ArgumentNullException.
        ///
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, if content parses as a double, the contents of the named cell becomes that double.
        ///
        /// Otherwise, if content begins with the character '=', an attempt is made to parse the
        /// remainder of content into a Formula f using the Formula constructor with s.ToUpper() as
        /// the normalizer and a validator that checks that s is a valid cell name as defined in the
        /// AbstractSpreadsheet class comment. There are then three possibilities:
        ///
        /// (1) If the remainder of content cannot be parsed into a Formula, a
        /// Formulas.FormulaFormatException is thrown.
        ///
        /// (2) Otherwise, if changing the contents of the named cell to be f would cause a circular
        /// dependency, a CircularException is thrown.
        ///
        /// (3) Otherwise, the contents of the named cell becomes f.
        ///
        /// Otherwise, the contents of the named cell becomes content.
        ///
        /// If an exception is not thrown, the method returns a set consisting of name plus the names
        /// of all other cells whose value depends, directly or indirectly, on the named cell.
        ///
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the set {A1, B1, C1}
        /// is returned.
        /// </summary>
        /// <param name="name">The desired name for the cell</param>
        /// <param name="content">The desired contents for the cell</param>
        /// <returns>Returns a set of all cells that depend on the passed in cell</returns>
        public override ISet<string> SetContentsOfCell(string name, string content)
        {
            // Check to see if the contents are null.
            if (content == null)
            {
                throw new ArgumentNullException();
            }
            // Check to see if the name is null or invalid.
            if (name == null || !IsValidCellName(name))
            {
                throw new InvalidNameException();
            }
            // Check to see if the cell should simply not be touched, due to passing in an empty string.
            if (content.Length == 0)
            {
                return SetCellContents(name, content);
            }
            // Try and convert the cell to a double.
            try
            {
                return SetCellContents(name.ToUpper(), Convert.ToDouble(content));
            }
            catch (Exception) { }
            // Try and create a formula out of the contents.
            if (content[0] == '=')
            {
                string formula = content.Substring(1).ToUpper();
                return SetCellContents(name.ToUpper(), new Formula(formula, s => s.ToUpper(), IsValidCellName));
            }
            // Otherwise, just set the contents to simply be the passed in string.
            return SetCellContents(name, content);
        }

        /// <summary>
        /// If name is null, throws an ArgumentNullException.
        ///
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        ///
        /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell. In other words, returns an
        /// enumeration, without duplicates, of the names of all cells that contain formulas
        /// containing name.
        ///
        /// For example, suppose that A1 contains 3 B1 contains the formula A1 * A1 C1 contains the
        /// formula B1 + A1 D1 contains the formula B1 - C1 The direct dependents of A1 are B1 and C1
        /// </summary>
        /// <param name="name">The name of the cell whose dependents will be retrieved.</param>
        /// <returns>
        /// Enumerates a collection of all cells that directly depend on the provided cell.
        /// </returns>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            return dependencies.GetDependees(name);
        }

        /// <summary> Checks to see if the passed in string is a valid cell name. Valid cell names
        /// consist of one or more letters, followed by a digit 1-9, and zero or more digits 0-9.
        /// </summary> <param name="name"> The possible cell name </param> <param
        /// validChecker="validChecker"> Another regex for limiting possible cell names. </param> A
        /// regex expression that determines additional constraints for cell names. </param>
        /// <returns> True if "name" is a valid cell name. False otherwise. </returns>
        private bool IsValidCellName(String name)
        {
            // A pattern that matches only valid strings.
            string pattern = "^[a-zA-Z][1-9]([0-9]*)?$";
            Regex r = new Regex(pattern);
            // Check to see if the cell name is valid to our base rules and to the other rules
            // applied by the additional rules Regex.
            return r.IsMatch(name) && IsValid.IsMatch(name.ToUpper());
        }

        /// <summary>
        /// A private, generic helper method that sets the cell contents for the provided "name" to "o".
        /// </summary>
        /// <param name="name">The name of the cell whose contents will be set.</param>
        /// <param name="o">The desired contents for the specified cell.</param>
        /// <returns>Returns a set of all cells that now depend on the passed in cell.</returns>
        private ISet<string> SetCellContentsGeneric(string name, Cell o)
        {
            Changed = true;
            HashSet<string> dependents = new HashSet<string>();
            List<string> tempDependents = new List<string>();
            // Obtain all of the cells that need to be recalculated and check for any circular dependencies.
            foreach (string s in GetCellsToRecalculate(name))
            {
                dependents.Add(s);
                tempDependents.Add(s);
            }
            // Store the new cell contents in the Dictionary.
            cells[name.ToUpper()] = o;
            foreach (string s in tempDependents)
            {
                if (!cells.TryGetValue(s, out Cell c))
                {
                    continue;
                }
                if (c.cellContents is Formula)
                {
                    Formula f = c.cellContents as Formula;
                    try
                    {
                        c.cellValue = f.Evaluate(EvalLookup);
                    }
                    catch (Exception)
                    {
                        c.cellValue = new FormulaError();
                    }
                    cells[s] = c;
                }
            }
            // Return a set of all cells that depend on the changed cell.
            return dependents;
        }

        /// <summary>
        /// A private method to be used when evaluating formulas inside the spreadsheet. If the value
        /// of a variable it looks up is another formula, it then evaluates that formula using itself
        /// as the lookup method. Otherwise, if the value of the variable is a double, returns that
        /// double. Otherwise, if the value is a string, throws an UndefinedVariableException.
        /// </summary>
        /// <param name="variable">The name of the variable whose contents will be looked up.</param>
        /// <returns>Returns a double associated with that variable</returns>
        private double EvalLookup(string variable)
        {
            // Obtain the cell contents.
            object value = GetCellValue(variable);
            // Check and see what the contents are and act accordingly.
            if (value is double)
            {
                // Simply return if we've found a double.
                return (double)value;
            }
            else
            {
                throw new UndefinedVariableException("The variable doesn't have a valid definition!");
            }
        }

        private static void ValidateXML(object sender, ValidationEventArgs e)
        {
            throw new SpreadsheetReadException("Your XML file doesn't match the specifications!");
        }

        /// <summary>
        /// A struct that represents a cell, containing both it's contents and it's value.
        /// </summary>
        internal struct Cell
        {
            /// <summary>
            /// Represents the contents of a cell. Can be either a formula, a double, or a string.
            /// </summary>
            internal object cellContents { get; set; }

            /// <summary>
            /// Represents the value of a cell. Can either be a double, a string, or a FormulaError.
            /// </summary>
            internal object cellValue { get; set; }

            /// <summary>
            /// Constructs a new Cell with the passed in parameters.
            /// </summary>
            /// <param name="_cellContents">The desired contents for the cell.</param>
            /// <param name="_cellValue">The desired value for the cell.</param>
            internal Cell(object _cellContents, object _cellValue)
            {
                cellContents = _cellContents;
                cellValue = _cellValue;
            }
        }
    }
}