// Written by Joe Zachary for CS 3500, January 2017.

using Formulas;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace FormulaTestCases
{
    /// <summary>
    /// These test cases are in no sense comprehensive!  They are intended to show you how
    /// client code can make use of the Formula class, and to show you how to
	/// create your
    /// own (which we strongly recommend).  To run them, pull down the Test menu and do
    /// Run > All Tests.
    /// </summary>
    [TestClass]
    public class UnitTests
    {
        /// <summary>
        /// This tests that a syntactically incorrect parameter to Formula
        /// results in a FormulaFormatException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct1()
        {
            Formula f = new Formula("_");
        }

        /// <summary>
        /// Ensures that variables can only be followed by operators or a
        /// closing parenthesis.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct10()
        {
            Formula f = new Formula("x 9");
        }

        /// <summary>
        /// This is another syntax error
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct2()
        {
            Formula f = new Formula("2++3");
        }

        /// <summary>
        /// Another syntax error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct3()
        {
            Formula f = new Formula("2 3");
        }

        /// <summary>
        /// Checks to see if the constructor throws an error if too many closing
        /// parenthesis are found.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct4()
        {
            Formula f = new Formula("((3 + 9)))");
        }

        /// <summary>
        /// Checks to see if the constructor throws an error if only parenthesis
        /// are in the formula.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct5()
        {
            Formula f = new Formula("(())");
        }

        /// <summary>
        /// Checks to see if the constructor throws an error if not enough
        /// closing parenthesis are found.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct6()
        {
            Formula f = new Formula("( x +  z ");
        }

        /// <summary>
        /// Ensures that the constructors throws an exception if the formula is
        /// empty.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct7()
        {
            Formula f = new Formula("");
        }

        /// <summary>
        /// Ensures that the constructor doesn't allow invalid tokens through.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct8()
        {
            Formula f = new Formula("17 _ 19");
        }

        /// <summary>
        /// Ensures that the constructor doesn't let the formula end with an
        /// invalid token.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct9()
        {
            Formula f = new Formula("(");
        }

        /// <summary>
        /// Makes sure that "2+3" evaluates to 5.  Since the Formula
        /// contains no variables, the delegate passed in as the
        /// parameter doesn't matter.  We are passing in one that
        /// maps all variables to zero.
        /// </summary>
        [TestMethod]
        public void Evaluate1()
        {
            Formula f = new Formula("2+3");
            Assert.AreEqual(5.0, f.Evaluate(v => 0), 1e-6);
        }

        /// <summary>
        /// Ensures that the Evaluate method divides after reaching a closing
        /// parenthesis.
        /// </summary>
        [TestMethod]
        public void Evaluate10()
        {
            Formula f = new Formula("(3 + 7) / (4 + 6)");
            Assert.AreEqual(1, f.Evaluate(v => 0));
        }

        /// <summary>
        /// Ensures that the Evaluate method can handle multiplication of two
        /// adjacent variables.
        /// </summary>
        [TestMethod]
        public void Evaluate11()
        {
            Formula f = new Formula(" x * y");
            Assert.AreEqual(24, f.Evaluate(Lookup4));
        }

        /// <summary>
        /// The big daddy of a formula. A stress test for the Evaluate method.
        /// </summary>
        [TestMethod]
        public void Evaluate12()
        {
            StringBuilder sb = new StringBuilder("", 20_000);
            for (int i = 1; i <= 9_999; i++)
            {
                sb.Append(i + "+");
            }
            String s = sb.ToString() + 10_000;
            Formula f = new Formula(s);
            Assert.AreEqual(50_005_000, f.Evaluate(Lookup4));
        }

        /// <summary>
        /// The Formula consists of a single variable (x5).  The value of
        /// the Formula depends on the value of x5, which is determined by
        /// the delegate passed to Evaluate.  Since this delegate maps all
        /// variables to 22.5, the return value should be 22.5.
        /// </summary>
        [TestMethod]
        public void Evaluate2()
        {
            Formula f = new Formula("x5");
            Assert.AreEqual(f.Evaluate(v => 22.5), 22.5, 1e-6);
        }

        /// <summary>
        /// Here, the delegate passed to Evaluate always throws a
        /// UndefinedVariableException (meaning that no variables have
        /// values).  The test case checks that the result of
        /// evaluating the Formula is a FormulaEvaluationException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Evaluate3()
        {
            Formula f = new Formula("x + y");
            f.Evaluate(v => { throw new UndefinedVariableException(v); });
        }

        /// <summary>
        /// The delegate passed to Evaluate is defined below.  We check
        /// that evaluating the formula returns in 10.
        /// </summary>
        [TestMethod]
        public void Evaluate4()
        {
            Formula f = new Formula("x + y");
            Assert.AreEqual(f.Evaluate(Lookup4), 10.0, 1e-6);
        }

        /// <summary>
        /// This uses one of each kind of token.
        /// </summary>
        [TestMethod]
        public void Evaluate5()
        {
            Formula f = new Formula("(x + y) * (z / x) * 1.0");
            Assert.AreEqual(f.Evaluate(Lookup4), 20.0, 1e-6);
        }

        /// <summary>
        /// Ensures that the class can handle just a single number as a formula.
        /// </summary>
        [TestMethod]
        public void Evaluate6()
        {
            Formula f = new Formula("((7))");
            Assert.AreEqual(f.Evaluate(Lookup4), 7, 1e-6);
        }

        /// <summary>
        /// Ensures that the Evaluate method doesn't try to divide by zero.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Evaluate7()
        {
            Formula f = new Formula("17 + 16 / 0");
            f.Evaluate(Lookup4);
        }

        /// <summary>
        /// Ensures that the Evaluate method can handle division.
        /// </summary>
        [TestMethod]
        public void Evaluate8()
        {
            Formula f = new Formula("18 / 6");
            Assert.AreEqual(3, f.Evaluate(Lookup4));
        }

        /// <summary>
        /// Ensures that the Evaluate method can handle repeated subtraction.
        /// </summary>
        [TestMethod]
        public void Evaluate9()
        {
            Formula f = new Formula("(17 - 4 - 3)");
            Assert.AreEqual(10, f.Evaluate(Lookup4));
        }

        /// <summary>
        /// A Lookup method that maps x to 4.0, y to 6.0, and z to 8.0. All
        /// other variables result in an UndefinedVariableException.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public double Lookup4(String v)
        {
            switch (v)
            {
                case "x": return 4.0;
                case "y": return 6.0;
                case "z": return 8.0;
                default: throw new UndefinedVariableException(v);
            }
        }
    }
}