using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Tooling.Taskrunner.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class TaskAttribute : Attribute
    {
        public string Name { get; }
        public string Description { get; }
        public TaskAttribute(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }
    }
}
