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

        public Spreadsheet()
        {
            cells = new Dictionary<string, object>();
            dependencies = new DependencyGraph();
        }

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

        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            List<string> nonEmptyCells = new List<string>(cells.Count);
            foreach (var cell in cells)
            {
                nonEmptyCells.Add(cell.Key);
            }
            return nonEmptyCells;
        }

        public override ISet<string> SetCellContents(string name, double number)
        {
            return SetCellContentsGeneric(name, number);
        }

        public override ISet<string> SetCellContents(string name, string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException();
            }
            return SetCellContentsGeneric(name, text);
        }

        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            if (formula == null)
            {
                throw new ArgumentNullException();
            }
            /* TODO
             * Make it so that the dependencies are actually being changed
             * with the formula.
             * Also, make it so that circular dependencies aren't allowed,
             * probably by using the Visit method inside of the
             * AbstractSpreadsheet class.
             */
            return SetCellContentsGeneric(name, formula);
        }

        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            return dependencies.GetDependents(name);
        }

        /// <summary>
        /// Checks to see if the passed in string is a valid cell name. Valid
        /// cell names consist of one or more letters, followed by a digit 1-9,
        /// and zero or more digits 0-9.
        /// </summary>
        /// <param name="name"> The possible cell name </param>
        /// <returns>
        /// True if "name" is a valid cell name. False otherwise.
        /// </returns>
        private bool IsValidCellName(String name)
        {
            string pattern = "^[a-zA-Z]+[1-9][0-9]*$";
            Regex r = new Regex(pattern);
            return r.IsMatch(name);
        }

        private ISet<string> SetCellContentsGeneric(string name, object o)
        {
            if (!IsValidCellName(name))
            {
                throw new InvalidNameException();
            }
            cells[name] = o;
            HashSet<string> dependents = (HashSet<string>)GetCellsToRecalculate(name);
            return dependents;
        }
    }
}