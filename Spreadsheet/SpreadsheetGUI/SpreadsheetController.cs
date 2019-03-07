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
        /// <summary>
        /// The model part of Model, View, and Controller. Contains public methods that allow for the
        /// representation of spreadsheet data.
        /// </summary>
        private Spreadsheet spreadsheet;

        /// <summary>
        /// The view part of Model, View, and Controller. Contains public methods that allow the
        /// controller to interact with the user and vice versa. This is what the user actually sees.
        /// </summary>
        private ISpreadsheetView window;

        /// <summary>
        /// A public constructor that creates a new controller that is associated with the passed in
        /// window and has no data stored in the model.
        /// </summary>
        /// <param name="_window">A GUI that the controller can interact with</param>
        public SpreadsheetController(ISpreadsheetView _window)
        {
            window = _window;

            spreadsheet = new Spreadsheet();

            // Ensure that all of the events that the window has are handled by the controller
            window.CloseEvent += HandleCloseEvent;
            window.OpenEvent += HandleOpenEvent;
            window.SaveEvent += HandleSaveEvent;
            window.NewEvent += HandleNewEvent;
            window.ChangeContents += HandleChangeContents;
            window.NewCellSelected += HandleNewCellSelected;
        }

        /// <summary>
        /// Creates a new controller that is based off of the passed in window and contains all of
        /// the data stored in the passed in file. Error checking will have been done by a previous
        /// controller, so the passed in file will always be readable.
        /// </summary>
        /// <param name="_window">The GUI that the controller can interact with</param>
        /// <param name="filename">The name of the file containing the data to be read</param>
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
                toCoordinates(cell, out col, out row);
                window.SetValue(col, row, spreadsheet.GetCellValue(cell).ToString());
            }
            HandleNewCellSelected();
        }

        /// <summary>
        /// A private Event Handler that is called whenever the ChangeContents event is fired.
        /// </summary>
        private void HandleChangeContents()
        {
            // Obtain the name of the currently selected cell and its desired contents.
            window.GetSelection(out int col, out int row);
            string cellName = toCellName(col, row);
            string contents = window.GetDesiredContents();
            ISet<string> dependents = new HashSet<string>();
            // Try and change the contents of the spreadsheet to the new contents.
            try
            {
                dependents = spreadsheet.SetContentsOfCell(cellName, contents);
            }
            catch (Exception e)
            {
                // Show the appropriate error message if the passed in contents aren't allowed.
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
                toCoordinates(s, out int currentCol, out int currentRow);
                if (value is FormulaError)
                {
                    value = "#ERROR";
                }
                window.SetValue(currentCol, currentRow, value.ToString());
            }

            // Update the text box to show the correct contents.
            HandleNewCellSelected();
        }

        /// <summary>
        /// A private Event Handler that is called whenever the CloseEvent is fired.
        /// </summary>
        private void HandleCloseEvent()
        {
            // If the spreadsheet has been changed and hasn't been saved, begin the process the for
            // closing without saving.
            if (spreadsheet.Changed)
            {
                window.BeginCloseWithoutSave();
                return;
            }
            // Otherwise, just close the current window.
            window.DoClose();
        }

        /// <summary>
        /// A private Event Handler that is called whenever the NewCellSelected event is fired.
        /// Ensures that the window receives all of the necessary information for a new cell being selected.
        /// </summary>
        private void HandleNewCellSelected()
        {
            window.GetSelection(out int col, out int row);
            string cellName = toCellName(col, row);
            object contents = spreadsheet.GetCellContents(cellName);
            if (contents is Formula)
            {
                window.HandleSelectedNewCell("=" + contents.ToString());
                return;
            }
            window.HandleSelectedNewCell(contents.ToString());
        }

        /// <summary>
        /// A private Event Handler that is called whenever the NewEvent is fired. Opens a new
        /// window with its own new controller.
        /// </summary>
        private void HandleNewEvent()
        {
            window.OpenNew();
        }

        /// <summary>
        /// A private Event handler that is called whenever the OpenEvent is fired. Opens a new
        /// window with its own new controller that contains all of the data stored in the passed in file.
        /// </summary>
        /// <param name="filename">The name of the file to be opened</param>
        private void HandleOpenEvent(string filename)
        {
            window.OpenNew(filename);
        }

        /// <summary>
        /// A private Event Handler that is called whenever the SaveEvent is fired. Calls the models
        /// save method with the passed in desired file name.
        /// </summary>
        /// <param name="filename">The desired name for the model's contents to be saved under</param>
        private void HandleSaveEvent(string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                spreadsheet.Save(sw);
            }
        }

        /// <summary>
        /// A private helper method that converts the passed in row and column numbers to their
        /// appropriate cell names. For instance, if the passed in row is 0 and the passed in column
        /// is 1, the resulting cell name will be "B1".
        /// </summary>
        /// <param name="row">The row of the cell</param>
        /// <param name="col">The column of the cell</param>
        /// <returns>The name of the cell in string format</returns>
        private string toCellName(int col, int row)
        {
            return (char)('A' + col) + (row + 1).ToString();
        }

        /// <summary>
        /// A private helper method that converts a cell name such as "B1" or "C44" into a row and
        /// column. The rows and columns use zero based indexing.
        /// </summary>
        /// <param name="cellName">The name of the cell to be converted</param>
        /// <param name="row">
        /// An out parameter that represents the row associated with the passed in cell name
        /// </param>
        /// <param name="col">
        /// An out parameter that represents the column associated with the passed in cell name
        /// </param>
        private void toCoordinates(string cellName, out int col, out int row)
        {
            string rowNumber = cellName.Substring(1);
            row = Convert.ToInt32(rowNumber) - 1;
            char column = cellName[0];
            col = Convert.ToInt32(column) - 'A';
        }
    }
}