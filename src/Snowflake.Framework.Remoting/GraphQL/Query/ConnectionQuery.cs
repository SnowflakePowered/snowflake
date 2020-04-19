using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;

namespace Snowflake.Remoting.GraphQL.Query
{
    internal class ConnectionQuery
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public Type GraphType { get; set; }
        public QueryArguments Arguments { get; set; }
        public Type ElementType { get; set; }
        public Type CollectionType { get; set; }
        public Func<IResolveFieldContext<object>, object> Resolver { get; set; }
    }
}
