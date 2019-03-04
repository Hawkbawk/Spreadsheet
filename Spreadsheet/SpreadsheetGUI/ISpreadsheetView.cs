using System;

namespace SpreadsheetGUI
{
    interface ISpreadsheetView
    {
        event Action NewEvent;
        event Action SaveEvent;
        event Action OpenEvent;
        event Action CloseEvent;

    }
}
