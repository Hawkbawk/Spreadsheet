using SSGui;
using System;
using System.IO;
using System.Windows.Forms;

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

        void DoClose();

        string GetDesiredContents();

        void GetSelection(out int col, out int row);
        void OpenNew();

        void OpenNew(string filename);

        void HandleSelectedNewCell(string contents);

        void SetValue(int col, int row, string content);
        void BeginCloseWithoutSave();
    }
}
