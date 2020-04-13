using GraphQL.Types;
using HotChocolate.Language;
using HotChocolate.Types;
using HotChocolate.Types.Filters;
using HotChocolate.Types.Relay;
using HotChocolate.Utilities;
using Snowflake.Framework.Remoting.GraphQL.Model.Records;
using Snowflake.Model.Game;
using Snowflake.Model.Records.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                .Type<ListType<GameType>>()
                .AddFilterArguments<GameRecordQueryFilterInputType>()
                .Use(next => context =>
                {
                    IValueNode valueNode = context.Argument<IValueNode>("where");

                    var queryBuilder = context.Service<IGameLibrary>();

                    if (valueNode is null || valueNode is NullValueNode)
                    {
                        context.Result = queryBuilder.GetAllGames().ToList();
                        return next.Invoke(context);
                    }

                    if (context.Field.Arguments["where"].Type is InputObjectType iot && iot is IFilterInputType fit)
                    {

                        var filter = new QueryableFilterVisitorContext(iot, typeof(IGameRecordQuery), context.GetTypeConversion(), true);
                        QueryableFilterVisitor.Default.Visit(valueNode, filter);

                        var expr = filter.CreateFilter<IGameRecordQuery>();
                        //var queryResult = queryBuilder.QueryGames(expr);
                        //queryBuilder.GetGames(g => g.Metadata.Any(g => g.MetadataKey == "x"));
                        context.Result = queryBuilder.QueryGames(expr).ToList();
                    }
                    return next.Invoke(context);
                });
                //.UsePaging<GameType>();
        }
    }
}
