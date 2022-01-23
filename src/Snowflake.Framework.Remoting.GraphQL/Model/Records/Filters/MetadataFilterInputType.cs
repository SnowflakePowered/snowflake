using HotChocolate.Data.Filters;
using HotChocolate.Types;
using Snowflake.Model.Records;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Remoting.GraphQL.Records.Filters
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
