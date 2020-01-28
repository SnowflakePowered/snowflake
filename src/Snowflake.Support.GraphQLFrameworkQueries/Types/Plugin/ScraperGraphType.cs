﻿using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Scraping.Extensibility;

namespace Snowflake.Support.GraphQLFrameworkQueries.Types.Plugin
{
    public class ScraperGraphType : ObjectGraphType<IScraper>
    {
        public ScraperGraphType()
        {
            Name = "Scraper";
            Description = "A scraper plugin produces seeds to create game records.";
            Field(p => p.Author).Description("The author of the plugin.");
            Field(p => p.Description).Description("The description of the plugin.");
            Field(p => p.Name).Description("The plugin name.");
            Field<NonNullGraphType<StringGraphType>>("version",
                resolve: context => context.Source.Version.ToString(),
                description: "The version of the plugin.");

            Field(p => p.TargetType).Description("The type of seeds this scraper requires.");
            Interface<PluginInterfaceType>();
        }
    }
}
