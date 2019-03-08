using Formulas;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetGUI;

namespace SpreadsheetGUITest
{
    [TestClass]
    public class SpreadsheetGUITests
    {

        //public void TestBase()
        //{
        //    SpreadsheetTestStub stub = new SpreadsheetTestStub();
        //    SpreadsheetController controller = new SpreadsheetController(stub);
        //}

        [TestMethod]
        public void TestCloseEmptySpreadsheet()
        {
            ControllerTester stub = new ControllerTester();
            Controller controller = new Controller(stub);
            stub.FireNewEvent();
            Assert.IsTrue(stub.CalledOpenNew);
            stub.FireCloseEvent();
            Assert.IsTrue(stub.CalledDoClose);
        }

        [TestMethod]
        public void TestChangedContentsAndClosed()
        {
            ControllerTester stub = new ControllerTester();
            Controller controller = new Controller(stub);
            stub.FireNewEvent();
            Assert.IsTrue(stub.CalledOpenNew);
            stub.FireChangeContents();
            Assert.IsTrue(stub.CalledSetValue);
            Assert.IsTrue(stub.CalledGetDesiredContents);
            Assert.IsTrue(stub.CalledGetSelection);
        }

        [TestMethod]
        public void TestGetInvalidCellName()
        {
            ControllerTester stub = new ControllerTester();
            Controller controller = new Controller(stub);
            stub.cellContents = "=+D5";

            stub.FireChangeContents();
            Assert.IsTrue(stub.CalledInvalidFormula);
        }

        [TestMethod]
        public void TestGetCircularException()
        {
            ControllerTester stub = new ControllerTester();
            Controller controller = new Controller(stub);
            stub.selectedRow = 0;
            stub.selectedColumn = 0;
            stub.cellContents = "=A1";
            stub.FireChangeContents();
            Assert.IsTrue(stub.CalledCircularFormula);
        }

        [TestMethod]
        public void TestGetCircularException1()
        {
            ControllerTester stub = new ControllerTester();
            Controller controller = new Controller(stub);
            stub.selectedRow = 0;
            stub.selectedColumn = 1;
            stub.cellContents = "=B1";
            stub.FireChangeContents();
            Assert.IsTrue(stub.CalledCircularFormula);
        }

        [TestMethod]
        public void TestChangedContents()
        {
            ControllerTester stub = new ControllerTester();
            Controller controller = new Controller(stub);
            stub.selectedRow = 0;
            stub.selectedColumn = 0;
            stub.cellContents = "=D2";
            stub.FireChangeContents();
            Assert.IsTrue(stub.CalledGetDesiredContents);
        }


        [TestMethod]
        public void TestClosedWithoutSaving()
        {
            ControllerTester stub = new ControllerTester();
            Controller controller = new Controller(stub);
            stub.selectedRow = 0;
            stub.selectedColumn = 0;
            stub.cellContents = "5";
            stub.FireChangeContents();
            stub.selectedRow = 1;
            stub.selectedColumn = 1;
            stub.FireNewCellSelected();
            Assert.IsTrue(stub.CalledSelectedNewCell);
            Assert.IsTrue(stub.CalledGetDesiredContents);
            stub.FireCloseEvent();
            Assert.IsTrue(stub.CalledBeginCloseWithoutSave);
        }

        [TestMethod]
        public void TestSaveAndOpen()
        {
            ControllerTester stub = new ControllerTester();
            Controller controller = new Controller(stub);
            stub.selectedRow = 0;
            stub.selectedColumn = 0;
            stub.cellContents = "5";
            stub.FireChangeContents();
            stub.selectedRow = 1;
            stub.selectedColumn = 1;
            stub.cellContents = "=A1";
            stub.FireNewCellSelected();
            Assert.IsTrue(stub.CalledSelectedNewCell);
            Assert.IsTrue(stub.CalledGetDesiredContents);
            string filename = "TestSave.ss";
            stub.FireSaveEvent(filename);
            stub.FireOpenEvent(filename);
            Assert.IsTrue(stub.CalledOpenNew);
        }

    }

}
