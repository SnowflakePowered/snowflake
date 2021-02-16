using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Attributes
{
    /// <summary>
    /// Defines a target context to create a read-only AST
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public sealed class ConfigurationTargetMemberAttribute : Attribute
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
        /// Marks a configuration section as a member of the given target.
        /// </summary>
        /// <param name="targetName">The target this member belongs to.</param>
        /// <param name="explode">Whether or not to explode the children of the section into the target node.</param>
        public ConfigurationTargetMemberAttribute(string targetName, bool explode)
        {
            this.TargetName = targetName;
            this.Explode = explode;
        }

        /// <summary>
        /// The name of the target.
        /// </summary>
        public string TargetName { get; }

        /// <summary>
        /// Whether or not to explode the members of this target.
        /// </summary>
        public bool Explode { get; }
    }
}
