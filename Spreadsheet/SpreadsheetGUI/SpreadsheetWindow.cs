using SSGui;
using System;
using System.IO;
using System.Windows.Forms;

namespace SpreadsheetGUI
{

    public partial class SpreadsheetWindow : Form, ISpreadsheetView
    {

        private string helpMessage = "Hey there! So you want to know how to use this spreadsheet program? Well, it's fairly" +
            " similar to Excel, with a few differences. First, if you want to change the contents of a cell, simply click on " +
            "the cell and enter in your desired cell contents in the Cell Contexts text box, then press enter. \nYou can use formulas," +
            " just like Excel! Your formulas can contain links to the values in other cells, by using a format such as \"= A4 + A5\". You" +
            " can also just put in any numbers you please, or text. If you want to open a new spreadsheet window, just hit File and then " +
            "New and a new window will open up. If you want to save a spreadsheet, just hit File and then Save. A File Explorer box will then" +
            " open up and you can then decide where to save your file. \nIf you want to open up a previously saved spreadsheet, simply hit File" +
            " and then Open. A File Explorer box will then open up and then you can navigate to the desired file. This program works with only with .ss files" +
            ". If you try and open another file, an error message will popup, telling you to try again. \nIf you want to close your current spreadsheet, " +
            "simply hit the \"X\" button in the top right corner, or select File and then Close, and your current spreadsheet will close.";
        public event Action NewEvent;
        public event Action<string> SaveEvent;
        public event Action<string> OpenEvent;
        public event Action CloseEvent;
        public event Action ChangeContents;
        public event Action NewCellSelected;

        // A private instance variable for telling if the shift key has been pressed. Allows for
        // Shift+Tab and Shift+Enter to move the current cell selection, just like in Excel.
        private bool ShiftPressed { get; set; }

        public SpreadsheetWindow()
        {
            InitializeComponent();
            spreadsheetPanelOne.SelectionChanged += HandleChangedSelection;
            spreadsheetPanelOne.SetSelection(0, 0);
            ActiveControl = spreadsheetPanelOne;

        }


        private void HandleChangedSelection(SpreadsheetPanel sender)
        {
            NewCellSelected();
        }

        public string GetDesiredContents()
        {
            return textBoxOne.Text;
        }

        public SpreadsheetPanel GetSpreadsheetPanel()
        {
            return spreadsheetPanelOne;
        }

        public void GetSelection(out int col, out int row)
        {
            spreadsheetPanelOne.GetSelection(out col, out row);
        }

        public void SetSelection(int col, int row)
        {
            spreadsheetPanelOne.SetSelection(col, row);
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                ChangeContents();
                ActiveControl = spreadsheetPanelOne;
                e.SuppressKeyPress = true;
            }
        }

        public void GetValue(int row, int col, out string contents)
        {
            spreadsheetPanelOne.GetValue(row, col, out contents);

        }

        public void SetValue(int row, int col, string content)
        {
            spreadsheetPanelOne.SetValue(col, row, content);
        }

        private void Help_Clicked(object sender, EventArgs e)
        {
            MessageBox.Show(helpMessage);
        }

        public void SelectedNewCell(string contents)
        {
            ActiveControl = spreadsheetPanelOne;
            textBoxOne.Text = contents;
        }

        public void OpenNew()
        {
            SpreadsheetWindowContext.GetContext().RunNew();
        }

        public void OpenNew(string filename)
        {
            SpreadsheetWindowContext.GetContext().RunNew(filename);
        }

        public void DoClose()
        {
            Close();
        }

        private void New_Clicked(object sender, EventArgs e)
        {
            if (NewEvent != null)
            {
                NewEvent();
            }
        }

        private void Close_Clicked(object sender, EventArgs e)
        {
            if (CloseEvent != null)
            {
                CloseEvent();
            }
        }

        private void SpreadsheetPanel_KeyDown(object sender, KeyEventArgs e)
        {
            int col, row;
            switch (e.KeyData)
            {
                case Keys.Up:
                    // Move the current cell selection up one.
                    GetSelection(out col, out row);
                    spreadsheetPanelOne.SetSelection(col, row - 1);
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
                    spreadsheetPanelOne.SetSelection(col - 1, row);
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
                    spreadsheetPanelOne.SetSelection(col + 1, row);
                    NewCellSelected();
                    break;
                case Keys.Shift:
                    ShiftPressed = e.Shift;
                    break;
                case Keys.Back:
                    textBoxOne.Text = "";
                    ChangeContents();
                    break;
            }

        }

        public void CloseWithoutSave()
        {
            if (CloseEvent != null)
            {
                CloseEvent();
            }
        }

        private void Save_Clicked(object sender, EventArgs e)
        {
            saveSpreadsheetDialog.Filter = "Spreadsheet Files (*.ss)|*.ss";
            DialogResult result = saveSpreadsheetDialog.ShowDialog();
            if (result == DialogResult.OK || result == DialogResult.Yes)
            {
                if (SaveEvent != null)
                {
                    SaveEvent(Path.GetFullPath(saveSpreadsheetDialog.FileName));
                }

            }
        }

        private void Open_Click(object sender, EventArgs e)
        {
            openSpreadsheetDialog.Filter = "Spreadsheet Files (*.ss)|*.ss";
            DialogResult result = openSpreadsheetDialog.ShowDialog();
            if (result == DialogResult.OK || result == DialogResult.Yes)
            {
                if (OpenEvent != null)
                {
                    OpenEvent(Path.GetFullPath(openSpreadsheetDialog.FileName));
                }

            }
        }

        private void SpreadsheetPanel_KeyPressed(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
            {
                return;
            }
            textBoxOne.Text += e.KeyChar;
            
            textBoxOne.Select(textBoxOne.TextLength, 0);
            ActiveControl = textBoxOne;
        }
    }
}
