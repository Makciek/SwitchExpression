using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SwitchExpression
{
    public class ExpressionGenerator<TestT, T>
    {
        private bool _isInitialized = false;

        private Expression _expression;
        private List<SwitchCase> _cases = new List<SwitchCase>();
        
        public Action<TestT, T> Compiled { get; set; }


        ParameterExpression expParameter = Expression.Parameter(typeof(TestT));
        ParameterExpression invokeParameter = Expression.Parameter(typeof(T));

        public Action<TestT, T> Generate()
        {
            if (_isInitialized)
                return Compiled;
            _expression = Expression.Switch(expParameter, _cases.ToArray());
            Expression<Action<TestT, T>> expressionToCompile = Expression.Lambda<Action<TestT, T>>(_expression, expParameter, invokeParameter);
            Compiled = expressionToCompile.Compile();
            _isInitialized = true;
            return Compiled;
        }

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
