using Formulas;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace PS4aDevelopmentTests
{
    [TestClass]
    public class DevelopmentTests
    {
        /// <summary>
        /// Asserts that you can't pass any null parameters to the new
        /// Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void FirstNullParameter()
        {
            Formula f = new Formula(null, s => s, s => true);
        }

        /// <summary>
        /// Asserts that we are obtaining the right variables when accessing the
        /// GetVariables method.
        /// </summary>
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

        /// <summary>
        /// Asserts that the set aspect of the GetVariables is working correctly.
        /// </summary>
        [TestMethod]
        public void GetVars4()
        {
            Formula f = new Formula("a + b - c * d", s => "x", s => true);
            HashSet<string> expected = new HashSet<string>();
            expected.Add("x");
            var actual = f.GetVariables();
            Assert.IsTrue(expected.SetEquals(actual));
        }

        /// <summary>
        /// Asserts that the formula doesn't allow for invalid formats from the
        /// normalizer.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void InvalidNormalizer()
        {
            Formula f = new Formula("x + y", s => "x67^", s => true);
        }

        /// <summary>
        /// Asserts that a null lookup method throws the right exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void NullLookup()
        {
            Formula f = new Formula("x + y + 123663377");
            f.Evaluate(null);
        }

        /// <summary>
        /// Asserts that you can't pass any null parameters to the new
        /// Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void SecondNullParameter()
        {
            Formula f = new Formula("x + y", null, s => true);
        }

        /// <summary>
        /// Asserts that you can't pass any null parameters to the new
        /// Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void ThirdNullParameter()
        {
            Formula f = new Formula("x + y", s => s, null);
        }

        /// <summary>
        /// Checks to see if the normalizer is actually being used.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ThreeArg4()
        {
            Formula f = new Formula("x+y", s => s == "x" ? "z" : s, s => s != "z");
            Assert.AreEqual("z+y", f.ToString());
        }

        /// <summary>
        /// Asserts that the formula is replacing the variables according to the
        /// normalizer.
        /// </summary>
        [TestMethod]
        public void ThreeArg7()
        {
            Formula f = new Formula("y", s => "x", s => true);
            Assert.AreEqual(1.0, f.Evaluate(s => (s == "x") ? 1 : 0), 1e-6);
        }

        /// <summary>
        /// Checks to see that the ToString method is performing as expected.
        /// </summary>
        [TestMethod]
        public void ToString1()
        {
            Formula f1 = new Formula("a - b + c - y * 72", s => s.ToUpper(), s => true);
            Formula f2 = new Formula(f1.ToString());
            Assert.AreEqual(f1.ToString(), f2.ToString());
        }

        /// <summary>
        /// Asserts that the ToString method is allow for proper evaluation.
        /// </summary>
        [TestMethod]
        public void ToString4()
        {
            Formula f1 = new Formula("a+b*(c-15)/2");
            Formula f2 = new Formula(f1.ToString());
            Assert.AreEqual(24.0, f2.Evaluate(s => char.IsLower(s[0]) ? 16 : 0), 1e-6);
        }
    }
}