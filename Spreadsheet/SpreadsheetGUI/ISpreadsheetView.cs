using System;

namespace SpreadsheetGUI
{
    public interface ISpreadsheetView
    {
        event Action ChangeContents;

        event Action CloseEvent;

        event Action NewCellSelected;

        event Action NewEvent;

        event Action<string> OpenEvent;

        event Action<string> SaveEvent;

        void BeginCloseWithoutSave();

        void DoClose();

        string GetDesiredContents();

        void GetSelection(out int col, out int row);

        void InvalidFormula();

        void CircularFormula();

        void OpenNew();

        void OpenNew(string filename);

        void SelectedNewCell(string contents);

        void SetValue(int col, int row, string content);
    }
}