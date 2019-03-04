using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfilingCode
{
    class ProfilingCode
    {
        static void Main(string[] args)
        {
            LongTest();
        }

        private static void LongTest()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("SUM1", "= A1 + A2");
            int i;
            int depth = 25;
            for (i = 1; i <= depth * 2; i += 2)
            {
                s.SetContentsOfCell("A" + i, "= A" + (i + 2) + " + A" + (i + 3));
                s.SetContentsOfCell("A" + (i + 1), "= A" + (i + 2) + "+ A" + (i + 3));
            }
            TextWriter dest = new StreamWriter("LongText.xml");
            s.SetContentsOfCell("A" + i, "1");
            s.SetContentsOfCell("A" + (i + 1), "1");
            s.Save(dest);
            Assert.AreEqual(Math.Pow(2, depth + 1), (double)s.GetCellValue("SUM1"), 1e20);
            s.SetContentsOfCell("A" + i, "0");
            Assert.AreEqual(Math.Pow(2, depth), (double)s.GetCellValue("SUM1"), 1e20);
            s.SetContentsOfCell("A" + (i + 1), "0");
            Assert.AreEqual(0.0, (double)s.GetCellValue("SUM1"), 0.1);
        }
    }
}
