using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Orchestration
{
    public sealed class CreateEmulationInstanceInput
    {
        public string Orchestrator { get; set; }
        public Guid GameID { get; set; }
        public Guid CollectionID { get; set; }
        public Guid SaveProfileID { get; set; }

    }
}
