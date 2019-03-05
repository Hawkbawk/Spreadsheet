﻿using SSGui;
using System;

namespace SpreadsheetGUI
{
    public interface ISpreadsheetView
    {
        event Action NewEvent;
        event Action<string> SaveEvent;
        event Action<string> OpenEvent;
        event Action CloseEvent;
        event Action ChangeContents;


        SpreadsheetPanel GetSpreadsheetPanel();
        string GetDesiredContents();
        void GetValue(int row, int col, out string contents);
        void SetValue(int row, int col, string content);
        void GetSelection(out int row, out int col);
        void SetSelection(int row, int col);
        void ChangeTextbox(string contents);
        void OpenNew();
        void DoClose();

    }
}