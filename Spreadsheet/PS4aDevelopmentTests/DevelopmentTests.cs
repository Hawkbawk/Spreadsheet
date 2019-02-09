using Formulas;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace PS4aDevelopmentTests
{
    [TestClass]
    public class DevelopmentTests
    {
        [TestMethod]
        public void GetVars3()
        {
            Formula f = new Formula("a * b - c + d / e * 2.5e6");
            var expected = new HashSet<string>();
            expected.Add("a");
            expected.Add("b");
            expected.Add("c");
            expected.Add("d");
            expected.Add("e");
            var actual = f.GetVariables();
            Assert.IsTrue(expected.SetEquals(actual));
        }

        [TestMethod]
        public void GetVars4()
        {
            Formula f = new Formula("a + b - c * d", s => "x", s => true);
            HashSet<string> expected = new HashSet<string>();
            expected.Add("x");
            var actual = f.GetVariables();
            Assert.IsTrue(expected.SetEquals(actual));
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ThreeArg4()
        {
            Formula f = new Formula("x+y", s => s == "x" ? "z" : s, s => s != "z");
        }

        [TestMethod]
        public void ThreeArg7()
        {
            Formula f = new Formula("y", s => "x", s => true);
            Assert.AreEqual(1.0, f.Evaluate(s => (s == "x") ? 1 : 0), 1e-6);
        }

        [TestMethod]
        public void ToString1()
        {
            Formula f1 = new Formula("a - b + c - y * 72", s => s.ToUpper(), s => true);
            Formula f2 = new Formula(f1.ToString());
            Assert.AreEqual(f1.ToString(), f2.ToString());
        }

        [TestMethod]
        public void ToString4()
        {
            Formula f1 = new Formula("a+b*(c-15)/2");
            Formula f2 = new Formula(f1.ToString());
            Assert.AreEqual(24.0, f2.Evaluate(s => char.IsLower(s[0]) ? 16 : 0), 1e-6);
        }
    }
}