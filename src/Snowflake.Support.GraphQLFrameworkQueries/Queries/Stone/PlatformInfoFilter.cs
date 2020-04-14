using HotChocolate.Types.Filters;
using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.Stone
{
    public class PlatformInfoFilter
        : FilterInputType<IPlatformInfo>
    {
        protected override void Configure(IFilterInputTypeDescriptor<IPlatformInfo> descriptor)
        {
            descriptor
                .Name("PlatformInfoFilter")                
                .Filter(p => p.PlatformID)
                
                .AllowEquals().Description("Get the platform with the specific PlatformID.").And()
                .AllowIn().Description("Get platforms with the given PlatformIDs.").And()
                .AllowStartsWith().Description("Get platforms whose PlatformIDs start with the given string");
        }
    }
}
