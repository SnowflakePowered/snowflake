using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Queries.Game
{
    public class ReifyingMethodCallExpressionVisitor : ExpressionVisitor
    {
        [return: NotNullIfNotNull("node")]
        public override Expression Visit(Expression node)
        {
            return base.Visit(node);
        }

        protected override Expression VisitUnary(UnaryExpression expr)
        {
            var visitedOp = this.Visit(expr.Operand);
            return expr.Update(visitedOp);
        }

        protected override Expression VisitMember(MemberExpression expr)
        {
            if (expr.Expression is ConstantExpression constant)
            {
                var val = constant.Value;
                var type = val.GetType();
                if (!type.FullName.StartsWith("HotChocolate.Data.Filters.Expressions.FilterExpressionBuilder"))
                    return expr;
                var innerVal = val.GetType().GetField("value").GetValue(val);
                return Expression.Constant(innerVal);
            }
            return expr;
        }
    }
}
