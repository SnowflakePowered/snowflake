using GraphQL.Types;
using Snowflake.Orchestration.Saving;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQLFrameworkQueries.Types.Saving
{
    public class SaveManagementStrategyEnum : EnumerationGraphType<SaveManagementStrategy>
    {
        public SaveManagementStrategyEnum()
        {
            Name = "SaveManagementStrategy";
            Description = "Various strategies used to manage save profile data.";
        }
    }
}
