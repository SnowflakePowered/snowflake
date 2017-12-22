using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Loader;

namespace Snowflake.Support.Remoting.GraphQl.Types.Module
{
    public class ModuleGraphType : ObjectGraphType<IModule>
    {
        public ModuleGraphType()
        {
            Field(m => m.Author).Description("The author of the module.");
            Field(m => m.Name).Description("THe name of the module.");
            Field(m => m.Loader).Description("The name of the loader used to load this module.");
            Field<NonNullGraphType<StringGraphType>>("version",
                resolve: context => context.Source.Version,
                description: "The version of the module as specified in its manifest.");
        }
    }
}
