using HotChocolate.Data.Filters;
using Snowflake.Model.Game;
using Snowflake.Remoting.GraphQL.Model.Stone.PlatformInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.Stone
{
    public class PlatformInfoFilterInputType
        : FilterInputType<IPlatformInfo>
    {
        protected override void Configure(IFilterInputTypeDescriptor<IPlatformInfo> descriptor)
        {
            descriptor
                .Name("PlatformInfoFilter")
                .Field(p => p.PlatformID)
                    .Name("platformId");
                
                //.AllowEquals()
                //    .Name("platformId")
                //    .Description("Get the platform with the specific PlatformID.").And()
                //.AllowIn()
                //    .Name("platformId_in")
                //    .Description("Get platforms with the given PlatformIDs.").And()
                //.AllowStartsWith()
                //    .Name("platformId_startsWith")
                //    .Description("Get platforms whose PlatformIDs start with the given string");
        }
    }
}
