using GraphQL.Conventions.Adapters.Types;
using GraphQL.Types;
using Snowflake.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Types.Record
{
    public class RecordInterface : InterfaceGraphType<IRecord>
    {
        public RecordInterface()
        {
            Name = "Record";
            Description = "A piece of Recordable and Metadatable data.";
            Field<GuidGraphType>("guid",
                description: "The unique ID of the record.",
                resolve: context => context.Source);
            Field<ListGraphType<RecordMetadataGraphType>>(
                "metadata",
                description: "A list of metadata related to this data.",
                resolve: context => context.Source.Metadata.Select(m => m.Value)                
                );
        }
    }
}
