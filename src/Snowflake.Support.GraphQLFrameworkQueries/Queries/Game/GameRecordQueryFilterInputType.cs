using HotChocolate.Types;
using HotChocolate.Types.Filters;
using Snowflake.Framework.Remoting.GraphQL.Model.Stone;
using Snowflake.Model.Records.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.Game
{
    public class GameRecordQueryFilterInputType
        : FilterInputType<IGameRecordQuery>
    {
        protected override void Configure(IFilterInputTypeDescriptor<IGameRecordQuery> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor
                .Filter(g => g.PlatformID);
            descriptor
               .Filter(g => g.RecordID);
            descriptor
                .List(g => g.Metadata);

      

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
