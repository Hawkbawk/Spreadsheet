using SSGui;
using System;
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

        public SpreadsheetWindow()
        {
            InitializeComponent();
            spreadsheetPanel1.SelectionChanged += HandleChangedSelection;
            spreadsheetPanel1.SetSelection(0, 0);

        }


        private void HandleChangedSelection(SpreadsheetPanel sender)
        {
            NewCellSelected();
        }

        public string GetDesiredContents()
        {
            return textBox1.Text;
        }

        public SpreadsheetPanel GetSpreadsheetPanel()
        {
            return spreadsheetPanel1;
        }

        public void GetSelection(out int col, out int row)
        {
            spreadsheetPanel1.GetSelection(out col, out row);
        }

        public void SetSelection(int col, int row)
        {
            spreadsheetPanel1.SetSelection(col, row);
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                ChangeContents();
                this.ActiveControl = spreadsheetPanel1;
                e.SuppressKeyPress = true;
            }
        }

        public void GetValue(int row, int col, out string contents)
        {
            spreadsheetPanel1.GetValue(row, col, out contents);

        }

        public void SetValue(int row, int col, string content)
        {
            spreadsheetPanel1.SetValue(col, row, content);
        }

        private void Help_Clicked(object sender, EventArgs e)
        {
            MessageBox.Show(helpMessage);
        }

        public void SelectedNewCell(string contents)
        {
            textBox1.Text = contents;
        }

        public void OpenNew()
        {
            SpreadsheetWindowContext.GetContext().RunNew();
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

        private void SpreadsheetPanel_Hold(object sender, KeyEventArgs e)
        {
            int col, row;
            switch (e.KeyData)
            {
                case Keys.Up:
                    GetSelection(out col, out row);
                    spreadsheetPanel1.SetSelection(col, row - 1);
                    break;
                case Keys.Down:
                    GetSelection(out col, out row);
                    SetSelection(col, row + 1);
                    break;
                case Keys.Right:
                    GetSelection(out col, out row);
                    spreadsheetPanel1.SetSelection(col + 1, row);
                    break;
                case Keys.Left:
                    GetSelection(out col, out row);
                    spreadsheetPanel1.SetSelection(col - 1, row);
                    break;
                default:
                    break;
            }
        }

        private void SpreadsheetWindow_PreveiewKeyPress(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Left:
                    e.IsInputKey = true;
                    break;
            }
        }
    }
}
