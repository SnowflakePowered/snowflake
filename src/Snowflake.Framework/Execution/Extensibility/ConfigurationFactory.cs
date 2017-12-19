using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Snowflake.Configuration;
using Snowflake.Configuration.Input;
using Snowflake.Extensibility.Provisioning;
using Snowflake.Records.Game;

namespace Snowflake.Execution.Extensibility
{
    public abstract class ConfigurationFactory : IConfigurationFactory
    {
        public IEnumerable<IInputMapping> InputMappings { get; }

        protected ConfigurationFactory(IPluginProvision provision)
            : this(provision.CommonResourceDirectory.CreateSubdirectory("InputMappings").EnumerateFiles()
                 .Select(mapping => JsonConvert.DeserializeObject<InputMapping>(File.ReadAllText(mapping.FullName)))
                 .Cast<IInputMapping>().ToList())
        {
        }

        protected ConfigurationFactory(IEnumerable<IInputMapping> inputMappings)
        {
            this.InputMappings = inputMappings;
        }

        public abstract IConfigurationCollection GetConfiguration(IGameRecord gameRecord, string profileName = "default");
        public abstract IConfigurationCollection GetConfiguration();
        public abstract IInputTemplate GetInputTemplate(IEmulatedController emulatedDevice);
    }
}
