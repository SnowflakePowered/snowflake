using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class FieldAttribute : Attribute
    {
        public Type GraphType { get; }
        public string FieldName { get; }
        public string Description { get; }
        public FieldAttribute(string fieldName, string description, Type graphType)
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
