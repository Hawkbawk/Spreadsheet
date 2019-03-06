using System;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    public partial class CloseDialog : Form
    {
        public bool CloseWithoutSaving { get; private set; }

        public delegate void HandleCloseWithoutSave();

        public event HandleCloseWithoutSave CloseWithoutSave;

        public CloseDialog()
        {
            InitializeComponent();
            CloseWithoutSaving = false;
        }

        private void Yes_Clicked(object sender, EventArgs e)
        {
            CloseWithoutSaving = true;
            if (CloseWithoutSave != null)
            {
                CloseWithoutSave();
            }
            Close();
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            CloseWithoutSaving = false;
            if (CloseWithoutSave != null)
            {
                CloseWithoutSave();
            }
            Close();
        }
    }
}
