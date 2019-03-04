using SSGui;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SS;

namespace SpreadsheetGUI
{

    public partial class SpreadsheetWindow : Form
    {
        private Spreadsheet s;
        private int currentRow;
        private int currentColumn;
        private string cellName;
        public SpreadsheetWindow()
        {
            InitializeComponent();
            s = new Spreadsheet();
            currentRow = 0;
            currentColumn = 0;
            spreadsheetPanel1.SelectionChanged += DisplaySelection;
            spreadsheetPanel1.SetSelection(0, 0);

        }

        private void DisplaySelection(SpreadsheetPanel ss)
        {
            ss.GetSelection(out currentColumn, out currentRow);
            cellName = ((char)('A' + currentColumn)).ToString() + (currentRow + 1).ToString();
        }

    }
}
