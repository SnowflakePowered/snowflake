using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Serialization
{
    public class ConfigurationTarget : IConfigurationTarget
    {
        internal Dictionary<string, IConfigurationTarget> ChildTargets { get; }

        public string TargetName { get; }

        IReadOnlyDictionary<string, IConfigurationTarget> IConfigurationTarget.ChildTargets => this.ChildTargets;

        internal ConfigurationTarget(string targetName)
        {
            this.ChildTargets = new Dictionary<string, IConfigurationTarget>();
            this.TargetName = targetName;
        }
    }
}
