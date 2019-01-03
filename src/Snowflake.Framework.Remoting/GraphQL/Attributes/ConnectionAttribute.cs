using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphQL.Types;

namespace Snowflake.Framework.Remoting.GraphQL.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class ConnectionAttribute : Attribute
    {
        public Type GraphType { get; }
        public string FieldName { get; }
        public string Description { get; }

        public ConnectionAttribute(string fieldName, string description, Type graphType)
        {
            this.GraphType = graphType;
            this.FieldName = fieldName;
            this.Description = description;

            if (!this.GraphType.GetInterfaces().Contains(typeof(IGraphType)))
            {
                throw new InvalidOperationException($"The connection type for {this.FieldName} must be a graph type.");
            }
        }
    }
}
