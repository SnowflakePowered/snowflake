using GraphQL.Types;

namespace Snowflake.Support.Remoting.GraphQL.RootProvider
{
    internal sealed class RootQuery : ObjectGraphType<object>
    {
        public RootQuery()
        {
            this.Name = "Query";
            this.Description = "The query root of Snowflake's GraphQL interface";
        }
    }
}
