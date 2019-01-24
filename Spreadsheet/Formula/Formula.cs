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
        /// Initializes a new instance of the <see cref="Formula"/> class.
        /// </summary>
        /// <param name="formula">The formula<see cref="String"/></param>
        public Formula(String formula)
        {
            // Keeps track of how many opening and closing parentheses are in 
            // the formula, as well as the number of tokens in the formula, 
            // all for error checking. The number of opening and closing 
            // parenthesis must match in the end, and the number of closing
            // parenthesis should never exceed the number of closing
            // parenthesis.
            double rParenCount = 0;
            double lParenCount = 0;
            tokenCount = 0;

            // Keeps track of the previous token encountered. Used to check if
            // the formula ends with the appropriate type of token.
            Tuple<string, TokenType> previousToken = new Tuple<string, TokenType>("", Invalid);

            foreach (Tuple<string, TokenType> token in GetTokens(formula))
            {
                // The formula should only begin with a number, variable,
                // or opening parenthesis.
                if (tokenCount == 0)
                {
                    switch (token.Item2)
                    {
                        case Number:
                            break;
                        case Var:
                            break;
                        case LParen:
                            break;
                        default:
                            throw new FormulaFormatException("Your formula " +
                                "must begin with an opening parenthesis, variable, or number!");
                    }
                }
                switch (token.Item2)
                {
                    case Invalid:
                        throw new FormulaFormatException("There are invalid characters in your formula!");
                    case LParen:
                        lParenCount++;
                        break;
                    case RParen:
                        rParenCount++;
                        if (rParenCount > lParenCount)
                        {
                            throw new FormulaFormatException("Your parentheses don't match!");
                        }
                        break;
                }
                previousToken = token;
                tokenCount++;
            }

            if (tokenCount == 0)
            {
                throw new FormulaFormatException("Please provide a formula, not just blank space.");
            }
            if (rParenCount != lParenCount)
            {
                throw new FormulaFormatException("The number of opening and closing parentheses doesn't match!");
            }
            switch (previousToken.Item2)
            {
                case RParen:
                    break;
                case Number:
                    break;
                case Var:
                    break;
                default:
                    throw new FormulaFormatException("Your formula must end" +
                        "with a closing parenthesis, number, or variable!");
            }
        }

        /// <summary>
        /// Defines the tokenCount
        /// </summary>
        private int tokenCount;

        /// <summary>
        /// Defines the add
        /// </summary>
        private const string add = "+";

        /// <summary>
        /// Defines the storedFormula
        /// </summary>
        private IEnumerable<Tuple<string, TokenType>> storedFormula;

        /// <summary>
        /// Defines the subtract
        /// </summary>
        private const string subtract = "-";

        /// <summary>
        /// Defines the multiply
        /// </summary>
        private const string multiply = "*";

        /// <summary>
        /// Defines the divide
        /// </summary>
        private const string divide = "/";

        /// <summary>
        /// Defines the openParen
        /// </summary>
        private const string openParen = "(";

        /// <summary>
        /// Defines the closeParen
        /// </summary>
        private const string closeParen = ")";

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
        /// <param name="lookup">The lookup<see cref="Lookup"/></param>
        /// <returns>The <see cref="double"/></returns>
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
                    case LParen:
                        LParenEncountered(values, operators, token);
                        break;
                    case RParen:
                        RParenEncountered(values, operators, token);
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
        /// <param name="values">The values<see cref="Stack{double}"/></param>
        /// <param name="operators">The operators<see cref="Stack{string}"/></param>
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

        /// <summary>
        /// The NumberEncountered
        /// </summary>
        /// <param name="values">The values<see cref="Stack{double}"/></param>
        /// <param name="operators">The operators<see cref="Stack{string}"/></param>
        /// <param name="token">The token<see cref="Tuple{string, TokenType}"/></param>
        private void NumberEncountered(Stack<double> values, Stack<string> operators, Tuple<string, TokenType> token)
        {
            double result = Convert.ToDouble(token.Item1);
            string oper = "";
            // Check what operator we're working with, if any.
            if (!isEmpty(operators))
            {
                oper = operators.Peek();
            }
            // Either multiply the result of values.Pop() with result, or divide it by result.
            if (oper.Equals(multiply))
            {
                double leftOperand = -1;
                if (!isEmpty(values))
                {
                    leftOperand = values.Pop();
                }
                // Get rid of the operator, cause we're using it.
                operators.Pop();

                result = leftOperand * result;
            }
            else if (oper.Equals(divide))
            {
                double leftOperand = -1;
                if (!isEmpty(values))
                {
                    leftOperand = values.Pop();
                }
                // Get rid of the operator, cause we're using it.
                operators.Pop();

                result = leftOperand / result;
            }
            // Regardless of whether we've done any math, push the result on to the stack.
            values.Push(result);
        }

        /// <summary>
        /// The VarEncountered
        /// </summary>
        /// <param name="values">The values<see cref="Stack{double}"/></param>
        /// <param name="operators">The operators<see cref="Stack{string}"/></param>
        /// <param name="token">The token<see cref="Tuple{string, TokenType}"/></param>
        /// <param name="lookup">The lookup<see cref="Lookup"/></param>
        private void VarEncountered(Stack<double> values, Stack<string> operators, Tuple<string, TokenType> token, Lookup lookup)
        {
            double result = lookup(token.Item1);
            string oper = "";
            // Check what operator we're working with, if any.
            if (!isEmpty(operators))
            {
                oper = operators.Peek();
            }
            // Either multiply the result of values.Pop() with result, or divide it by result.
            if (oper.Equals(multiply))
            {
                double leftOperand = -1;
                if (!isEmpty(values))
                {
                    leftOperand = values.Pop();
                }
                // Get rid of the operator, cause we're using it.
                operators.Pop();

                result = leftOperand * result;
            }
            else if (oper.Equals(divide))
            {
                double leftOperand = -1;
                if (!isEmpty(values))
                {
                    leftOperand = values.Pop();
                }
                // Get rid of the operator, cause we're using it.
                operators.Pop();

                result = leftOperand / result;
            }
            // Regardless of whether we've done any math, push the result on to the stack.
            values.Push(result);
        }

        /// <summary>
        /// The OperEncountered
        /// </summary>
        /// <param name="values">The values<see cref="Stack{double}"/></param>
        /// <param name="operators">The operators<see cref="Stack{string}"/></param>
        /// <param name="token">The token<see cref="Tuple{string, TokenType}"/></param>
        private void OperEncountered(Stack<double> values, Stack<string> operators, Tuple<string, TokenType> token)
        {

            switch (token.Item1)
            {
                case "+":
                    addOrSubtractOperands(values, operators);
                    operators.Push(token.Item1);
                    break;
                case "-":
                    addOrSubtractOperands(values, operators);
                    operators.Push(token.Item1);
                    break;
                case "*":
                    operators.Push(token.Item1);
                    break;
                case "/":
                    operators.Push(token.Item1);
                    break;
            }
        }

        /// <summary>
        /// The LParenEncountered
        /// </summary>
        /// <param name="values">The values<see cref="Stack{double}"/></param>
        /// <param name="operators">The operators<see cref="Stack{string}"/></param>
        /// <param name="token">The token<see cref="Tuple{string, TokenType}"/></param>
        private void LParenEncountered(Stack<double> values, Stack<string> operators, Tuple<string, TokenType> token)
        {
            operators.Push(token.Item1);
        }

        /// <summary>
        /// The RParenEncountered
        /// </summary>
        /// <param name="values">The values<see cref="Stack{double}"/></param>
        /// <param name="operators">The operators<see cref="Stack{string}"/></param>
        /// <param name="token">The token<see cref="Tuple{string, TokenType}"/></param>
        private void RParenEncountered(Stack<double> values, Stack<string> operators, Tuple<string, TokenType> token)
        {
            addOrSubtractOperands(values, operators);
            operators.Pop();
            string oper = "";
            if (!isEmpty(operators))
            {
                oper = operators.Peek();
            }
            if (oper.Equals(multiply))
            {
                double rightOperand = -1;
                double leftOperand = -1;
                if (!isEmpty(values))
                {
                    rightOperand = values.Pop();
                }
                if (!isEmpty(values))
                {
                    leftOperand = values.Pop();
                }
                operators.Pop();
                values.Push(leftOperand * rightOperand);
            }
            else if (oper.Equals(divide))
            {
                double rightOperand = -1;
                double leftOperand = -1;
                if (!isEmpty(values))
                {
                    rightOperand = values.Pop();
                }
                if (!isEmpty(values))
                {
                    leftOperand = values.Pop();
                }
                operators.Pop();
                values.Push(leftOperand / rightOperand);
            }
        }

        /// <summary>
        /// Checks to see what operator is currently on top of the operators stack. If a "+" is at the top, calls the addOperands method. If a "-" is at the top, calls the subtractOperands method. Otherwise, no methods are called.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="operators"></param>
        private void addOrSubtractOperands(Stack<double> values, Stack<string> operators)
        {
            string oper = "";
            if (!isEmpty(operators))
            {
                oper = operators.Peek();
            }
            if (oper.Equals(add))
            {
                addOperands(values, operators);
            }
            else if (oper.Equals(subtract))
            {
                subtractOperands(values, operators);
            }
        }

        /// <summary>
        /// The subtractOperands
        /// </summary>
        /// <param name="values"></param>
        /// <param name="operators"></param>
        private void subtractOperands(Stack<double> values, Stack<string> operators)
        {
            double result;
            double rightOperand = -1;
            double leftOperand = -1;
            if (!isEmpty(values))
            {
                rightOperand = values.Pop();
            }
            if (!isEmpty(values))
            {
                leftOperand = values.Pop();
            }
            operators.Pop();
            result = leftOperand - rightOperand;
            values.Push(result);
        }

        /// <summary>
        /// The addOperands
        /// </summary>
        /// <param name="values"></param>
        /// <param name="operators"></param>
        private void addOperands(Stack<double> values, Stack<string> operators)
        {
            double result;
            double rightOperand = -1;
            double leftOperand = -1;
            if (!isEmpty(values))
            {
                rightOperand = values.Pop();
            }
            if (!isEmpty(values))
            {
                leftOperand = values.Pop();
            }
            operators.Pop();
            result = leftOperand + rightOperand;
            values.Push(result);
        }

        /// <summary>
        /// Tells whether or not the given stack is empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stack">The stack<see cref="Stack{T}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private bool isEmpty<T>(Stack<T> stack)
        {
            return stack.Count == 0;
        }

        /// <summary>
        /// Given a formula, enumerates the tokens that compose it.  Each token
        /// is described by a Tuple containing the token's text and TokenType. 
        /// There are no empty tokens, and no token contains white space.
        /// </summary>
        /// <param name="formula">The formula<see cref="String"/></param>
        /// <returns>The <see cref="IEnumerable{Tuple{string, TokenType}}"/></returns>
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
        /// Initializes a new instance of the <see cref="UndefinedVariableException"/> class.
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
        /// Initializes a new instance of the <see cref="FormulaFormatException"/> class.
        /// </summary>
        /// <param name="message">The message<see cref="String"/></param>
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
        /// Initializes a new instance of the <see cref="FormulaEvaluationException"/> class.
        /// </summary>
        /// <param name="message">The message<see cref="String"/></param>
        public FormulaEvaluationException(String message) : base(message)
        {
        }
    }
}
