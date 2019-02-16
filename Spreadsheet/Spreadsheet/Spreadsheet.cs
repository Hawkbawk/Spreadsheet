using Dependencies;
using Formulas;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
        private Dictionary<string, object> cells;

        /// <summary>
        /// A collection of all dependencies in the current spreadsheet.
        /// </summary>
        private DependencyGraph dependencies;

        /// <summary>
        /// Creates a new spreadsheet with no dependencies and an "infinite"
        /// number of cells. These infinite cells are all empty, simply because
        /// their contents are null. If someone gets an empty cell's contents, an
        /// empty string is returned, rather than null.
        /// </summary>
        public Spreadsheet()
        {
            cells = new Dictionary<string, object>();
            dependencies = new DependencyGraph();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override object GetCellContents(string name)
        {
            if (name == null || !IsValidCellName(name))
            {
                throw new InvalidNameException();
            }
            if (!cells.TryGetValue(name, out object contents))
            {
                return "";
            }
            return contents;
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
                if (!cell.Value.Equals(""))
                {
                    nonEmptyCells.Add(cell.Key);
                }
            }
            return nonEmptyCells;
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
        public override ISet<string> SetCellContents(string name, double number)
        {
            return SetCellContentsGeneric(name, number);
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
        public override ISet<string> SetCellContents(string name, string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException();
            }
            return SetCellContentsGeneric(name, text);
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
        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            if (formula == null)
            {
                throw new ArgumentNullException();
            }
            foreach (string s in formula.GetVariables())
            {
                dependencies.AddDependency(name, s);
            }

            return SetCellContentsGeneric(name, formula);
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
            if (!IsValidCellName(name))
            {
                throw new InvalidNameException();
            }
            return dependencies.GetDependents(name);
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
            return r.IsMatch(name);
        }

        /// <summary>
        /// A private, generic helper method that sets the cell contents for the provided "name" to "o".
        /// </summary>
        /// <param name="name">
        /// The name of the cell whose contents will be set.
        /// </param>
        /// <param name="o">
        /// The desired contents for the specified cell.
        /// </param>
        /// <returns>
        /// Returns a set of all cells that now depend on the passed in cell.
        /// </returns>
        private ISet<string> SetCellContentsGeneric(string name, object o)
        {
            if (name == null || !IsValidCellName(name))
            {
                throw new InvalidNameException();
            }
            cells[name] = o;
            HashSet<string> dependents = new HashSet<string>();
            foreach (string s in GetCellsToRecalculate(name))
            {
                dependents.Add(s);
            }
            return dependents;
        }
    }
}