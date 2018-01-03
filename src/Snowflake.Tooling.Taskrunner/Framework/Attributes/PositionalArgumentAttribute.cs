using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Tooling.Taskrunner.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class PositionalArgumentAttribute : Attribute
    {
        public int Position { get; }
        public string Description { get; }

        public PositionalArgumentAttribute(int position, string description)
        {
            this.Position = position;
            this.Description = description;
        }
    }
}
