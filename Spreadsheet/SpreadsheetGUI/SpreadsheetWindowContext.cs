using System.Windows.Forms;

namespace SpreadsheetGUI
{
    class SpreadsheetWindowContext : ApplicationContext
    {
        private int windowCount = 0;
        private static SpreadsheetWindowContext context;

        private SpreadsheetWindowContext()
        {

        }

        public static SpreadsheetWindowContext GetContext()
        {
            if (context == null)
            {
                context = new SpreadsheetWindowContext();
            }
            return context;
        }

        public void RunNew()
        {
            spreadsheetWindow window = new spreadsheetWindow();
            new SpreadsheetController(window);

            windowCount++;

            window.FormClosed += (o, e) => { if (--windowCount <= 0) ExitThread(); };

            window.Show();
        }

        public void RunNew(string filename)
        {
            spreadsheetWindow window = new spreadsheetWindow();

            new SpreadsheetController(window, filename);

            windowCount++;

            window.FormClosed += (o, e) => { if (--windowCount <= 0) ExitThread(); };

            window.Show();
        }
    }
}
