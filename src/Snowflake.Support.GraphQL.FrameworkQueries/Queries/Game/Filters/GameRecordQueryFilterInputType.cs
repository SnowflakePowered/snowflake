using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.Model.Stone;
using Snowflake.Model.Records.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotChocolate.Data.Filters;
using Snowflake.Remoting.GraphQL.Model.Stone.PlatformInfo;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.Game
{
    public class GameRecordQueryFilterInputType
        : FilterInputType<IGameRecordQuery>
    {
        protected override void Configure(IFilterInputTypeDescriptor<IGameRecordQuery> descriptor)
        {
            descriptor
                .Name("GameFilter")
                .BindFieldsExplicitly();
            descriptor
                .Field(g => g.PlatformID)
                .Name("platformId");
            descriptor.Field(g => g.RecordID)
                .Name("recordId");
            descriptor.Field(g => g.Metadata)
                .Name("metadata")
                .Type<ListFilterInputType<MetadataFilterInputType>>();
            //    .Filter(g => g.PlatformID)
            //    .AllowEquals()
            //        .Name("platformId")
            //        .Description("Get the platform with the specific PlatformID.").And()
            //    .AllowIn()
            //        .Name("platformId_in")
            //        .Description("Get platforms with the given PlatformIDs.").And()
            //    .AllowStartsWith()
            //        .Name("platformId_startsWith")
            //        .Description("Get platforms whose PlatformIDs start with the given string");
            //descriptor
            //   .Filter(g => g.RecordID)
            //   .AllowEquals()
            //        .Name("gameId")
            //        .Description("Gets the Game with the given ID.");
            //// todo for 11
            //descriptor
            //    .List(g => g.Metadata)
            //    .AllowAll<MetadataFilterInputType>().And()
            //    .AllowSome<MetadataFilterInputType>().And()
            //    .AllowNone<MetadataFilterInputType>().And()
            //    .AllowAny();
        }
    }
    //public class GameRecordQueryFilterInputType
    //    : InputObjectType
    //{
    //    protected override void Configure(IInputObjectTypeDescriptor descriptor)
    //    {
    //        descriptor.Name("GameRecordQueryFilter");
    //        descriptor.Field("platformID")
    //            .Type<PlatformIdType>();
    //        descriptor.Field("platformID_not")
    //            .Type<PlatformIdType>();
    //        descriptor.Field("platformID_in")
    //            .Type<ListType<NonNullType<PlatformIdType>>>();
    //        descriptor.Field("platformID_not_in")
    //            .Type<PlatformIdType>();
    //        descriptor.Field("recordID")
    //            .Type<UuidType>();
    //        descriptor.Field("recordID_in")
    //            .Type<ListType<NonNullType<UuidType>>>();
    //        descriptor.Field("recordID_not")
    //           .Type<UuidType>();
    //        descriptor.Field("recordID_not_in")
    //            .Type<ListType<NonNullType<UuidType>>>();
    //        descriptor.Field("metadata")
    //            .Type<ListType<NonNullType<MetadataFilterInputType>>>();
    //        descriptor.Field("OR")
    //           .Type<ListType<NonNullType<GameRecordQueryFilterInputType>>>();
    //        descriptor.Field("AND")
    //            .Type<ListType<NonNullType<GameRecordQueryFilterInputType>>>();
    //    }
    //}
}
