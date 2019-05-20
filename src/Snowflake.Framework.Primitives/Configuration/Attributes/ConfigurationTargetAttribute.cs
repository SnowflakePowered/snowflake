using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Attributes
{
    /// <summary>
    /// Defines a target context to create a read-only AST.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
    public class ConfigurationTargetAttribute : Attribute
    {
        /// <summary>
        /// Define a root target with the following name and root target transformer.
        /// </summary>
        /// <param name="targetName">The name of the target.</param>
        /// <param name="targetTransformer">The type of the ConfigurationTargetTransformer that will evaluate the AST.</param>
        public ConfigurationTargetAttribute(string targetName, Type targetTransformer)
        {
            this.TargetName = targetName;
            this.TargetTransformer = targetTransformer;
        }

        /// <summary>
        /// Define a child target with the following name and parent target.
        /// </summary>
        /// <param name="targetName">The name of the target.</param>
        /// <param name="parentTarget">The name of the parent target from which this members' AST will be inserted into.s</param>
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
        /// If <see cref="IsRoot"/> is true, the type of the target transformer that will evaulate this target's resultant AST.
        /// </summary>
        public Type? TargetTransformer { get; }

        /// <summary>
        /// Whether or not this target is a root target that will evaluate an AST to a stream.
        /// </summary>
        public bool IsRoot => this.TargetTransformer == null;
    }
}
