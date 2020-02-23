using GraphQL.Types;
using Snowflake.Orchestration.Saving;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQLFrameworkQueries.Types.Saving
{
    public class SaveProfileGraphType : ObjectGraphType<ISaveProfile>
    {
        public SaveProfileGraphType()
        {
            Field<StringGraphType>("profileName", resolve: c => c.Source.ProfileName, description: "The type of the save.");
            Field<SaveManagementStrategyEnum>("managementStrategy", resolve: c => c.Source.ManagementStrategy,
                description: "The strategy this profile use to manage saves.");
            Field<StringGraphType>("saveType", resolve: c => c.Source.SaveType, description: "The type of the save.");
            Field<GuidGraphType>("profileGuid", resolve: c => c.Source.Guid,
                description: "The GUID of this save profile");
            Field<ListGraphType<SaveGameGraphType>>("history", resolve: c => c.Source.GetHistory(), description: "Gets all saves in the profile's history");
            Field<ListGraphType<SaveGameGraphType>>("head", resolve: c => c.Source.GetHeadSave(), description: "Gets all saves in the profile's history");
        }
    }
}
