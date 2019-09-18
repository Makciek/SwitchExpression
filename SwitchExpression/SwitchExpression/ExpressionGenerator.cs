using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SwitchExpression
{
    /// <summary>
    /// Helps generating expression that execute as switch 
    /// </summary>
    /// <typeparam name="TestT">The type of switch test value.</typeparam>
    /// <typeparam name="T">The type of action invoked in selected case in switch</typeparam>
    public class ExpressionGenerator<TestT, T>
    {
        private bool _isInitialized = false;
        private readonly List<SwitchCase> _cases = new List<SwitchCase>();

        /// <summary>
        /// Gets the compiled SwitchExpression.
        /// </summary>
        /// <value>
        /// The compiled expression.
        /// </value>
        public Action<TestT, T> Compiled { get; private set; }
        /// <summary>
        /// Gets the switch expression.
        /// </summary>
        /// <value>
        /// The switch expression.
        /// </value>
        public Expression SwitchExpression { get; private set; }

        ParameterExpression expParameter = Expression.Parameter(typeof(TestT));
        ParameterExpression invokeParameter = Expression.Parameter(typeof(T));

        /// <summary>
        /// Generates the expression and return it's compiled version.
        /// </summary>
        /// <param name="forceRegenerate">if set to <c>true</c> force regeneration.</param>
        /// <returns>Compiled switch expression</returns>
        public Action<TestT, T> Generate(bool forceRegenerate = false)
        {
            if (_isInitialized && !forceRegenerate)
                return Compiled;
            SwitchExpression = Expression.Switch(expParameter, _cases.ToArray());
            Expression<Action<TestT, T>> expressionToCompile = Expression.Lambda<Action<TestT, T>>(SwitchExpression, expParameter, invokeParameter);
            Compiled = expressionToCompile.Compile();
            _isInitialized = true;
            return Compiled;
        }

        /// <summary>
        /// Adds the case.
        /// </summary>
        /// <param name="testValue">The test value.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public ExpressionGenerator<TestT, T> AddCase(TestT testValue, Expression<Action<T>> action)
        {
            var expCase = Expression.SwitchCase(
                Expression.Invoke(action, invokeParameter),
                Expression.Constant(testValue, typeof(TestT))
            );
            _cases.Add(expCase);
            return this;
        }
    }
}
