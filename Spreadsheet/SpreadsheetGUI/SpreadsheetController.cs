using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            window.NewCellSelected += HandleNewCellSelected;
        }

        private void HandleNewCellSelected()
        {
            window.GetSelection(out int row, out int col);
            string cellName = toCellName(row, col);
            object contents = spreadsheet.GetCellContents(cellName);
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
            foreach(string s in dependents)
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

        private void HandleSaveEvent(string obj)       //I think the input is filename???? 
        {
            string filename = obj;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = "Save a Spreadsheet file";
            saveFileDialog1.FileName = filename + ".ss";
            saveFileDialog1.Filter = "Spreadsheet Files (*.ss)|*.ss|All Files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(saveFileDialog1.FileName))
                {
                    spreadsheet.Save(sw);
                }
            }
        }

        private void HandleCloseEvent()
        {
            if (spreadsheet.Changed)
            {
                
                return;
            }
            window.DoClose();
        }



        private void HandleOpenEvent(string obj)
        {
            string filename = obj;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Open a Spreadsheet file";
            openFileDialog1.InitialDirectory = "Spreadsheet Files (*.ss)|*.ss|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;


            try
            {
                Regex r = new Regex("^[a-zA-Z][1-9][0-9]?$");
                TextReader t = File.OpenText(filename);
                spreadsheet = new Spreadsheet(t, r);
            }catch (Exception e)
            {
                MessageBox.Show("Unable to open file" + e);
            }
        }
    }
}
