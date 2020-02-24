using Snowflake.Input.Controller;
using Snowflake.Model.Game;
using Snowflake.Support.StoneProvider.JsonConverters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Snowflake.Support.StoneProviders
{
    [JsonConverter(typeof(StoneDataConverter))]
    internal class StoneData
    {
        public IReadOnlyDictionary<PlatformId, IPlatformInfo> Platforms { get; }
        public IReadOnlyDictionary<ControllerId, IControllerLayout> Controllers { get; }
        public Version Version { get; }

        public StoneData(IReadOnlyDictionary<PlatformId, IPlatformInfo> platforms,
            IReadOnlyDictionary<ControllerId, IControllerLayout> controllers,
            Version version)
        {
            this.Platforms = platforms;
            this.Controllers = controllers;
            this.Version = version;
        }
    }
}
