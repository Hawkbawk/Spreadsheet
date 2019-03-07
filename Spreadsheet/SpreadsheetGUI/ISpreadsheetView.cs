using SSGui;
using System;
using System.IO;

namespace SpreadsheetGUI
{
    public interface ISpreadsheetView
    {
        event Action NewEvent;
        event Action<string> SaveEvent;
        event Action<string> OpenEvent;
        event Action CloseEvent;
        event Action ChangeContents;
        event Action NewCellSelected;

        string GetDesiredContents();
        void GetValue(int row, int col, out string contents);
        void SetValue(int row, int col, string content);
        void GetSelection(out int row, out int col);
        void SetSelection(int row, int col);
        void SelectedNewCell(string contents);

        void CloseWithoutSave();
        void OpenNew();

        void OpenNew(string filename);
        void DoClose();

    }
}
