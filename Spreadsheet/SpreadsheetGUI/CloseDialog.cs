using System;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    public partial class CloseDialog : Form
    {
        public event Action CloseWithoutSaving;
        public event Action Cancel;

        

        public CloseDialog()
        {
            InitializeComponent();
        }

        private void Yes_Clicked(object sender, EventArgs e)
        {
            if (CloseWithoutSaving != null)
            {
                CloseWithoutSaving();
            }
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            if (Cancel != null)
            {
                Cancel();
            }
        }
    }
}
