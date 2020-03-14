using GraphQL.Types;
using HotChocolate.Language;
using HotChocolate.Types;
using HotChocolate.Types.Filters;
using HotChocolate.Types.Relay;
using HotChocolate.Utilities;
using Snowflake.Framework.Remoting.GraphQL.Model.Records;
using Snowflake.Model.Records.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.Game
{
    public class GameQueries
        : ObjectType<GameQueryBuilder>
    {
        protected override void Configure(IObjectTypeDescriptor<GameQueryBuilder> descriptor)
        {
            descriptor
                .BindFieldsExplicitly();
            descriptor
                .Field("games")
                .Type<ListType<GameType>>()
                .AddFilterArguments<GameRecordQueryFilterInputType>()
                //.Argument("where", a => a.Type<GameRecordQueryFilterInputType>())
                .Use(next => context =>
                {
                    IValueNode filter = context.Argument<IValueNode>("where");

                    var queryBuilder = context.Parent<GameQueryBuilder>();

                    if (filter is null || filter is NullValueNode)
                    {
                        context.Result = queryBuilder.GetAllGames().ToList();
                        
                        return next(context);
                    }

                    if (context.Field.Arguments["where"].Type is InputObjectType iot && iot is IFilterInputType fit)
                    {
                        var visitor = new QueryableFilterVisitor(iot, typeof(IGameRecordQuery), context.GetTypeConversion(), true);
                        filter.Accept(visitor);
                        var expr = visitor.CreateFilter<IGameRecordQuery>();

                        var x = queryBuilder.GetGames(expr);
                        //queryBuilder.GetGames(g => g.Metadata.Any(g => g.MetadataKey == "x"));
                        context.Result = queryBuilder.GetGames(expr).ToList();
                    }

                    //if (context.Field.Arguments["where"].Type is InputObjectType iot && iot is GameRecordQueryFilterInputType grqf)
                    //{
                    //    var visitor = new GameRecordQueryFilterVisitor(iot, typeof(IGameRecordQuery), context.GetTypeConversion(), true);
                    //    filter.Accept(visitor);
                    //    var expr = visitor.CreateFilter<IGameRecordQuery>();

                    //    //queryBuilder.GetGames(g => g.Metadata.Any(g => g.MetadataKey == "x"));
                    //    context.Result = queryBuilder.GetGames(expr);
                    //}
                    return next(context);
                })
                .UsePaging<GameType>();
        }
    }
}
