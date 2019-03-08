using System;
using System.Windows.Forms;

namespace SpreadsheetGUI
{


    public partial class SendHelpForm : Form
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
        public SendHelpForm()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
