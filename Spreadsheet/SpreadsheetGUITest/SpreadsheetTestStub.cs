using SpreadsheetGUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUITest
{
    class SpreadsheetTestStub : ISpreadsheetView
    {
        public event Action ChangeContents;
        public event Action CloseEvent;
        public event Action NewCellSelected;
        public event Action NewEvent;
        public event Action<string> OpenEvent;
        public event Action<string> SaveEvent;

        public void BeginCloseWithoutSave()
        {
            throw new NotImplementedException();
        }

        public void DoClose()
        {
            throw new NotImplementedException();
        }

        public string GetDesiredContents()
        {
            throw new NotImplementedException();
        }

        public void GetSelection(out int col, out int row)
        {
            throw new NotImplementedException();
        }

        public void OpenNew()
        {
            throw new NotImplementedException();
        }

        public void OpenNew(string filename)
        {
            throw new NotImplementedException();
        }

        public void SelectedNewCell(string contents)
        {
            throw new NotImplementedException();
        }

        public void SetValue(int col, int row, string content)
        {
            throw new NotImplementedException();
        }
    }

}
