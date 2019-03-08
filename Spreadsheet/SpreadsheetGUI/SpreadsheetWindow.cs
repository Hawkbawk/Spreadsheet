using SSGui;
using System;
using System.IO;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    public partial class spreadsheetWindow : Form, ISpreadsheetView
    {
        private string helpMessage = "Hey there! So you want to know how to " +
            "use this spreadsheet program? Well, it's fairly" +
            " similar to Excel, with a few differences. \n\nFirst, if you want " +
            "to change the contents of a cell, simply click on " +
            "the cell and start typing, then press enter when you're done. " +
            "\nYou can use formulas, just like Excel, with an equals sign " +
            "indicating a formula. Your formulas can contain links to the " +
            "values in other cells, by using a format such as \"= A4 + A5\". " +
            "You can also just put in any numbers you please, or text. " +
            "\n\nIf you want to open a new spreadsheet window, just hit File and then " +
            "New and a new window will open up. \nIf you want to save a spreadsheet," +
            " just hit File and then Save. A File Explorer box will then" +
            " open up and you can then decide where to save your file. \nIf you " +
            "want to open up a previously saved spreadsheet, simply hit File" +
            " and then Open. A File Explorer box will then open up and then you can " +
            "navigate to the desired file. \nIf you want to close your current spreadsheet, " +
            "simply hit the \"X\" button in the top right corner, or select File and then Close, " +
            "and your current spreadsheet will close.";

        private FormClosingEventArgs WindowClosing;

        public spreadsheetWindow()
        {
            InitializeComponent();
            spreadsheetPanelOne.SelectionChanged += HandleChangedSelection;
            SetSelection(0, 0);
            ActiveControl = spreadsheetPanelOne;
        }

        public event Action ChangeContents;

        public event Action CloseEvent;

        public event Action NewCellSelected;

        public event Action NewEvent;

        public event Action<string> OpenEvent;

        public event Action<string> SaveEvent;

        /// <summary>
        /// Starts the process for closing the form, if the current data hasn't been saved.
        /// </summary>
        public void BeginCloseWithoutSave()
        {
            // Create a new close dialog and add our two event Handlers.
            cd = new CloseDialog();
            cd.CloseWithoutSaving += HandleCloseWithoutSave;
            cd.Cancel += HandleCancelCloseWithoutSave;
            cd.CloseDialogClosing += HandleCancelCloseWithoutSave;
            cd.WantsToSave += Save_Clicked;
            // Show the close dialog as a dialog.
            cd.ShowDialog();
        }

        public void CircularFormula()
        {
            MessageBox.Show("That formula creates a circular dependency!");
        }

        /// <summary>
        /// Closes the current spreadsheet window.
        /// </summary>
        public void DoClose()
        {
            Close();
        }

        /// <summary>
        /// Obtains the desired contents for a cell by pulling from the text box's text.
        /// </summary>
        /// <returns></returns>
        public string GetDesiredContents()
        {
            return textBoxOne.Text;
        }

        /// <summary>
        /// Obtains the currently selected cell in the spreadsheet.
        /// </summary>
        /// <param name="col">The current column selected</param>
        /// <param name="row">The current row selected</param>
        public void GetSelection(out int col, out int row)
        {
            spreadsheetPanelOne.GetSelection(out col, out row);
        }

        public void InvalidFormula()
        {
            MessageBox.Show("That is an invalid Formula!");
        }

        /// <summary>
        /// Creates a new, independent spreadsheet window.
        /// </summary>
        public void OpenNew()
        {
            SpreadsheetWindowContext.GetContext().RunNew();
        }

        /// <summary>
        /// Opens a file with the passed in filename.
        /// </summary>
        /// <param name="filename">The name of the file to be opened</param>
        public void OpenNew(string filename)
        {
            SpreadsheetWindowContext.GetContext().RunNew(filename);
        }

        /// <summary>
        /// Method that handles when a new cell is selected by changing the ActiveControl to the
        /// spreadsheet panel and adjusting the text box's text to contents.
        /// </summary>
        /// <param name="contents">
        /// The contents of the currently selected cell to be displayed in the text box
        /// </param>
        public void SelectedNewCell(string contents)
        {
            ActiveControl = spreadsheetPanelOne;
            textBoxOne.Text = contents;
        }

        /// <summary>
        /// Sets the contents of the cell at the passed in location to the passed in contents.
        /// </summary>
        /// <param name="row">The row of the cell to be selected</param>
        /// <param name="col">The column of the cell to be selected</param>
        /// <param name="content">The content of the cell to be selected</param>
        public void SetValue(int col, int row, string content)
        {
            spreadsheetPanelOne.SetValue(col, row, content);
        }

        /// <summary>
        /// Private Event Handler that is called whenever the close button is clicked.
        /// </summary>
        /// <param name="sender">The object calling this method</param>
        /// <param name="e">The arguments associated with this event</param>
        private void Close_Clicked(object sender, EventArgs e)
        {
            if (CloseEvent != null)
            {
                DoClose();
            }
        }

        /// <summary>
        /// Private Event Handler for the close dialog box that is called whenever the user decides
        /// to cancel their decision to close without saving.
        /// </summary>
        private void HandleCancelCloseWithoutSave()
        {
            cd.CloseDialogClosing -= HandleCancelCloseWithoutSave;
            cd.Close();
            WindowClosing.Cancel = true;
            FormClosing += SpreadsheetForm_Closing;
        }

        /// <summary>
        /// Private Event Handler that is called whenever a new cell is selected in the spreadsheet panel.
        /// </summary>
        /// <param name="sender"></param>
        private void HandleChangedSelection(SpreadsheetPanel sender)
        {
            NewCellSelected();
        }

        /// <summary>
        /// Private Event Handler for the close dialog box that is called whenever the user decides
        /// to close the current window without saving.
        /// </summary>
        private void HandleCloseWithoutSave()
        {
            cd.Close();
            FormClosing -= SpreadsheetForm_Closing;
            DoClose();
        }

        /// <summary>
        /// Private Event Handler that is called whenever the user clicks the help button.
        /// </summary>
        /// <param name="sender">The object calling this method</param>
        /// <param name="e">The arguments associated with this event</param>
        private void Help_Clicked(object sender, EventArgs e)
        {
            helpForm = new SendHelpForm();
            helpForm.ShowDialog();
        }

        /// <summary>
        /// Private Event Handler that is called whenever the user clicks the file_NewMenuItem.
        /// </summary>
        /// <param name="sender">The object calling this method</param>
        /// <param name="e">The arguments associated with this event</param>
        private void New_Clicked(object sender, EventArgs e)
        {
            if (NewEvent != null)
            {
                NewEvent();
            }
        }

        /// <summary>
        /// Private Event Handler that is called whenever the file_OpenMenuItem is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Open_Click(object sender, EventArgs e)
        {
            // Show the open file dialog and filter it to only show *.ss files.
            openSpreadsheetDialog.DefaultExt = "Spreadsheet Files (*.ss)|*.ss";
            DialogResult result = openSpreadsheetDialog.ShowDialog();
            // Call the open event if the user clicks a file and hits okay, otherwise do nothing.
            if (result == DialogResult.OK || result == DialogResult.Yes)
            {
                if (OpenEvent != null)
                {
                    try
                    {
                        OpenEvent(Path.GetFullPath(openSpreadsheetDialog.FileName));

                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Unable to read file. \n" +
                            "Please only select files with a *.ss extension.");
                    }
                }
            }
        }

        /// <summary>
        /// Private Event Handler that is called whenever the file_SaveMenuItem control is clicked.
        /// </summary>
        /// <param name="sender">The object calling this method</param>
        /// <param name="e">The event arguments associated with this event</param>
        private void Save_Clicked(object sender, EventArgs e)
        {
            saveSpreadsheetDialog.Filter = "Spreadsheet Files (*.ss)|*.ss| All Files | *.*";
            saveSpreadsheetDialog.AddExtension = true;
            DialogResult result = saveSpreadsheetDialog.ShowDialog();
            if (result == DialogResult.OK || result == DialogResult.Yes)
            {
                if (SaveEvent != null)
                {
                    SaveEvent(Path.GetFullPath(saveSpreadsheetDialog.FileName));
                }
            }
        }

        /// <summary>
        /// Helper method that sets the current cell selection to the passed in column and row.
        /// </summary>
        /// <param name="col">The desired column to be selected</param>
        /// <param name="row">The desired row to be selected</param>
        private void SetSelection(int col, int row)
        {
            spreadsheetPanelOne.SetSelection(col, row);
        }

        /// <summary>
        /// Private Event Handler that is called whenever the entire form starts closing. Ensures
        /// that no data is lost, that the event can be canceled if needed, and that all appropriate
        /// closing steps are otherwise taken.
        /// </summary>
        /// <param name="sender">The object calling this method</param>
        /// <param name="e">:The Form closing event arguments associated with this event</param>
        private void SpreadsheetForm_Closing(object sender, FormClosingEventArgs e)
        {
            // Remove this method from the FormClosing event so it doesn't fire again if the window
            // actually needs to be closed.
            FormClosing -= SpreadsheetForm_Closing;
            // Set aside the FormClosingEventArgs so the close can be canceled just in case the user
            // has unsaved data they'd like to save.
            WindowClosing = e;
            // Call the method associated with the close event.
            if (CloseEvent != null)
            {
                CloseEvent();
            }
        }

        /// <summary>
        /// Private Event Handler that is called whenever the spreadsheet panel has focus and the
        /// KeyDown event is fired.
        /// </summary>
        /// <param name="sender">The object calling this method</param>
        /// <param name="e">
        /// The key event arguments associated with this event
        /// </param>
        private void SpreadsheetPanel_KeyDown(object sender, KeyEventArgs e)
        {
            int col, row;
            switch (e.KeyData)
            {
                case Keys.Up:
                    // Move the current cell selection up one.
                    GetSelection(out col, out row);
                    SetSelection(col, row - 1);
                    NewCellSelected();
                    break;

                case Keys.Down:
                    // Move the current cell selection down one.
                    GetSelection(out col, out row);
                    SetSelection(col, row + 1);
                    NewCellSelected();
                    break;

                case Keys.Right:
                    // Move the current cell selection right one.
                    GetSelection(out col, out row);
                    SetSelection(col + 1, row);
                    NewCellSelected();
                    break;

                case Keys.Left:
                    // Move the current cell selection left one.
                    GetSelection(out col, out row);
                    SetSelection(col - 1, row);
                    NewCellSelected();
                    break;

                case Keys.Enter:
                    // Move the current cell selection down one.
                    GetSelection(out col, out row);
                    SetSelection(col, row + 1);
                    NewCellSelected();
                    break;

                case Keys.Tab:
                    // Move the current cell selection one to the right.
                    GetSelection(out col, out row);
                    SetSelection(col + 1, row);
                    NewCellSelected();
                    break;

                case Keys.Back:
                    textBoxOne.Text = "";
                    ChangeContents();
                    break;
            }
        }

        /// <summary>
        /// Private Event Handler that is called whenever the spreadsheet panel has focus and the
        /// KeyPressed Event is fired.
        /// </summary>
        /// <param name="sender">The object calling this method</param>
        /// <param name="e">The key press event arguments associated with this event</param>
        private void SpreadsheetPanel_KeyPressed(object sender, KeyPressEventArgs e)
        {
            // If the character is a control character, this method shouldn't do anything.
            if (char.IsControl(e.KeyChar))
            {
                return;
            }
            // However, if the key character is an actual character, append it to the end of the text
            // box's text, move the text box so it's cursor is at the end, and transfer control to
            // the text box.
            textBoxOne.Text += e.KeyChar;
            textBoxOne.Select(textBoxOne.TextLength, 0);
            ActiveControl = textBoxOne;
        }

        /// <summary>
        /// Private Event Handler that is called whenever the text box has focus and the KeyDown
        /// event is fired.
        /// </summary>
        /// <param name="sender">The object calling this method</param>
        /// <param name="e">The key event arguments associated with this event</param>
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // If the pressed key is the enter key, change the current cell's contents, suppress the
            // annoying Windows error ding, and transfer control to the spreadsheet panel.
            if (e.KeyData == Keys.Enter)
            {
                ChangeContents();
                e.SuppressKeyPress = true;
                ActiveControl = spreadsheetPanelOne;
            }
        }

        private void spreadsheetPanelOne_Load(object sender, EventArgs e)
        {

        }

        public void SetTitle(string filename)
        {
            string title = Path.GetFileNameWithoutExtension(filename);
            this.Text = title;
        }
    }
}