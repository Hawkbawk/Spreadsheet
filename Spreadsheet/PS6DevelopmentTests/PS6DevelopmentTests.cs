using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace DevelopmentTests
{
    [TestClass]
    public class PS6DevelopmentTests
    {
        // Verifies cells and their values, which must alternate.
        public void VV(AbstractSpreadsheet sheet, params object[] constraints)
        {
            for (int i = 0; i < constraints.Length; i += 2)
            {
                if (constraints[i + 1] is double)
                {
                    Assert.AreEqual((double)constraints[i + 1], (double)sheet.GetCellValue((string)constraints[i]), 1e-9);
                }
                else
                {
                    Assert.AreEqual(constraints[i + 1], sheet.GetCellValue((string)constraints[i]));
                }
            }
        }

        // For setting a spreadsheet cell.
        public IEnumerable<string> Set(AbstractSpreadsheet sheet, string name, string contents)
        {
            List<string> result = new List<string>(sheet.SetContentsOfCell(name, contents));
            return result;
        }

        // Tests 1-argument constructor
        [TestMethod()]
        public void IsValidTest1()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "x");
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void IsValidTest2()
        {
            AbstractSpreadsheet ss = new Spreadsheet(new Regex("^[b-zB-Z]+\\d+$"));
            ss.SetContentsOfCell("A1", "x");
        }

        [TestMethod()]
        public void NormalizeTest2()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "5");
            s.SetContentsOfCell("A1", "6");
            s.SetContentsOfCell("B1", "= a1");
            Assert.AreEqual(6.0, (double)s.GetCellValue("B1"), 1e-9);
        }

        [TestMethod()]
        public void EmptySheet()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            VV(ss, "A1", "");
        }

        [TestMethod]
        public void ValuesUpdatingCorrectly()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "=b1 + c1");
            s.SetContentsOfCell("b1", "5");
            s.SetContentsOfCell("c1", "=b1 * d1");
            s.SetContentsOfCell("d1", "10");
            Assert.AreEqual(55.0, s.GetCellValue("a1"));
            Assert.AreEqual(5.0, s.GetCellValue("b1"));
            Assert.AreEqual(50.0, s.GetCellValue("c1"));
            Assert.AreEqual(10.0, s.GetCellValue("d1"));
            s.SetContentsOfCell("d1", "0");
            Assert.AreEqual(5.0, s.GetCellValue("a1"));
            Assert.AreEqual(5.0, s.GetCellValue("b1"));
            Assert.AreEqual(0.0, s.GetCellValue("c1"));
            Assert.AreEqual(0.0, s.GetCellValue("d1"));

        }

        [TestMethod, Timeout(3000)]
        public void StressTest1()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "=b1");
            s.SetContentsOfCell("b1", "=c1");
            s.SetContentsOfCell("c1", "=d1");
            s.SetContentsOfCell("d1", "=e1");
            s.SetContentsOfCell("e1", "=f1");
            s.SetContentsOfCell("f1", "=g1");
            s.SetContentsOfCell("g1", "=h1");
            s.SetContentsOfCell("h1", "=i1");
            s.SetContentsOfCell("i1", "=j1");
            s.SetContentsOfCell("j1", "=k1");
            s.SetContentsOfCell("k1", "=l1");
            s.SetContentsOfCell("l1", "=m1");
            s.SetContentsOfCell("m1", "5");
            Assert.AreEqual(5.0, s.GetCellValue("a1"));

        }

        [TestMethod, Timeout(3000)]
        public void StressTest2()
        {
            Spreadsheet s = new Spreadsheet();
            for(int i = 1; i < 800; i++)
            {
                s.SetContentsOfCell("a" + i, "=a" + (i + 1));
            }
            s.SetContentsOfCell("a800", "1");
            Assert.AreEqual(1.0, s.GetCellValue("a1"));
        }

        [TestMethod, Timeout(5000)]
        public void StressTest3()
        {
            Spreadsheet s = new Spreadsheet();
            for (int i = 1; i < 500; i++)
            {
                s.SetContentsOfCell("a" + i, "=a" + (i + 1));
            }
            s.SetContentsOfCell("a500", "1");
            for (int i = 1; i < 500; i++)
            {
                Assert.AreEqual(1.0, s.GetCellValue("a" + i));
            }
        }

        /// <summary>
        /// Add a bunch of random things to the spreadsheet and see how long it takes.
        /// </summary>
        [TestMethod, Timeout(3000)]
        public void StressTest4()
        {
            Spreadsheet s = new Spreadsheet();
            Random r = new Random();
            List<double> numbers = new List<double>();
            numbers.Add(0);
            for (int i = 1; i < 800; i++)
            {
                int result = r.Next();
                s.SetContentsOfCell("a" + i, "" + result);
                numbers.Add(result);
            }
            for (int i = 1; i < 800; i++)
            {
                Assert.AreEqual(numbers[i], s.GetCellValue("a" + i));

            }
        }

        [TestMethod()]
        public void OneNumber()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Set(ss, "C1", "17.5");
            VV(ss, "C1", 17.5);
        }

        [TestMethod()]
        public void OneFormula()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Set(ss, "A1", "4.1");
            Set(ss, "B1", "5.2");
            Set(ss, "C1", "= A1+B1");
            VV(ss, "A1", 4.1, "B1", 5.2, "C1", 9.3);
        }

        [TestMethod()]
        public void Changed2()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Assert.IsFalse(ss.Changed);
            Set(ss, "C1", "17.5");
            Assert.IsTrue(ss.Changed);
        }

        [TestMethod]
        public void GetCellContents()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "Hello World!");
            Assert.AreEqual("Hello World!", s.GetCellContents("a1"));
        }

        [TestMethod]
        public void GetCellContentsEmptyCell()
        {
            Spreadsheet s = new Spreadsheet();
            Assert.AreEqual("", s.GetCellContents("v9875"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContentsNull()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellValuesNull()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellValue(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetToNullContents()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a4", null);
        }    
        
        [TestMethod]
        public void SetToEmptyString()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "");
            Assert.AreEqual("", s.GetCellContents("a1"));
        }
        [TestMethod()]
        public void DivisionByZero1()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Set(ss, "A1", "4.1");
            Set(ss, "B1", "0.0");
            Set(ss, "C1", "= A1 / B1");
            Assert.IsInstanceOfType(ss.GetCellValue("C1"), typeof(FormulaError));
        }

        [TestMethod()]
        public void EmptyArgument()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Set(ss, "A1", "4.1");
            Set(ss, "C1", "= A1 + B1");
            Assert.IsInstanceOfType(ss.GetCellValue("C1"), typeof(FormulaError));
        }

        [TestMethod()]
        public void NumberFormula1()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Set(ss, "A1", "4.1");
            Set(ss, "C1", "= A1 + 4.2");
            VV(ss, "C1", 8.3);
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadException))]
        public void ReadInvalidXML()
        {
            StreamReader name = new StreamReader("InvalidXML.xml");
            Spreadsheet s = new Spreadsheet(name, new Regex(@"^.*$"));
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetVersionException))]
        public void InvalidVersionCheck()
        {
            StringWriter sw = new StringWriter();
            using (XmlWriter writer = XmlWriter.Create(sw))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("IsValid", "^.*$");

                writer.WriteStartElement("cell");
                writer.WriteAttributeString("name", "A1");
                writer.WriteAttributeString("contents", "hello");
                writer.WriteEndElement();

                writer.WriteStartElement("cell");
                writer.WriteAttributeString("name", "A2");
                writer.WriteAttributeString("contents", "5.0");
                writer.WriteEndElement();

                writer.WriteStartElement("cell");
                writer.WriteAttributeString("name", "A3");
                writer.WriteAttributeString("contents", "4.0");
                writer.WriteEndElement();

                writer.WriteStartElement("cell");
                writer.WriteAttributeString("name", "A4");
                writer.WriteAttributeString("contents", "= A2 + A3");
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            AbstractSpreadsheet ss = new Spreadsheet(new StringReader(sw.ToString()), new Regex(@"^[b-zB-Z]\d&"));
        }
        [TestMethod]
        public void ReadValidXML()
        {
            StreamReader name = new StreamReader("SaveTest3.xml");
            Spreadsheet s = new Spreadsheet(name, new Regex(@"^.*$"));
        }

        [TestMethod()]
        public void SaveTest3()
        {
            StringWriter sw = new StringWriter();
            using (XmlWriter writer = XmlWriter.Create(sw))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("IsValid", "^.*$");

                writer.WriteStartElement("cell");
                writer.WriteAttributeString("name", "A1");
                writer.WriteAttributeString("contents", "hello");
                writer.WriteEndElement();

                writer.WriteStartElement("cell");
                writer.WriteAttributeString("name", "A2");
                writer.WriteAttributeString("contents", "5.0");
                writer.WriteEndElement();

                writer.WriteStartElement("cell");
                writer.WriteAttributeString("name", "A3");
                writer.WriteAttributeString("contents", "4.0");
                writer.WriteEndElement();

                writer.WriteStartElement("cell");
                writer.WriteAttributeString("name", "A4");
                writer.WriteAttributeString("contents", "= A2 + A3");
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            AbstractSpreadsheet ss = new Spreadsheet(new StringReader(sw.ToString()), new Regex(""));
            StreamWriter path = new StreamWriter("SaveTest3.xml");
            ss.Save(path);
            VV(ss, "A1", "hello", "A2", 5.0, "A3", 4.0, "A4", 9.0);
        }



        [TestMethod()]
        public void SaveTest4()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Set(ss, "A1", "hello");
            Set(ss, "A2", "5.0");
            Set(ss, "A3", "4.0");
            Set(ss, "A4", "= A2 + A3");
            StringWriter sw = new StringWriter();
            ss.Save(sw);
            StreamWriter path = new StreamWriter("SaveTest4.xml");
            ss.Save(path);


            using (XmlReader reader = XmlReader.Create(new StringReader(sw.ToString())))
            {
                int spreadsheetCount = 0;
                int cellCount = 0;

                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "spreadsheet":
                                Assert.IsTrue(new Regex(reader["IsValid"]).IsMatch("a4"));
                                spreadsheetCount++;
                                break;

                            case "cell":
                                string name = reader["name"];
                                string contents = reader["contents"];
                                if (name.Equals("A1")) { Assert.AreEqual("hello", contents); }
                                else if (name.Equals("A2")) { Assert.AreEqual(5.0, Double.Parse(contents), 1e-9); }
                                else if (name.Equals("A3")) { Assert.AreEqual(4.0, Double.Parse(contents), 1e-9); }
                                else if (name.Equals("A4")) { contents = contents.Replace(" ", ""); Assert.AreEqual("=A2+A3", contents); }
                                else Assert.Fail();
                                cellCount++;
                                break;
                        }
                    }
                }
                Assert.AreEqual(1, spreadsheetCount);
                Assert.AreEqual(4, cellCount);
            }
        }
    }
}