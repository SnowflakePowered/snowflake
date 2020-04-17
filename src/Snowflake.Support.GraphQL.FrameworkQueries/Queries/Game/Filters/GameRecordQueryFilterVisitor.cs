using HotChocolate.Language;
using HotChocolate.Types;
using HotChocolate.Types.Filters;
using HotChocolate.Types.Filters.Expressions;
using HotChocolate.Utilities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Snowflake.Model.Records.Game;
using System.Linq;
using System.Reflection;
using Snowflake.Model.Records;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.Game
{
    //public class GameRecordQueryFilterVisitor
    //    : FilterVisitorBase
    //{
    //    private readonly bool _inMemory;
    //    private readonly ITypeConversion _converter;

    //    protected Stack<QueryableClosure> Closures { get; } = new Stack<QueryableClosure>();

    //    public GameRecordQueryFilterVisitor(
    //        InputObjectType initialType,
    //        Type source,
    //        ITypeConversion converter,
    //        bool inMemory)
    //        : base(initialType)
    //    {
    //        if (initialType is null)
    //        {
    //            throw new ArgumentNullException(nameof(initialType));
    //        }
    //        if (source is null)
    //        {
    //            throw new ArgumentNullException(nameof(source));
    //        }
          
    //        if (converter is null)
    //        {
    //            throw new ArgumentNullException(nameof(converter));
    //        }

    //        _converter = converter;
    //        _inMemory = inMemory;
    //        Closures.Push(new QueryableClosure(source, "r", _inMemory));
    //    }

    //    public Expression<Func<TSource, bool>> CreateFilter<TSource>()
    //    {
    //        return Closures.Peek().CreateLambda<Func<TSource, bool>>();
    //    }

    //    #region Object Value

    //    public override VisitorAction Enter(
    //        ObjectValueNode node,
    //        ISyntaxNode parent,
    //        IReadOnlyList<object> path,
    //        IReadOnlyList<ISyntaxNode> ancestors)
    //    {
    //        Closures.Peek().Level.Push(new Queue<Expression>());
    //        return VisitorAction.Continue;
    //    }

    //    public override VisitorAction Leave(
    //        ObjectValueNode node,
    //        ISyntaxNode parent,
    //        IReadOnlyList<object> path,
    //        IReadOnlyList<ISyntaxNode> ancestors)
    //    {
    //        Queue<Expression> operations = Closures.Peek().Level.Pop();

    //        if (TryCombineOperations(
    //            operations,
    //            (a, b) => Expression.AndAlso(a, b),
    //            out Expression combined))
    //        {
    //            Closures.Peek().Level.Peek().Enqueue(combined);
    //        }

    //        return VisitorAction.Continue;
    //    }

    //    #endregion

    //    #region Object Field
    //    public override VisitorAction Enter(
    //        ObjectFieldNode node,
    //        ISyntaxNode parent,
    //        IReadOnlyList<object> path,
    //        IReadOnlyList<ISyntaxNode> ancestors)
    //    {
    //        base.Enter(node, parent, path, ancestors);

    //        var field = Operations.Peek();
    //        var gameRecordQuery = Closures.Peek().Instance.Peek();

    //        var _platformID = Expression.Property(gameRecordQuery, nameof(IGameRecordQuery.PlatformID));
    //        var _recordID = Expression.Property(gameRecordQuery, nameof(IGameRecordQuery.RecordID));
    //        var _metadata = Expression.Property(gameRecordQuery, nameof(IGameRecordQuery.Metadata));

    //        var filterExpression = field.Name.ToString() switch
    //        {
    //            "platformID" => FilterExpressionBuilder.Equals(_platformID, field.Type.ParseLiteral(node.Value)),
    //            "platformID_not" => FilterExpressionBuilder.NotEquals(_platformID, field.Type.ParseLiteral(node.Value)),
    //            "platformID_in" => FilterExpressionBuilder.In(_platformID, field.Type.ElementType().ToClrType(), field.Type.ParseLiteral(node.Value)),
    //            "platformID_not_in" => FilterExpressionBuilder.Not(FilterExpressionBuilder.In(_platformID, 
    //                field.Type.ElementType().ToClrType(), field.Type.ParseLiteral(node.Value))),

    //            "recordID" => FilterExpressionBuilder.Equals(_recordID, field.Type.ParseLiteral(node.Value)),
    //            "recordID_not" => FilterExpressionBuilder.NotEquals(_recordID, field.Type.ParseLiteral(node.Value)),
    //            "recordID_in" => FilterExpressionBuilder.In(_recordID, field.Type.ElementType().ToClrType(), field.Type.ParseLiteral(node.Value)),
    //            "recordID_not_in" => FilterExpressionBuilder.Not(FilterExpressionBuilder.In(_recordID, 
    //                field.Type.ElementType().ToClrType(), field.Type.ParseLiteral(node.Value))),
    //            _ => null
    //        };

    //        if (filterExpression == null && field.Name != "metadata") return VisitorAction.Continue;
    //        if (field.Name == "metadata" && field.Type.ParseLiteral(node.Value) is IList<MetadataFilter> metadataFilters)
    //        {
    //            Queue<Expression> metadataExprs = new Queue<Expression>();
                
    //            foreach(var filter in metadataFilters)
    //            {
    //                //FilterExpressionBuilder.Any(typeof(IRecordMetadataQuery));
    //            }
    //            return VisitorAction.Continue;
    //        }
    //        Closures.Peek().Level.Peek().Enqueue(filterExpression);

    //        return VisitorAction.Continue;
    //    }

    //    public override VisitorAction Leave(
    //        ObjectFieldNode node,
    //        ISyntaxNode parent,
    //        IReadOnlyList<object> path,
    //        IReadOnlyList<ISyntaxNode> ancestors)
    //    {

    //        return base.Leave(node, parent, path, ancestors);
    //    }

    //    private bool TryCombineOperations(
    //        Queue<Expression> operations,
    //        Func<Expression, Expression, Expression> combine,
    //        out Expression combined)
    //    {
    //        if (operations.Count != 0)
    //        {
    //            combined = operations.Dequeue();

    //            while (operations.Count != 0)
    //            {
    //                combined = combine(combined, operations.Dequeue());
    //            }

    //            return true;
    //        }

    //        combined = null;
    //        return false;
    //    }

    //    #endregion

    //    #region List

    //    public override VisitorAction Enter(
    //        ListValueNode node,
    //        ISyntaxNode parent,
    //        IReadOnlyList<object> path,
    //        IReadOnlyList<ISyntaxNode> ancestors)
    //    {
    //        Closures.Peek().Level.Push(new Queue<Expression>());
    //        return VisitorAction.Continue;
    //    }

    //    private static Func<Expression, Expression, Expression> CombineOr = new Func<Expression, Expression, Expression>(
    //                (a, b) => Expression.OrElse(a, b));
    //    private static Func<Expression, Expression, Expression> CombineAnd = new Func<Expression, Expression, Expression>(
    //                (a, b) => Expression.AndAlso(a, b));

    //    public override VisitorAction Leave(
    //        ListValueNode node,
    //        ISyntaxNode parent,
    //        IReadOnlyList<object> path,
    //        IReadOnlyList<ISyntaxNode> ancestors)
    //    {
    //        var combine = Operations.Peek().Name.Equals("OR")
    //            ? CombineOr
    //            : CombineAnd;

    //        Queue<Expression> operations = Closures.Peek().Level.Pop();

    //        if (TryCombineOperations(
    //            operations,
    //            combine,
    //            out Expression combined))
    //        {
    //            Closures.Peek().Level.Peek().Enqueue(combined);
    //        }

    //        return VisitorAction.Continue;
    //    }

    //    #endregion
    //}
}
