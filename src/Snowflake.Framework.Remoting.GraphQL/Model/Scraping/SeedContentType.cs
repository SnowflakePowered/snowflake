using HotChocolate.Types;
using Snowflake.Scraping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Remoting.GraphQL.Model.Scraping
{
    public sealed class SeedContentType
        : ObjectType<SeedContent>
    {
        protected override void Configure(IObjectTypeDescriptor<SeedContent> descriptor)
        {
            descriptor.Name("SeedContent")
                .Description("The contents of a scraping seed.");
            descriptor.Field(s => s.Value)
                .Description("The string value of the scraping result.")
                .Type<NonNullType<StringType>>();
            descriptor.Field(s => s.Type)
                .Description("The semantic type of the scraping result.")
                .Type<NonNullType<StringType>>();
        }
    }
}
