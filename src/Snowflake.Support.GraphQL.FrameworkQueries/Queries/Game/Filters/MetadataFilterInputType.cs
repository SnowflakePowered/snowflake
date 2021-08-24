using HotChocolate.Data.Filters;
using HotChocolate.Types;
using Snowflake.Model.Records;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.Game
{
    internal class MetadataFilterInputType
        : FilterInputType<IRecordMetadataQuery>
    {
        protected override void Configure(IFilterInputTypeDescriptor<IRecordMetadataQuery> descriptor)
        {
            descriptor
                .Name("RecordMetadataFilter");
            descriptor
                .Field(f => f.MetadataKey)
                .Name("key");
            descriptor
                .Field(f => f.MetadataValue)
                .Name("value");
  
        }
    }
}
