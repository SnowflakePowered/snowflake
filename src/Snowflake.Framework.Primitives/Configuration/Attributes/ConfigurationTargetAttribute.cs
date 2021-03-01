using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Configuration.Serialization;

namespace Snowflake.Configuration
{
    /// <summary>
    /// Defines a target context to create a read-only AST.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
    public class ConfigurationTargetAttribute : Attribute
    {
        /// <summary>
        /// Define a root target with the following name.
        /// </summary>
        /// <param name="targetName">The name of the target.</param>
        public ConfigurationTargetAttribute(string targetName)
        {
            this.TargetName = targetName;
            this.ParentTarget = null;
        }

        /// <summary>
        /// Define a child target with the following name and parent target.
        /// Each target name must be unique, and targets that do not lead back to a root target will
        /// be discarded. This ensures that no cycles are created when evaluating the target tree.
        /// </summary>
        /// <param name="targetName">The name of the target.</param>
        /// <param name="parentTarget">The name of the parent target from which this members' AST will be inserted into.</param>
        public ConfigurationTargetAttribute(string targetName, string parentTarget)
        {
            this.TargetName = targetName;
            this.ParentTarget = parentTarget;
        }

        /// <summary>
        /// The name of the target.
        /// </summary>
        public string TargetName { get; }

        /// <summary>
        /// If <see cref="IsRoot"/>  is false, then the name of the parent target that this target's AST will be
        /// combined with. Otherwise, it is null.
        /// </summary>
        public string? ParentTarget { get; }

        /// <summary>
        /// Whether or not this target is a root target that will evaluate an AST to a stream.
        /// </summary>
        public bool IsRoot => this.ParentTarget == null;
    }
}
