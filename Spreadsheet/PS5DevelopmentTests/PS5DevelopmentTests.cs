﻿using Formulas;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using System.Collections.Generic;

namespace DevelopmentTests
{
    /// <summary>
    /// These are developments tests for PS5, to assert that the Spreadsheet
    /// class is performing as outlined.
    ///</summary>
    [TestClass()]
    public class SpreadsheetTest
    {
        /// <summary>
        /// Checks to see if references are being passed, instead of deep copies.
        /// </summary>
        [TestMethod]
        public void CheckingForReferencePassing1()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("t1", "Hello world!");
            var tester = s.GetCellContents("t1");
            tester = "I've been changed!";
            Assert.AreEqual("Hello world!", s.GetCellContents("t1"));
        }

        /// <summary>
        /// Checks to see if references to the formula are being passed, instead
        /// of a deep copy.
        /// </summary>
        [TestMethod]
        public void CheckingForReferencePassing2()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            Formula f = new Formula("9 + 7");
            s.SetContentsOfCell("t1", "f");
            var tester = s.GetCellContents("t1");
            tester = new Formula("123 + 17");
            Assert.AreNotEqual(f.ToString(), tester.ToString());
            f = new Formula("9 + 71");
            Assert.AreNotEqual(f, s.GetCellContents("t1"));
        }

        /// <summary>
        /// One final check to make sure that changing the cell contents doesn't
        /// change what someone has pulled out of the spreadsheet.
        /// </summary>
        [TestMethod]
        public void CheckingForReferencePassing3()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("t1", "Hello world!");
            var tester = s.GetCellContents("t1");
            s.SetContentsOfCell("t1", "I've been changed");
            Assert.AreNotEqual(tester, s.GetCellContents("t1"));
        }

        /// <summary>
        /// Tests to see that an invalid name throws an exception for the get
        /// method.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void InvalidNameGetTest()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.GetCellContents("5tyi8");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void InvalidNameSetTest1()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("st5t", "5");
        }

        /// <summary>
        /// Checks to see that an invalid name throws an exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void InvalidNameSetTest2()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a4", "");
            s.SetContentsOfCell("a%", "asdffj;l");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void InvalidNameSetTest3()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("st5t", "9 + 182");
        }

        /// <summary>
        /// Test that only cells with non-empty contents are being returned.
        /// </summary>
        [TestMethod]
        public void NonEmptyCellTest()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("t1", "");
            s.SetContentsOfCell("t2", "a");
            s.SetContentsOfCell("t3", "4567");
            s.SetContentsOfCell("t4", "4 + 17 - 92 / 4");
            var expected = new HashSet<string>();
            expected.Add("T2");
            expected.Add("T3");
            expected.Add("T4");
            Assert.IsTrue(expected.SetEquals(s.GetNamesOfAllNonemptyCells()));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void NullGetTest1()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.GetCellContents(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void NullSetTest1()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell(null, "5");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void NullSetTest2()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell(null, " ");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void NullSetTest3()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell(null, "9");
        }

        // SETTING CELL TO A STRING
        [TestMethod()]
        public void Test10()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("Z7", "hello");
            Assert.AreEqual("hello", s.GetCellContents("Z7"));
        }

        // SETTING CELL TO A FORMULA
        [TestMethod()]
        public void Test13()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("Z7", "=3");
            Formula f = (Formula)s.GetCellContents("Z7");
            Assert.AreEqual(3, f.Evaluate(x => 0), 1e-6);
        }

        // CIRCULAR FORMULA DETECTION
        [TestMethod()]
        [ExpectedException(typeof(CircularException))]
        public void Test14()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=A2");
            s.SetContentsOfCell("A2", "=A1");
        }

        // EMPTY SPREADSHEETS
        [TestMethod()]
        public void Test3()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            Assert.AreEqual("", s.GetCellContents("A2"));
        }

        // SETTING CELL TO A DOUBLE
        [TestMethod()]
        public void Test6()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("Z7", "1.5");
            Assert.AreEqual(1.5, (double)s.GetCellContents("Z7"), 1e-9);
        }

        [TestMethod]
        public void TestDirectDependents()
        {
            var s = new Spreadsheet();
            s.SetContentsOfCell("b1", "a1 + b3 - c7");
        }
    }
}