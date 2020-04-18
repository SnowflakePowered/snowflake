using HotChocolate.Types;
using HotChocolate.Types.Filters;
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
                .Filter(f => f.MetadataKey)
                .AllowEquals().And()
                .AllowNotEquals().And()
                .AllowContains().And()
                .AllowNotContains();
            descriptor
                .Filter(f => f.MetadataValue)
                .AllowEquals().And()
                .AllowNotEquals().And()
                .AllowContains().And()
                .AllowNotContains();
        }
    }
}
