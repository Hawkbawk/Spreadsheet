using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Formulas;
using SS;
using SSGui;

namespace SpreadsheetGUI
{
    public class SpreadsheetController
    {
        private ISpreadsheetView window;

        private Spreadsheet spreadsheet;

        public SpreadsheetController(ISpreadsheetView window)
        {
            this.window = window;

            this.spreadsheet = new Spreadsheet();

            window.CloseEvent += HandleCloseEvent;
            window.OpenEvent += HandleOpenEvent;
            window.SaveEvent += HandleSaveEvent;
            window.NewEvent += HandleNewEvent;
            window.ChangeContents += HandleChangeContents;
            window.GetSpreadsheetPanel().SelectionChanged += HandleChangedSelection;
        }

        private void HandleChangedSelection(SpreadsheetPanel sender)
        {
            window.GetSelection(out int row, out int col);
            string cellName = toCellName(row, col);
            object cellContents = spreadsheet.GetCellContents(cellName);
            // Check to see if we need to prepend the string with an equals sign cause its a formula.
            if (cellContents is Formula)
            {
                window.ChangeTextbox("=" + cellContents.ToString());
                return;
            }
            // Otherwise just change the text box to the current contents
            window.ChangeTextbox(cellContents.ToString());
        }

        private void HandleChangeContents()
        {
            // Obtain the name of the currently selected cell and its desired contents.
            window.GetSelection(out int row, out int col);
            string cellName = toCellName(row, col);
            string contents = window.GetDesiredContents();
            ISet<string> dependents = new HashSet<string>();
            // Try and change the contents of the spreadsheet to the new contents.
            try
            {
                dependents = spreadsheet.SetContentsOfCell(cellName, contents);
            }
            catch (Exception e)
            {
                if (e is FormulaFormatException)
                {
                    MessageBox.Show("That's not a valid formula!");
                } 
                else if (e is CircularException)
                {
                    MessageBox.Show("That formula creates a circular dependency!");
                }
                return;
            }

            // Update all of the cells so they show the correct value
            foreach(string s in dependents)
            {
                string value = spreadsheet.GetCellValue(s).ToString();
                toCoordinates(s, out int currentRow, out int currentColumn);
                // TODO: Decide what should be displayed if the value is a formula error.
                window.SetValue(currentRow, currentColumn, value);
            }

            // Update the text box to show the correct contents.
            HandleChangedSelection(window.GetSpreadsheetPanel());
            
        }

        private void toCoordinates(string cellName, out int row, out int col)
        {
            string rowNumber = cellName.Substring(1);
            row = Convert.ToInt32(rowNumber) - 1;
            char column = cellName[0];
            col = Convert.ToInt32(column) - 'A';
            
        }

        private string toCellName(int row, int col)
        {
            return (char)('A' + row) + (col + 1).ToString();
        }

        private void HandleNewEvent()
        {
            window.OpenNew();
        }

        private void HandleSaveEvent(string obj)
        {
            throw new NotImplementedException();
        }

        private void HandleCloseEvent()
        {
            window.DoClose();
        }

        private void HandleOpenEvent(string obj)
        {
            throw new NotImplementedException();
        }
    }
}
