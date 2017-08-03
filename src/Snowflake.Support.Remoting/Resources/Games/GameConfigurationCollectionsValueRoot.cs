using EnumsNET.NonGeneric;
using Snowflake.Configuration;
using Snowflake.Emulator;
using Snowflake.Records.Game;
using Snowflake.Remoting.Resources;
using Snowflake.Remoting.Resources.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Snowflake.Resources.Games
{
    [Resource("games", ":game", "configs", ":profile", ":emulator")]
    [Parameter(typeof(IGameRecord), "game")]
    [Parameter(typeof(string), "profile")]
    [Parameter(typeof(string), "emulator")]
    public class GameConfigurationCollectionsValueRoot : Resource
    {
        private IEnumerable<IEmulatorAdapter> AdapterCollection { get; }
        private IConfigurationCollectionStore Store { get; }
        public GameConfigurationCollectionsValueRoot(IEnumerable<IEmulatorAdapter> adapter, IConfigurationCollectionStore store)
        {
            this.AdapterCollection = adapter;
            this.Store = store;
        }

        [Endpoint(EndpointVerb.Update)]
        [Parameter(typeof(Guid), "valueGuid")]
        [Parameter(typeof(string), "newStrValue")]
        public IConfigurationCollection SetConfigValue(IGameRecord game, string profile, string emulator, Guid valueGuid, string newStrValue)
        {
            // We have to go through this convoluted process because ConfigurationValues are not type safe.
            // Without the underlying option metadata, we are unable to assign a proper value.
            var emulatorAdapter = this.AdapterCollection.FirstOrDefault(e => e.Name == emulator);
            var config = emulatorAdapter?.GetConfiguration(game, profile);
            var configValue = config.SelectMany(p => p.Value).FirstOrDefault(p => p.Value.Guid == valueGuid);
            var newValue = GameConfigurationCollectionsValueRoot.FromString(newStrValue, configValue.Key?.Type);
            configValue.Value.Value = newValue;
            this.Store.Set(configValue.Value);
            return emulatorAdapter.GetConfiguration(game, profile);
        }

        [Endpoint(EndpointVerb.Read)]
        public IConfigurationCollection GetConfiguration(IGameRecord game, string profile, string emulator)
        {
            var emulatorAdapter = this.AdapterCollection.FirstOrDefault(e => e.Name == emulator);
            var config = emulatorAdapter?.GetConfiguration(game, profile);
            return config;
        }

        // This is the same implementation as in ConfigurationSection.
        private static object FromString(string strValue, Type optionType)
        {
            return optionType == typeof(string)
                ? strValue //return string value if string
                : optionType.GetTypeInfo().IsEnum
                    ? NonGenericEnums.Parse(optionType, strValue)//return parsed enum if enum
                    : TypeDescriptor.GetConverter(optionType).ConvertFromInvariantString(strValue);
        }
    }
}
