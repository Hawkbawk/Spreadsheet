using SpreadsheetGUI;
using System;

namespace SpreadsheetGUITest
{
    class ControllerTester : ISpreadsheetView
    {
        public event Action ChangeContents;
        public event Action CloseEvent;
        public event Action NewCellSelected;
        public event Action NewEvent;
        public event Action<string> OpenEvent;
        public event Action<string> SaveEvent;

        public string filename
        {
            get; private set;
        }
        public string cellContents
        {
            get; set;
        }

        public int selectedColumn
        {
            get; set;
        }

        public int selectedRow
        {
            get; set;
        }

        public void FireChangeContents()
        {
            if (ChangeContents != null)
            {
                ChangeContents();
            }
        }

        public void FireCloseEvent()
        {
            if (CloseEvent != null)
            {
                CloseEvent();
            }
        }
        public void FireNewCellSelected()
        {
            if (NewCellSelected != null)
            {
                NewCellSelected();
            }
        }
        public void FireNewEvent()
        {
            if (NewEvent != null)
            {
                NewEvent();
            }
        }
        public void FireOpenEvent(string filename)
        {
            if (OpenEvent != null)
            {
                OpenEvent(filename);
            }
        }
        public void FireSaveEvent(string filename)
        {
            if (SaveEvent != null)
            {
                SaveEvent(filename);
            }
        }
        public bool CalledInvalidFormula
        {
            get; private set;
        }

        public bool CalledCircularFormula
        {
            get; private set;
        }
        public bool CalledBeginCloseWithoutSave
        {
            get; private set;
        }

        public bool CalledDoClose
        {
            get; private set;
        }
        public bool CalledGetDesiredContents
        {
            get; private set;
        }
        public bool CalledGetSelection
        {
            get; private set;
        }
        public bool CalledOpenNew
        {
            get; private set;
        }
        public bool CalledSelectedNewCell
        {
            get; private set;
        }
        public bool CalledSetValue
        {
            get; private set;
        }

        public void BeginCloseWithoutSave()
        {
            CalledBeginCloseWithoutSave = true;
        }

        public void DoClose()
        {
            CalledDoClose = true;
        }

        public string GetDesiredContents()
        {
            CalledGetDesiredContents = true;
            if (cellContents == null)
            {
                return "";
            }
            return cellContents;
        }

        public void GetSelection(out int col, out int row)
        {
            CalledGetSelection = true;
            col = 0;
            row = 0;
        }

        public void OpenNew()
        {
            CalledOpenNew = true;
        }

        public void OpenNew(string filename)
        {
            this.filename = filename;
            CalledOpenNew = true;
        }

        public void SelectedNewCell(string contents)
        {
            cellContents = contents;
            CalledSelectedNewCell = true;
        }

        public void SetValue(int col, int row, string content)
        {
            CalledSetValue = true;
            cellContents = content;
        }

        public void InvalidFormula()
        {
            CalledInvalidFormula = true;
        }

        public void CircularFormula()
        {
            CalledCircularFormula = true;
        }
    }

}
