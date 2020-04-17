using GraphQL.Types;
using Snowflake.Installation.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Support.GraphQLFrameworkQueries.Types.Installable
{
    public class InstallableGraphType : ObjectGraphType<InstallableGraphObject>
    {
        public InstallableGraphType()
        {
            Name = "Installable";
            Description = "A grouping of installable file entries that can be installed as a unit to a Game.";
            Field<StringGraphType>("displayName",
                description: "The friendly display name of the installable file.",
                resolve: o => o.Source.DisplayName);
            Field<ListGraphType<StringGraphType>>("artifacts",
               description: "The list of artifacts to be installed.",
               resolve: o => o.Source.Artifacts);
            Field<StringGraphType>("installerName",
                description: "The name of the installer that produced this installable",
                resolve: o => o.Source.InstallerName);
        }
    }
}
