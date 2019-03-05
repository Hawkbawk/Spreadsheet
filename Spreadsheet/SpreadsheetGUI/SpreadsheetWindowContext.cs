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
            SpreadsheetWindow window = new SpreadsheetWindow();
            new SpreadsheetController(window);

            windowCount++;

            window.FormClosed += (o, e) => { if (--windowCount <= 0) ExitThread(); };

            window.Show();
        }
    }
}
