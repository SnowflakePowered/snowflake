using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Serialization
{
    /// <summary>
    /// Defines a unit of a <see cref="IConfigurationCollection{T}"/> that will be
    /// evaluated independently into a tree of <see cref="IAbstractConfigurationNode"/>
    /// that will be evaluated and produce an output or side effects.
    /// 
    /// As an implementation detail, two configuaration targets of the same name
    /// are equal.
    /// </summary>
    public class ConfigurationTarget : IConfigurationTarget, IEquatable<ConfigurationTarget>
    {
        internal Dictionary<string, IConfigurationTarget> ChildTargets { get; }

        public string TargetName { get; }

        IReadOnlyDictionary<string, IConfigurationTarget> IConfigurationTarget.ChildTargets => this.ChildTargets;

        internal ConfigurationTarget(string targetName)
        {
            this.ChildTargets = new Dictionary<string, IConfigurationTarget>();
            this.TargetName = targetName;
        }

        public bool Equals(ConfigurationTarget other)
        {
            return other.TargetName == this.TargetName;
        }

        public override bool Equals(object obj)
        {
            if (obj is ConfigurationTarget t) return this.Equals(obj);
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.TargetName.GetHashCode();
        }
    }
}
