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
            string cellContents = spreadsheet.GetCellContents(cellName).ToString();
            window.ChangeTextbox(cellContents);
        }

        private void HandleChangeContents()
        {
            window.GetSelection(out int row, out int col);
            string cellName = toCellName(row, col);
            string contents = window.GetDesiredContents();
            ISet<string> dependents = new HashSet<string>();
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

            foreach(string s in dependents)
            {
                string value = spreadsheet.GetCellValue(s).ToString();
                toCoordinates(s, out int currentRow, out int currentColumn);
                window.SetValue(currentRow, currentColumn, value);
            }
            
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
            throw new NotImplementedException();
        }

        private void HandleSaveEvent(string obj)
        {
            throw new NotImplementedException();
        }

        private void HandleCloseEvent()
        {
            throw new NotImplementedException();
        }

        private void HandleOpenEvent(string obj)
        {
            throw new NotImplementedException();
        }
    }
}
