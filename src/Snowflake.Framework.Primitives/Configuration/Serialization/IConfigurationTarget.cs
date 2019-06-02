using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Serialization
{
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
        /// <summary>
        /// If not null, the type of the target transformer that will evaulate this target's resultant AST.
        /// </summary>
        Type? TargetTransformer { get; }
    }
}
