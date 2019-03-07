using System;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    public partial class CloseDialog : Form
    {
        public CloseDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Allows for the save button method inside of spreadsheet window to be called when the save
        /// button on the close dialog is pressed. Yay for not reusing code.
        /// </summary>
        /// <param name="sender">Irrelevant here. Just ignore it</param>
        /// <param name="e">Also irrelevant. Don't use it/param>
        public delegate void SaveClicked(object sender, MouseEventArgs e);

        /// <summary>
        /// An event to be fired whenever the cancel button is pressed.
        /// </summary>
        public event Action Cancel;

        /// <summary>
        /// An event to be fired whenever this form is closing.
        /// </summary>
        public event Action CloseDialogClosing;

        /// <summary>
        /// An event to be fired when the "Yes, I want to close without saving" button is pressed.
        /// </summary>
        public event Action CloseWithoutSaving;

        /// <summary>
        /// An event to be fired when the user clicks the save button on the close dialog.
        /// </summary>
        public event SaveClicked WantsToSave;

        /// <summary>
        /// An Event Handler that is called whenever the cancel button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Clicked(object sender, EventArgs e)
        {
            if (Cancel != null)
            {
                Cancel();
            }
        }

        /// <summary>
        /// An Event Handler that is called whenever this form starts closing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exiting_CloseDialog(object sender, FormClosingEventArgs e)
        {
            if (CloseDialogClosing != null)
            {
                CloseDialogClosing();
            }
        }

        /// <summary>
        /// An Event Handler that is called whenever the save button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Clicked(object sender, MouseEventArgs e)
        {
            if (WantsToSave != null)
            {
                Close();
                WantsToSave(sender, e);
            }
        }

        /// <summary>
        /// An Event Handler that is called whenever the close without saving button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Yes_Clicked(object sender, EventArgs e)
        {
            if (CloseWithoutSaving != null)
            {
                CloseWithoutSaving();
            }

        }
    }
}