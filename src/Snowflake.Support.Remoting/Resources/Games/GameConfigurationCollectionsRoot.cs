using Snowflake.Configuration;
using Snowflake.Emulator;
using Snowflake.Records.Game;
using Snowflake.Remoting.Resources;
using Snowflake.Remoting.Resources.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Resources.Games
{
    [Resource("games", ":game", "configs", ":profile")]
    [Parameter(typeof(IGameRecord), "game")]
    [Parameter(typeof(string), "profile")]
    public class GameConfigurationCollectionsRoot : Resource
    {
        private IEnumerable<IEmulatorAdapter> AdapterCollection { get; }
        public GameConfigurationCollectionsRoot(IEnumerable<IEmulatorAdapter> adapter)
        {
            this.AdapterCollection = adapter;
        }

        [Endpoint(EndpointVerb.Read)]
        public IDictionary<string, IConfigurationCollection> GetConfigs(IGameRecord game, string profile)
        {
            var fileTypes = game.Files.Select(f => f.MimeType);
            var emulatorsWithFileTypes = (from adapter in this.AdapterCollection
                                          from mimetype in fileTypes
                                          where adapter.Mimetypes.Contains(mimetype)
                                          select adapter).Distinct();
            var configurations = (from emulator in emulatorsWithFileTypes
                                  let config = emulator.GetConfiguration(game, profile)
                                  let emulatorName = emulator.Name
                                  select (config: config, emulatorName: emulatorName));
                                  
            return configurations.ToDictionary(p => p.emulatorName, p => p.config);

        }
    }
}
