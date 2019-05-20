using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Attributes
{
    /// <summary>
    /// Defines a target context to create a read-only AST
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ConfigurationTargetMemberAttribute : Attribute
    {
        /// <summary>
        /// Marks a configuration section as a member of the given target.
        /// </summary>
        /// <param name="targetName">The target this member belongs to.</param>
        public ConfigurationTargetMemberAttribute(string targetName)
        {
            this.TargetName = targetName;
        }

        /// <summary>
        /// The name of the target.
        /// </summary>
        public string TargetName { get; }
    }
}
