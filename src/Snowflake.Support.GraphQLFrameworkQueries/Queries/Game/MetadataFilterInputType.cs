using HotChocolate.Types;
using HotChocolate.Types.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.Game
{
    internal class MetadataFilter
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public List<string> Value_in { get; set; }
    }

    internal class MetadataFilterInputType
        : InputObjectType<MetadataFilter>
    {
        protected override void Configure(IInputObjectTypeDescriptor<MetadataFilter> descriptor)
        {
            descriptor
                .Name("MetadataFilterInputType");
            descriptor
                .Field(f => f.Key)
                .Type<NonNullType<StringType>>();
            descriptor
                .Field(f => f.Value)
                .Type<StringType>();
            descriptor
                .Field(f => f.Value_in)
                .Type<ListType<NonNullType<StringType>>>();
        }
    }
}
