using System;

namespace SymbolicDifferentiation.Core.AST
{
    public class ConditionalExpression : Expression
    {
        public Expression Condition { private set; get; }
        public Expression Success { private set; get; }
        public Expression Failure { private set; get; }

        public override T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return String.Format("({0}) ? {1} : {2}", Condition, Success, Failure);
        }

        public static Expression Create(Expression condition, Expression success, Expression failure)
        {
            return new ConditionalExpression{ Condition = condition, Success = success, Failure = failure };
        }
    }
}
