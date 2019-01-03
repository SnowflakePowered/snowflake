using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowflake.Extensibility.Provisioning;
using Snowflake.Model.Game;
using Snowflake.Services;

namespace Snowflake.Execution.Extensibility
{
    internal sealed class EmulatorProperties : IEmulatorProperties
    {
        public EmulatorProperties(IPluginProvision provision, IStoneProvider stone)
        {
            this.SpecialCapabilities = provision.Properties.GetEnumerable("capabilities").ToList();
            this.Mimetypes = provision.Properties.GetEnumerable("mimetypes").ToList();
            this.SaveFormat = provision.Properties.Get("saveformat");
            this.OptionalSystemFiles = (from biosFile in provision.Properties.GetEnumerable("optionalbios")
                let files = stone.Platforms.Values.SelectMany(p => p.BiosFiles)
                    .Where(b => b.FileName.Equals(biosFile, StringComparison.OrdinalIgnoreCase))
                select files).SelectMany(p => p).ToList();
            this.RequiredSystemFiles = (from biosFile in provision.Properties.GetEnumerable("requiredbios")
                let files = stone.Platforms.Values.SelectMany(p => p.BiosFiles)
                    .Where(b => b.FileName.Equals(biosFile, StringComparison.OrdinalIgnoreCase))
                select files).SelectMany(p => p).ToList();
        }

        public string SaveFormat { get; }

        public IEnumerable<string> Mimetypes { get; }

        public IEnumerable<IBiosFile> RequiredSystemFiles { get; }

        public IEnumerable<IBiosFile> OptionalSystemFiles { get; }

        public IEnumerable<string> SpecialCapabilities { get; }
    }
}
