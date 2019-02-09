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
        /// Initializes a new instance of the Formula class. Upon construction,
		/// checks to see if the passed in formula is arithmetically correct,
		/// and throws the appropriate error if it isn't.
        /// </summary>
        /// <param name="formula">A string representation of a mathematical formula./></param>
        public Formula(string formula) : this(formula, s => s, s => true)
        {
            // Keeps track of how many opening and closing parentheses are in
            // the formula, as well as the number of tokens in the formula, all
            // for error checking. The number of opening and closing parenthesis
            // must match in the end, and the number of closing parenthesis
            // should never exceed the number of closing parenthesis.
            int rParenCount = 0;
            int lParenCount = 0;
            tokenCount = 0;

            // Keeps track of the previous token encountered. Used to check if
            // the formula ends with the appropriate type of token.
            Token previousToken = new Token("", Invalid);

            foreach (Token currentToken in GetTokens(formula))
            {
                // The formula should only begin with a number, variable, or
                // opening parenthesis.
                if (tokenCount == 0)
                {
                    switch (currentToken.Type)
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
                // The current token should be valid, and the number of closing
                // parenthesis should never exceed the number of opening
                // parenthesis.
                switch (currentToken.Type)
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
                tokenOrderChecker(previousToken, currentToken);
                previousToken = currentToken;
                tokenCount++;
            }
            endFormulaErrorCheck(lParenCount, rParenCount, previousToken);
            // Once all error checking is done, we can finally store the
            // formula.
            storedFormula = GetTokens(formula);
        }

        public Formula(string formula, Normalizer n, Validator v)
        {
            // After checking for errors in the base formula, we also need to check for errors with the Normalizer and
            foreach (Token token in GetTokens(formula))
            {
                string currentVar = "";
                if (token.Type == Var)
                {
                    currentVar = n(token.Text);
                }
                string pattern = @"^([a-zA-Z]\w*)$";
                Regex r = new Regex(pattern);
                if (!r.IsMatch(currentVar))
                {
                    throw new FormulaFormatException("Your normalizer creates invalid variables!");
                }
            }
        }

        /// <summary>
        /// Defines the number of tokens in the current formula.
        /// </summary>
        private int tokenCount;

        /// <summary>
        /// Defines the add operator as a string.
        /// </summary>
        private const string add = "+";

        /// <summary>
        /// Stores the formula in an IEnumerable structure.
        /// </summary>
        private IEnumerable<Token> storedFormula;

        /// <summary>
        /// Defines the subtract operator as a string.
        /// </summary>
        private const string subtract = "-";

        /// <summary>
        /// Defines the multiply operator as a string.
        /// </summary>
        private const string multiply = "*";

        /// <summary>
        /// Defines the divide operator as a string.
        /// </summary>
        private const string divide = "/";

        /// <summary>
        /// Defines an open parenthesis as a string.
        /// </summary>
        private const string openParen = "(";

        /// <summary>
        /// Defines a close parenthesis as a string.
        /// </summary>
        private const string closeParen = ")";

        /// <summary>
        /// Checks to see if the end of the formula passes certain conditions.
		/// Those conditions are that the number of open and close parenthesis
		/// must match, the last token in the formula has to be a number,
		/// variable, or a close parenthesis and the formula can't be empty.
        /// </summary>
        /// <param name="lParenCount">
        /// The number of opening parenthesis in the formula.</param>
        /// <param name="rParenCount">
        /// The number of closing parenthesis in the formula.</param>
        /// <param name="lastToken">
        /// The last token in the formula.</param>
        private void endFormulaErrorCheck(int lParenCount, int rParenCount, Token lastToken)
        {
            // The formula can't be empty.
            if (tokenCount == 0)
            {
                throw new FormulaFormatException("Please provide a formula, not just blank space.");
            }

            // The number of opening and closing parenthesis have to match.
            if (lParenCount != rParenCount)
            {
                throw new FormulaFormatException("The number of opening and closing parentheses doesn't match!");
            }
            // The last token in a formula has to be a closing parenthesis, a
            // number, or a variable.
            switch (lastToken.Type)
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
        /// Checks to see if the tokens in the formula are in an acceptable
		/// order. That is, ensures that the tokens in a formula are organized
		/// in such a way that the formula is able to be arithmetically solved.
        /// </summary>
        /// <param name="previousToken">
        /// The token immediately preceding the current token.</param>
        /// <param name="currentToken">
        /// The current token.</param>
        private void tokenOrderChecker(Token previousToken, Token currentToken)
        {
            // Only specific token types can follow other token types.
            switch (previousToken.Type)
            {
                // Only operators and closing parenthesis can follow a number.
                case Number:
                    switch (currentToken.Type)
                    {
                        case Oper:
                            break;

                        case RParen:
                            break;

                        default:
                            throw new FormulaFormatException("Your formula isn't in a valid format!");
                    }
                    break;
                // Only operators and closing parenthesis can follow a variable.
                case Var:
                    switch (currentToken.Type)
                    {
                        case Oper:
                            break;

                        case RParen:
                            break;

                        default:
                            throw new FormulaFormatException("Your formula isn't in a valid format!");
                    }
                    break;
                // Only numbers, variables, and closing parenthesis can follow
                // opening parenthesis.
                case LParen:
                    switch (currentToken.Type)
                    {
                        case Number:
                            break;

                        case Var:
                            break;

                        case LParen:
                            break;

                        default:
                            throw new FormulaFormatException("Your formula isn't in a valid format!");
                    }
                    break;
                // Operators can only be followed by numbers, variable, and
                // opening parenthesis.
                case Oper:
                    switch (currentToken.Type)
                    {
                        case Number:
                            break;

                        case Var:
                            break;

                        case LParen:
                            break;

                        default:
                            throw new FormulaFormatException("Your formula isn't in a valid format!");
                    }
                    break;
                // Closing parenthesis can only be followed by operators or
                // another closing parenthesis.
                case RParen:
                    switch (currentToken.Type)
                    {
                        case Oper:
                            break;

                        case RParen:
                            break;

                        default:
                            throw new FormulaFormatException("Your formula isn't in a valid format!");
                    }
                    break;
            }
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
        /// <param name="lookup">
        /// A delegate that maps some string value to a double value.</param>
        /// <returns>
        /// The result of evaluating the formula.</returns>
        public double Evaluate(Lookup lookup)
        {
            Stack<double> values = new Stack<double>();
            Stack<string> operators = new Stack<string>();
            foreach (Token token in storedFormula)
            {
                switch (token.Type)
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
        /// <param name="values">The last value or values left to be used.</param>
        /// <param name="operators">Either the last operator to be used on the passed in values, or is empty.</param>
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
        /// Deals with evaluating the expression in the case that the current
		/// token is a number.
        /// </summary>
        /// <param name="values">A Stack containing the values encountered so far.</param>
        /// <param name="operators">The operators encountered so far.</param>
        /// <param name="token">The current token.</param>
        private void NumberEncountered(Stack<double> values, Stack<string> operators, Token token)
        {
            double result = Convert.ToDouble(token.Text);
            string oper = "";
            // Check what operator we're working with, if any.
            if (!isEmpty(operators))
            {
                oper = operators.Peek();
            }
            // Either multiply the result of values.Pop() with result, or divide
            // it by result.
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

                if (result == 0)
                {
                    throw new FormulaEvaluationException("Division by zero isn't allowed!");
                }

                result = leftOperand / result;
            }
            // Regardless of whether we've done any math, push the result on to
            // the stack.
            values.Push(result);
        }

        /// <summary>
        /// Deals with evaluating the expression in the case that the current
		/// token is a variable.
        /// </summary>
        /// <param name="values">A Stack containing the values encountered so far.</param>
        /// <param name="operators">The operators encountered so far.</param>
        /// <param name="token">The current token.</param>
        private void VarEncountered(Stack<double> values, Stack<string> operators, Token token, Lookup lookup)
        {
            double result = 0;
            try
            {
                result = lookup(token.Text);
            }
            catch (UndefinedVariableException)
            {
                throw new FormulaEvaluationException("Your formula contains undefined variables!");
            }
            string oper = "";
            // Check what operator we're working with, if any.
            if (!isEmpty(operators))
            {
                oper = operators.Peek();
            }
            // Either multiply the result of values.Pop() with result, or divide
            // it by result.
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
            // Regardless of whether we've done any math, push the result on to
            // the stack.
            values.Push(result);
        }

        /// <summary>
        /// Deals with evaluating the expression in the case that the current
		/// token is an arithmetic operator.
        /// </summary>
        /// <param name="values">A Stack containing the values encountered so far.</param>
        /// <param name="operators">The operators encountered so far.</param>
        /// <param name="token">The current token.</param>
        private void OperEncountered(Stack<double> values, Stack<string> operators, Token token)
        {
            switch (token.Text)
            {
                case "+":
                    addOrSubtractOperands(values, operators);
                    operators.Push(token.Text);
                    break;

                case "-":
                    addOrSubtractOperands(values, operators);
                    operators.Push(token.Text);
                    break;

                case "*":
                    operators.Push(token.Text);
                    break;

                case "/":
                    operators.Push(token.Text);
                    break;
            }
        }

        /// <summary>
        /// Only called if a opening parenthesis is encountered. Simply pushes
		/// an opening parenthesis on to the stack.
        /// </summary>
        /// <param name="values">A Stack containing the values encountered so far.</param>
        /// <param name="operators">The operators encountered so far.</param>
        /// <param name="token">The current token.</param>
        private void LParenEncountered(Stack<double> values, Stack<string> operators, Token token)
        {
            operators.Push(token.Text);
        }

        /// <summary>
        /// Only called if a closing parenthesis is encountered. Performs the
		/// appropriate arithmetic depending on what is on the values and
		/// operators stacks.
        /// </summary>
        /// <param name="values">A Stack containing the values encountered so far.</param>
        /// <param name="operators">The operators encountered so far.</param>
        /// <param name="token">The current token.</param>
        private void RParenEncountered(Stack<double> values, Stack<string> operators, Token token)
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
                if (rightOperand == 0)
                {
                    throw new FormulaEvaluationException("Division by zero isn't allowed!");
                }
                values.Push(leftOperand / rightOperand);
            }
        }

        /// <summary>
        /// Checks to see what operator is currently on top of the operators
		/// stack. If a "+" is at the top, calls the addOperands method. If a
		/// "-" is at the top, calls the subtractOperands method. Otherwise, no
		/// methods are called.
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
        /// Pops the subtract operator from the operators stack, subtracts the
		/// top two values in the values stack by popping them, and pushes the
		/// result to the values stack.
        /// </summary>
        /// <param name="values">
        /// The values encountered so far.</param>
        /// <param name="operators">
        /// The operators encountered so far.</param>
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
        /// Pops the addition operator from the operators stack, adds together
		/// the top two values in the values stack by popping them, and pushes
		/// that result to the values stack.
        /// </summary>
        /// <param name="values">
        /// The values encountered so far.</param>
        /// <param name="operators">
        /// The operators encountered so far.</param>
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
        private static IEnumerable<Token> GetTokens(String formula)
        {
            // Patterns for individual tokens.
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z][0-9a-zA-Z]*";

            // NOTE:  I have added white space to this regex to make it more readable.
            // When the regex is used, it is necessary to include a parameter
            // that says
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
                    yield return new Token(match.Value, type);
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

    public delegate string Normalizer(string s);

    public delegate bool Validator(string s);

    public struct Token
    {
        public string Text { get; private set; }
        public TokenType Type { get; private set; }

        public Token(string _text, TokenType _type)
        {
            Text = _text;
            Type = _type;
        }
    }

    /// <summary>
    /// Used to report that a Lookup delegate is unable to determine the value
    /// of a variable.
    /// </summary>
    [Serializable]
    public class UndefinedVariableException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see
		/// cref="UndefinedVariableException"/> class.
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
        /// Initializes a new instance of the <see
		/// cref="FormulaFormatException"/> class.
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
        /// Initializes a new instance of the <see
		/// cref="FormulaEvaluationException"/> class.
        /// </summary>
        /// <param name="message">The message<see cref="String"/></param>
        public FormulaEvaluationException(String message) : base(message)
        {
        }
    }
}