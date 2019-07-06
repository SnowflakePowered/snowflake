using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Serialization
{
    /// <summary>
    /// Defines a unit of a <see cref="IConfigurationCollection{T}"/> that will be
    /// evaluated independently into a tree of <see cref="IAbstractConfigurationNode"/>
    /// that will be evaluated and produce an output or side effects.
    /// </summary>
    public interface IConfigurationTarget
    {
        /// <summary>
        /// A dictionary of targets this target is a parent of.
        /// </summary>
        IReadOnlyDictionary<string, IConfigurationTarget> ChildTargets { get; }
        /// <summary>
        /// The name of the target.
        /// </summary>
        string TargetName { get; }
    }
}