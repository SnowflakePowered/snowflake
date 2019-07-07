using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Snowflake.Configuration;
using Snowflake.Configuration.Input;
using Snowflake.Extensibility.Provisioning;
using Snowflake.Filesystem;

namespace Snowflake.Execution.Extensibility
{
    public abstract class
        ConfigurationFactory<TConfigurationCollection, TInputTemplate> : IConfigurationFactory<TConfigurationCollection,
            TInputTemplate>
        where TConfigurationCollection : class, IConfigurationCollection<TConfigurationCollection>
        where TInputTemplate : class, IInputTemplate<TInputTemplate>

    {
        public IEnumerable<IInputMapping> InputMappings { get; }

        protected ConfigurationFactory(IPluginProvision provision)
            : this(provision.CommonResourceDirectory.OpenDirectory("InputMappings").EnumerateFiles()
                .Select(mapping => JsonConvert.DeserializeObject<InputMapping>(mapping.ReadAllText()))
                .Cast<IInputMapping>().ToList())
        {
        }

        protected ConfigurationFactory(IEnumerable<IInputMapping> inputMappings)
        {
            this.InputMappings = inputMappings;
        }

        IConfigurationCollection IConfigurationFactory.GetConfiguration(Guid gameRecord, string profileName)
        {
            return this.GetConfiguration(gameRecord, profileName);
        }

        IConfigurationCollection IConfigurationFactory.GetConfiguration()
        {
            return this.GetConfiguration();
        }

        (IInputTemplate template, IInputMapping mapping) IConfigurationFactory.GetInputMappings(
            IEmulatedController emulatedDevice)
        {
            return this.GetInputMappings(emulatedDevice);
        }

        public abstract (IInputTemplate<TInputTemplate> template, IInputMapping mapping) GetInputMappings(
            IEmulatedController emulatedDevice);

        public abstract IConfigurationCollection<TConfigurationCollection> GetConfiguration(Guid gameRecord,
            string profileName);

        public abstract IConfigurationCollection<TConfigurationCollection> GetConfiguration();
    }
}
