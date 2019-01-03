using System;
using System.Collections.Generic;
using System.Linq;
using GraphQL.Types;
using Snowflake.Model.Records;

namespace Snowflake.Support.Remoting.GraphQL.Types.Model
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

            Field<StringGraphType>("id",
                description: "The opaque GraphQL unique ID of the game. For caching purposes only.",
                resolve: context => context.Source.RecordId.ToGraphQlUniqueId("Record"));

            Field<ListGraphType<RecordMetadataGraphType>>(
                "metadata",
                description: "A list of metadata related to this data.",
                resolve: context => context.Source.Metadata.Select(m => m.Value));
        }
    }
}
