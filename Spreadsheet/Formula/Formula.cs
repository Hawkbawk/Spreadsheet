// Skeleton written by Joe Zachary for CS 3500, January 2019

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static Formulas.TokenType;

namespace Formulas
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard
    /// precedence rules.  Provides a means to evaluate Formulas.  Formulas can
    /// be composed of non-negative floating-point numbers, variables, left and
    /// right parentheses, and the four binary operator symbols +, -, *, and /. 
    /// (The unary operators + and - are not allowed.)
    /// </summary>
    public class Formula
    {
        /// <summary>
        /// Creates a Formula from a string that consists of a standard infix
        /// expression composed from non-negative floating-point numbers (using
        /// C#-like syntax for double/int literals), variable symbols (a letter
        /// followed by zero or more letters and/or digits), left and right
        /// parentheses, and the four binary operator symbols +, -, *, and /. 
        /// White space is permitted between tokens, but is not required.
        /// Examples of a valid parameter to this constructor are: "2.5e9 + x5 /
        /// 17" "(5 * 2) + 8" "x*y-2+35/9" Examples of invalid parameters are:
        /// "_" "-5.3" "2 5 + 3" If the formula is syntacticaly invalid, throws
        /// a FormulaFormatException with an explanatory Message.
        /// </summary>

        private IEnumerable<Tuple<string, TokenType>> storedFormula;

        public Formula(String formula)
        {
            storedFormula = GetTokens(formula);
        }
        /// <summary>
        /// Evaluates this Formula, using the Lookup delegate to determine the
        /// values of variables.  (The delegate takes a variable name as a
        /// parameter and returns its value (if it has one) or throws an
        /// UndefinedVariableException (otherwise).  Uses the standard
        /// precedence rules when doing the evaluation. If no undefined
        /// variables or divisions by zero are encountered when evaluating this
        /// Formula, its value is returned.  Otherwise, throws a
        /// FormulaEvaluationException with an explanatory Message.
        /// </summary>
        public double Evaluate(Lookup lookup)
        {
            Stack<double> values = new Stack<double>();
            Stack<string> operators = new Stack<string>();
            foreach (Tuple<string, TokenType> token in storedFormula)
            {
                TokenType tokenType = token.Item2;
                switch (tokenType)
                {
                    case Number:
                        NumberEncountered(values, operators, token);
                        break;
                    case Var:
                        VarEncountered(values, operators, token, lookup);
                        break;
                    case Oper:
                        OperEncountered(values, operators, token);
                        break;
                }
            }
            return finishEvaluation(values, operators);
        }
        /// <summary>
        /// Finishes evaluating the stored formula. This method should only ever
        /// be called after the Evaluate method, otherwise it has undefined
        /// performance.
        /// </summary>
        /// <param name="values">A stack containing either one or two values.
        ///     </param>
        /// <param name="operators">Stack containing 0 or 1 arithmetic
        ///     operators.</param>
        /// <returns>The final result of the stored expression</returns>
        private double finishEvaluation(Stack<double> values, Stack<string> operators)
        {
            if (operators.Count == 0)
            {
                return values.Pop();
            }
            else
            {
                string oper = operators.Pop();
                double rightValue = values.Pop();
                double leftValue = values.Pop();
                if (oper.Equals("+"))
                {
                    return leftValue + rightValue;
                }
                else
                {
                    return leftValue - rightValue;
                }
            }
        }

        private void NumberEncountered(Stack<double> values, Stack<string> operators, Tuple<string, TokenType> token)
        {
            double leftOperand = values.Pop();
            double rightOperand = Convert.ToDouble(token.Item1);
            if (operatorStackCheck(operators).Equals("/"))
            {
                // Remove the division from the operators stack.
                operators.Pop();
                // Check for division by zero. Not allowed!
                if (rightOperand == 0)
                {
                    throw new FormulaEvaluationException("Division by zero isn't allowed!");
                }
                //Evaluate the expression and push it to the values stack.
                double result = values.Pop() / rightOperand;
                values.Push(result);

            }
            else if (operators.Peek().Equals("*"))
            {
                // Remove the multiplication from the operators stack.
                operators.Pop();
                // Evaluate the expression and push it to the values stack.
                double result = leftOperand * rightOperand;
                values.Push(result);
            }
            else
            {
                values.Push(rightOperand);
            }
        }
        private void VarEncountered(Stack<double> values, Stack<string> operators, Tuple<string, TokenType> token, Lookup lookup)
        {
            // Obtain the left and right operands for the expression.
            double leftOperand = values.Pop();
            double rightOperand;
            try
            {
                rightOperand = lookup(token.Item1);
            }
            catch (Exception)
            {
                // Throw an error if the lookup method has no assigned value for that string.
                throw new FormulaEvaluationException("That variable isn't defined!");
            }
            // Checks to see if we are doing division or multiplication and acts accordingly.
            if (operators.Peek().Equals("/"))
            {
                // Division by zero isn't allowed!
                if (rightOperand == 0)
                {
                    throw new FormulaEvaluationException("Division by zero isn't allowed!");
                }
                // Pop the division operator from the operators stack and evaluate the 
                // expression and push the result to the values stack.
                operators.Pop();
                double result = leftOperand / rightOperand;
                values.Push(result);

            }
            else if (operators.Peek().Equals("*"))
            {
                // Pop the division operator from the operators stack and evaluate
                // the expression and push the result to the values stack.               
                operators.Pop();
                double result = leftOperand * rightOperand;
                values.Push(result);
            }
            // If no multiplacation or division is being done, just push the 
            // rightOperand value onto the stack.
            else
            {
                values.Push(rightOperand);
            }
        }

        private void OperEncountered(Stack<double> values, Stack<string> operators, Tuple<string, TokenType> token)
        {
            // Checks to see what operator has been encountered and acts 
            // accordingly.

            switch (token.Item1)
            {
                /* For the addition and subtraction cases, we check to see if previous operands were
                 * being added or subtracted. If so, we perform that operation, and push the result on
                 * to the values stack. Regardless of whether or not we perform some kind of arithmetic,
                 * we push the "+" or "-" operator on to the operators stack.
                 *
                 */
                case "+":
                    string oper = operators.Peek();
                    double rightValue = values.Pop();
                    double leftValue = values.Pop();
                    operators.Pop();
                    if (oper.Equals("+"))
                    {
                        double result = leftValue + rightValue;
                        values.Push(result);
                    }
                    else if (oper.Equals("-"))
                    {
                        double result = leftValue - rightValue;
                        values.Push(result);
                    }
                    operators.Push(token.Item1);
                    break;
                case "-":
                    oper = operators.Peek();
                    rightValue = values.Pop();
                    leftValue = values.Pop();
                    operators.Pop();
                    if (oper.Equals("+"))
                    {
                        double result = leftValue + rightValue;
                        values.Push(result);
                    }
                    else if (oper.Equals("-"))
                    {
                        double result = leftValue - rightValue;
                        values.Push(result);
                    }
                    operators.Push(token.Item1);
                    break;
                case "*":
                    operators.Push(token.Item1);
                    break;
                case "/":
                    operators.Push(token.Item1);
                    break;
                case "(":
                    operators.Push(token.Item1);
                    break;
                case ")":
                    /* Check to see if a plus or minus is at the top of the operator stack.
                     * If one is, pop it off the operators stack, pull the top two values off
                     * the values stack, perform the operation on those two numbers, and then
                     * push that result on to the values stack. Finally, check to see if any 
                     * multiplication or division needs to be done. If it does, perform the same
                     * algorithm specified above one more time, with multiplication or division
                     * instead of addition or subtraction.
                     */
                    oper = operators.Peek();
                    rightValue = values.Pop();
                    leftValue = values.Pop();
                    operators.Pop();
                    if (oper.Equals("+"))
                    {
                        double result = leftValue + rightValue;
                        values.Push(result);
                    }
                    else if (oper.Equals("-"))
                    {
                        double result = leftValue - rightValue;
                        values.Push(result);
                    }
                    operators.Pop();
                    oper = operators.Peek();
                    rightValue = values.Pop();
                    leftValue = values.Pop();
                    if (oper.Equals("*"))
                    {
                        double result = leftValue * rightValue;
                        values.Push(result);
                    }
                    else if (oper.Equals("/"))
                    {
                        double result = leftValue / rightValue;
                        values.Push(result);
                    }
                    break;
            }
        }
        /// <summary>
        /// Checks to see if the operators stack is empty. If it is, returns
        /// null. Otherwise returns the result of operators.Peek().
        /// </summary>
        /// <param name="operators">A stack full of strings representing
        ///     arithmetic operators.</param>
        /// <returns>The result of operators.Peek() if operators isn't empty.
        ///     Otherwise returns an empty string.</returns>
        private string operatorStackCheck(Stack<string> operators)
        {
            if (operators.Count != 0)
            {
                return operators.Peek();
            }
            return "";
        }
        /// <summary>
        /// Checks to see if the operators stack is empty. If it is, returns
        /// null. Otherwise returns the result of values.Peek().
        /// </summary>
        /// <param name="values">A stack containing doubles that represent
        ///     values in an expression.</param>
        /// <returns>Returns the result of values.Peek() if the stack isn't
        ///     empty. Otherwise returns -1.</returns>
        private double valuesStackCheck(Stack<double> values)
        {
            if (values.Count != 0)
            {
                return values.Peek();
            }
            return -1;
        }

        /// <summary>
        /// Given a formula, enumerates the tokens that compose it.  Each token
        /// is described by a Tuple containing the token's text and TokenType. 
        /// There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<Tuple<string, TokenType>> GetTokens(String formula)
        {
            // Patterns for individual tokens.
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z][0-9a-zA-Z]*";

            // NOTE:  I have added white space to this regex to make it more readable.
            // When the regex is used, it is necessary to include a parameter that says
            // embedded white space should be ignored.  See below for an example of this.
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: e[\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall token pattern.  It contains embedded white space that must be ignored when
            // it is used.  See below for an example of this.
            String tokenPattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5}) | (.)",
                                            spacePattern, lpPattern, rpPattern, opPattern, varPattern, doublePattern);

            // Create a Regex for matching tokens.  Notice the second parameter to Split says 
            // to ignore embedded white space in the pattern.
            Regex r = new Regex(tokenPattern, RegexOptions.IgnorePatternWhitespace);

            // Look for the first match
            Match match = r.Match(formula);

            // Start enumerating tokens
            while (match.Success)
            {
                // Ignore spaces
                if (!match.Groups[1].Success)
                {
                    // Holds the token's type
                    TokenType type;

                    if (match.Groups[2].Success)
                    {
                        type = LParen;
                    }
                    else if (match.Groups[3].Success)
                    {
                        type = RParen;
                    }
                    else if (match.Groups[4].Success)
                    {
                        type = Oper;
                    }
                    else if (match.Groups[5].Success)
                    {
                        type = Var;
                    }
                    else if (match.Groups[6].Success)
                    {
                        type = Number;
                    }
                    else if (match.Groups[7].Success)
                    {
                        type = Invalid;
                    }
                    else
                    {
                        // We shouldn't get here
                        throw new InvalidOperationException("Regular exception failed in GetTokens");
                    }

                    // Yield the token
                    yield return new Tuple<string, TokenType>(match.Value, type);
                }

                // Look for the next match
                match = match.NextMatch();
            }
        }
    }

    /// <summary>
    /// Identifies the type of a token.
    /// </summary>
    public enum TokenType
    {
        /// <summary>
        /// Left parenthesis
        /// </summary>
        LParen,

        /// <summary>
        /// Right parenthesis
        /// </summary>
        RParen,

        /// <summary>
        /// Operator symbol
        /// </summary>
        Oper,

        /// <summary>
        /// Variable
        /// </summary>
        Var,

        /// <summary>
        /// Double literal
        /// </summary>
        Number,

        /// <summary>
        /// Invalid token
        /// </summary>
        Invalid
    };

    /// <summary>
    /// A Lookup method is one that maps some strings to double values.  Given a
    /// string, such a function can either return a double (meaning that the
    /// string maps to the double) or throw an UndefinedVariableException
    /// (meaning that the string is unmapped to a value. Exactly how a Lookup
    /// method decides which strings map to doubles and which don't is up to the
    /// implementation of the method.
    /// </summary>
    public delegate double Lookup(string var);

    /// <summary>
    /// Used to report that a Lookup delegate is unable to determine the value
    /// of a variable.
    /// </summary>
    [Serializable]
    public class UndefinedVariableException : Exception
    {
        /// <summary>
        /// Constructs an UndefinedVariableException containing whose message is
        /// the undefined variable.
        /// </summary>
        /// <param name="variable"></param>
        public UndefinedVariableException(String variable)
            : base(variable)
        {
        }
    }

    /// <summary>
    /// Used to report syntactic errors in the parameter to the Formula
    /// constructor.
    /// </summary>
    [Serializable]
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory
        /// message.
        /// </summary>
        public FormulaFormatException(String message) : base(message)
        {
        }
    }

    /// <summary>
    /// Used to report errors that occur when evaluating a Formula.
    /// </summary>
    [Serializable]
    public class FormulaEvaluationException : Exception
    {
        /// <summary>
        /// Constructs a FormulaEvaluationException containing the explanatory
        /// message.
        /// </summary>
        public FormulaEvaluationException(String message) : base(message)
        {
        }
    }
}
