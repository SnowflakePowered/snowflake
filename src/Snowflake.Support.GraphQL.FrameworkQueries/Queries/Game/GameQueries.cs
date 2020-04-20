using GraphQL.Types;
using HotChocolate.Language;
using HotChocolate.Types;
using HotChocolate.Types.Filters;
using HotChocolate.Types.Relay;
using HotChocolate.Utilities;
using Snowflake.Remoting.GraphQL.Model.Records;
using Snowflake.Model.Game;
using Snowflake.Model.Records.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Snowflake.Remoting.GraphQL;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.Game
{
    public class GameQueries
        : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor
                .Name("Query");

            descriptor
                .Field("games")
                .Type<NonNullType<ListType<NonNullType<GameType>>>>()
                .Argument("excludeDeleted", arg => arg.Type<BooleanType>()
                    .Description("Exclude games that have been marked as deleted. " +
                    "Setting this to true is shorthand for retrieving games with the game_deleted metadata not set to \"true\".")
                    .DefaultValue(true))
                .AddFilterArguments<GameRecordQueryFilterInputType>()
                .Use(next => context =>
                {
                    IValueNode valueNode = context.Argument<IValueNode>("where");
                    bool excludeDeleted = context.Argument<bool>("excludeDeleted");
                    Expression<Func<IGameRecordQuery, bool>> excludeDeletedQuery = r =>
                                !r.Metadata.Any(r => r.MetadataKey == GameMetadataKeys.Deleted && r.MetadataValue == "true");

                    var queryBuilder = context.SnowflakeService<IGameLibrary>();

                    if (valueNode is null || valueNode is NullValueNode)
                    {
                        if (!excludeDeleted)
                        {
                            context.Result = queryBuilder
                                .GetAllGames()
                                .ToList();
                        }
                        else
                        {
                            context.Result = queryBuilder.QueryGames(excludeDeletedQuery);
                        }
                        return next.Invoke(context);
                    }

                    if (context.Field.Arguments["where"].Type is InputObjectType iot && iot is IFilterInputType fit)
                    {

                        var filter = new QueryableFilterVisitorContext(iot, typeof(IGameRecordQuery), context.GetTypeConversion(), true);
                        QueryableFilterVisitor.Default.Visit(valueNode, filter);

                        var expr = filter.CreateFilter<IGameRecordQuery>();

                        
                        //var queryResult = queryBuilder.QueryGames(expr);
                        //queryBuilder.GetGames(g => g.Metadata.Any(g => g.MetadataKey == "x"));
                        
                        if (!excludeDeleted)
                        {
                            context.Result = queryBuilder.QueryGames(expr).ToList();
                        }
                        else
                        {
                            // Invoke works fine here. We could use a expression tree visitor, but
                            // that is a lot more code than just using Invoke.
                            ParameterExpression param = expr.Parameters[0];
                            var combinedBody = Expression.AndAlso(
                                   expr.Body,
                                   Expression.Invoke(excludeDeletedQuery, param)
                               );
                            var combinedLambda = Expression.Lambda<Func<IGameRecordQuery, bool>>(combinedBody, param);
                            context.Result = queryBuilder.QueryGames(combinedLambda).ToList();
                        }

                    }
                    return next.Invoke(context);
                })
                .UsePaging<GameType>();
        }
    }
}
