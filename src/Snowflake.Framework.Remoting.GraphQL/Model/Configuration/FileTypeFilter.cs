using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Remoting.GraphQL.Model.Configuration
{
    public sealed class FileTypeFilter
        : ObjectType<string>
    {
        protected override void Configure(IObjectTypeDescriptor<string> descriptor)
        {
            descriptor.Name("FileTypeFilter")
                .Description("Describes a filter for a file type.");
            descriptor.Field("name")
                .Description("The name of the filter.")
                .Resolver(ctx =>
                {
                    string filter = ctx.Parent<string>();
                    if (!filter.Contains('(')) return filter;
                    return filter.Substring(0, filter.IndexOf('('));
                }).Type<NonNullType<StringType>>();
            descriptor.Field("filters")
                .Description("The name of the filter.")
                .Resolver(ctx =>
                {
                    string filter = ctx.Parent<string>();
                    if (!filter.Contains('(') || !filter.Contains(')')) return filter.Split(",");
                    int bracketStart = filter.IndexOf('(') + 1;
                    return filter
                      .Substring(bracketStart, filter.IndexOf(')') - bracketStart)
                      .Split(",").Select(s => s.Trim());
                })
                .Type<NonNullType<ListType<NonNullType<StringType>>>>();
        }
    }
}
