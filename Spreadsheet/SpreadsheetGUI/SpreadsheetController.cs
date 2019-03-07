using Formulas;
using SS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    public class SpreadsheetController
    {
        private ISpreadsheetView window;

        private Spreadsheet spreadsheet;

        public SpreadsheetController(ISpreadsheetView _window)
        {
            window = _window;

            spreadsheet = new Spreadsheet();

            window.CloseEvent += HandleCloseEvent;
            window.OpenEvent += HandleOpenEvent;
            window.SaveEvent += HandleSaveEvent;
            window.NewEvent += HandleNewEvent;
            window.ChangeContents += HandleChangeContents;
            window.NewCellSelected += HandleNewCellSelected;
        }

        public SpreadsheetController(ISpreadsheetView _window, string filename) : this(_window)
        {
            int col, row;
            Regex r = new Regex(@"^[a-zA-Z][1-9][0-9]?$");
            using (StreamReader sw = new StreamReader(filename))
            {
                spreadsheet = new Spreadsheet(sw, r);
            }
            foreach (string cell in spreadsheet.GetNamesOfAllNonemptyCells())
            {
                toCoordinates(cell, out row, out col);
                window.SetValue(row, col, spreadsheet.GetCellValue(cell).ToString());
            }
            HandleNewCellSelected();
        }

        private void HandleNewCellSelected()
        {
            window.GetSelection(out int row, out int col);
            string cellName = toCellName(row, col);
            object contents = spreadsheet.GetCellContents(cellName);
            if (contents is Formula)
            {
                window.SelectedNewCell("=" + contents.ToString());
                return;
            }
            window.SelectedNewCell(contents.ToString());
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
            foreach (string s in dependents)
            {
                object value = spreadsheet.GetCellValue(s);
                toCoordinates(s, out int currentRow, out int currentColumn);
                if (value is FormulaError)
                {
                    value = "#ERROR";
                }
                window.SetValue(currentRow, currentColumn, value.ToString());
            }

            // Update the text box to show the correct contents.
            HandleNewCellSelected();

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

        private void HandleSaveEvent(string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                spreadsheet.Save(sw);
            }

        }

        private void HandleCloseEvent()
        {
            if (spreadsheet.Changed)
            {

            }
            window.DoClose();
        }



        private void HandleOpenEvent(string filename)
        {
            window.OpenNew(filename);
        }
    }
}
