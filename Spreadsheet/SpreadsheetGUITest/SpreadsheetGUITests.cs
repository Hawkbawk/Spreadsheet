using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetGUI;

namespace SpreadsheetGUITest
{
    [TestClass]
    public class SpreadsheetGUITests
    {
        
        public void TestBase()
        {
            SpreadsheetTestStub stub = new SpreadsheetTestStub();
            SpreadsheetController controller = new SpreadsheetController(stub);

        }

        [TestMethod]
        public void TestCloseEmptySpreadsheet()
        {
            SpreadsheetTestStub stub = new SpreadsheetTestStub();
            SpreadsheetController controller = new SpreadsheetController(stub);
            
        }
    }
}
