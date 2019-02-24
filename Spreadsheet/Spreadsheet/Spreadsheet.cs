using Dependencies;
using Formulas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace SS
{
    /// <summary>
    /// An inheritor of the AbstractSpreadsheet class. Has similar functions as
    /// (an extremely basic) Excel spreadsheet. Currently doesn't support
    /// actually obtaining values for a formula, but does allows for pulling out
    /// the contents of any given valid cell.
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

        public Regex IsValid { get; private set; }

        /// <summary>
        /// Creates an empty Spreadsheet whose IsValid regular expression accepts every string.
        /// </summary>
        public Spreadsheet()
        {
            Changed = false;
            // IsValid is set to match any string.
            IsValid = new Regex(@".*");
            cells = new Dictionary<string, Cell>();
            dependencies = new DependencyGraph();
        }

        /// <summary>
        /// Creates an empty Spreadsheet whose IsValid regular expression is provided as the parameter.
        /// </summary>
        /// <param name="isValid"></param>
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
        /// See the AbstractSpreadsheet.Save method and Spreadsheet.xsd for the file format 
        /// specification.  
        ///
        /// If there's a problem reading source, throws an IOException.
        ///
        /// Else if the contents of source are not consistent with the schema in Spreadsheet.xsd, 
        /// throws a SpreadsheetReadException.  
        ///
        /// Else if the IsValid string contained in source is not a valid C# regular expression, throws
        /// a SpreadsheetReadException.  (If the exception is not thrown, this regex is referred to
        /// below as oldIsValid.)
        ///
        /// Else if there is a duplicate cell name in the source, throws a SpreadsheetReadException.
        /// (Two cell names are duplicates if they are identical after being converted to upper case.)
        ///
        /// Else if there is an invalid cell name or an invalid formula in the source, throws a 
        /// SpreadsheetReadException.  (Use oldIsValid in place of IsValid in the definition of 
        /// cell name validity.)
        ///
        /// Else if there is an invalid cell name or an invalid formula in the source, throws a
        /// SpreadsheetVersionException.  (Use newIsValid in place of IsValid in the definition of
        /// cell name validity.)
        ///
        /// Else if there's a formula that causes a circular dependency, throws a SpreadsheetReadException. 
        ///
        /// Else, create a Spreadsheet that is a duplicate of the one encoded in source except that
        /// the new Spreadsheet's IsValid regular expression should be newIsValid.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="newIsValid"></param>
        public Spreadsheet(TextReader source, Regex newIsValid)
        {

        }

        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed { get; protected set; }

        /// <summary>
        /// Returns the cell contents associated with the provided cell name.
        /// </summary>
        /// <param name="name">
        /// The name of the cell whose contents will be retrieved.
        /// </param>
        /// <returns>
        /// The contents of the provided cell.
        /// </returns>
        public override object GetCellContents(string name)
        {
            if (name == null || !IsValidCellName(name))
            {
                throw new InvalidNameException();
            }

            if (!cells.TryGetValue(name.ToUpper(), out Cell contents))
            {
                return "";
            }
            return contents.cellContents;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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
        /// <returns>
        /// Returns all the names of all the non-empty cells in the spreadsheet.
        /// </returns>
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
        /// <param name="dest"></param>
        public override void Save(TextWriter dest)
        {
            using (XmlWriter writer = XmlWriter.Create(dest))
            {

            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        ///
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        /// <param name="name">
        /// The name of the cell whose contents will be set.
        /// </param>
        /// <param name="number">
        /// The desired contents for the cell.
        /// </param>
        /// <returns>
        /// Returns a set of all cells that now depend on the provided cell.
        /// </returns>
        protected override ISet<string> SetCellContents(string name, double number)
        {
            return SetCellContentsGeneric(name, new Cell(number, number));
        }

        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        ///
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        ///
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        /// <param name="name">
        /// The name of the cell whose contents will be set.
        /// </param>
        /// <param name="text">
        /// The desired contents for the cell.
        /// </param>
        /// <returns>
        /// Returns a set of all cells that now depend on the provided cell.
        /// </returns>
        protected override ISet<string> SetCellContents(string name, string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException();
            }
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
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// Set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        ///
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        /// <param name="name">
        /// The name of the cell whose contents will be set.
        /// </param>
        /// <param name="formula">
        /// The desired contents for the cell.
        /// </param>
        /// <returns>
        /// Returns a set of all cells that now depend on the provided cell.
        /// </returns>
        protected override ISet<string> SetCellContents(string name, Formula formula)
        {
            if (formula == null)
            {
                throw new ArgumentNullException();
            }
            foreach (string s in formula.GetVariables())
            {
                dependencies.AddDependency(name, s);
            }
            object value;
            try
            {
                value = formula.Evaluate(EvalLookup);
            }
            catch (FormulaEvaluationException e)
            {
                value = new FormulaError(e.Message);
            }
            return SetCellContentsGeneric(name, new Cell(formula, value));
        }

        /// <summary>
        /// If content is null, throws an ArgumentNullException.
        ///
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, if content parses as a double, the contents of the named
        /// cell becomes that double.
        ///
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor with s => s.ToUpper() as the normalizer and a validator that
        /// checks that s is a valid cell name as defined in the AbstractSpreadsheet
        /// class comment.  There are then three possibilities:
        ///
        ///   (1) If the remainder of content cannot be parsed into a Formula, a
        ///       Formulas.FormulaFormatException is thrown.
        ///
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown.
        ///
        ///   (3) Otherwise, the contents of the named cell becomes f.
        ///
        /// Otherwise, the contents of the named cell becomes content.
        ///
        /// If an exception is not thrown, the method returns a set consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell.
        ///
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public override ISet<string> SetContentsOfCell(string name, string content)
        {
            if (content == null)
            {
                throw new ArgumentNullException();
            }
            if (name == null || !IsValidCellName(name))
            {
                throw new InvalidNameException();
            }
            if (content.Length == 0)
            {
                return SetCellContents(name.ToUpper(), content);
            }
            try
            {
                return SetCellContents(name.ToUpper(), Convert.ToDouble(content));
            }
            catch (Exception e) { }
            if (content[0] != '=')
            {
               return SetCellContents(name, content);
            }
            string formula = content.Substring(1).ToUpper();
            return SetCellContents(name.ToUpper(), new Formula(formula));
            

            throw new NotImplementedException();
        }

        /// <summary>
        /// If name is null, throws an ArgumentNullException.
        ///
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        ///
        /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        ///
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        /// <param name="name">
        /// The name of the cell whose dependents will be retrieved.
        /// </param>
        /// <returns>
        /// Enumerates a collection of all cells that directly depend on the
        /// provided cell.
        /// </returns>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            return dependencies.GetDependees(name);
        }

        /// <summary>
        /// Checks to see if the passed in string is a valid cell name. Valid
        /// cell names consist of one or more letters, followed by a digit 1-9,
        /// and zero or more digits 0-9.
        /// </summary>
        /// <param name="name">
        /// The possible cell name
        /// </param>
        /// <returns>
        /// True if "name" is a valid cell name. False otherwise.
        /// </returns>
        private bool IsValidCellName(String name)
        {
            string pattern = "^[a-zA-Z]+[1-9][0-9]*$";
            Regex r = new Regex(pattern);
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
            if (name == null || !IsValidCellName(name))
            {
                throw new InvalidNameException();
            }
            Changed = true;
            HashSet<string> dependents = new HashSet<string>();
            foreach (string s in GetCellsToRecalculate(name))
            {
                dependents.Add(s);
            }
            cells[name] = o;
            RecalculateCells(dependents);
            return dependents;
        }

        private void RecalculateCells(ISet<string> recalculateCells)
        {
            foreach(string cellName in recalculateCells)
            {
                if (cells[cellName].cellContents is Formula)
                {
                    Formula f = cells[cellName].cellContents as Formula;
                    cells[cellName] = new Cell(cellName, f.Evaluate(EvalLookup));
                }
            }
        }

        /// <summary>
        /// A private method to be used when evaluating formulas inside the spreadsheet. If the value
        /// of a variable it looks up is another formula, it then evaluates that formula using itself
        /// as the lookup method. Otherwise, if the value of the variable is a double, returns that
        /// double. Otherwise, if the value is a string, throws an UndefinedVariableException.
        /// </summary>
        /// <param name="variable">
        /// The name of the variable whose contents will be looked up.
        /// </param>
        /// <returns>
        /// Returns a double associated with that variable
        /// </returns>
        private double EvalLookup(string variable)
        {
            object contents = GetCellContents(variable);
            if (contents is double)
            {
                return (double)contents;
            }
            else if (contents is string)
            {
                throw new UndefinedVariableException("Strings aren't allowed in formulas");
            }
            else
            {
                Formula f = contents as Formula;
                return f.Evaluate(EvalLookup);
            }
        }


        internal struct Cell
        {
            internal object cellContents { get; set; }
            internal object cellValue { get; set; }

            internal Cell(object _cellContents, object _cellValue)
            {
                cellContents = _cellContents;
                cellValue = _cellValue;
            }
        }
    }
}