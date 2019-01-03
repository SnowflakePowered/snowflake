using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;

namespace Snowflake.Framework.Remoting.GraphQL.Query
{
    internal class FieldQuery
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public Type GraphType { get; set; }
        public QueryArguments Arguments { get; set; }
        public Func<ResolveFieldContext<object>, object> Resolver { get; set; }
    }
}
