using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Emulator;
using Snowflake.Records.Game;

namespace Snowflake.Plugin.EmulatorAdapter.RetroArch
{
    public class RetroArchInstance : EmulatorInstance
    {
        public RetroArchInstance(RetroArchConfiguration configSection)
        {
            this.ConfigurationCollection = new Dictionary<string, IConfigurationCollection> { {configSection.FileName, configSection}};
        }

        public override IDictionary<string, IConfigurationCollection> ConfigurationCollection { get; }

        public override void Create()
        {
            var rcg = ConfigurationCollection["retroarch.cfg"] as RetroArchConfiguration;
            rcg.DirectoryConfiguration.SavefileDirectory = this.InstancePath;
           // rcg.DirectoryConfiguration.SystemDirectory set bios files directory
           //biosmanager?
            foreach (IConfigurationCollection configuration in this.ConfigurationCollection.Values)
            {
                var sectionBuilder = new StringBuilder();

                foreach (var section in configuration)
                {
                    sectionBuilder.Append(configuration.Serializer.Serialize(section));
                }
                File.WriteAllText(Path.Combine(this.InstancePath, configuration.FileName),sectionBuilder.ToString());
            }
            //start retroarchexe here with instacnepath as working directory
        }

        public override void Start()
        {
            throw new NotImplementedException();
        }

        public override void Pause()
        {
            throw new NotImplementedException();
        }

        public override void Resume()
        {
            throw new NotImplementedException();
        }

        public override void Destroy()
        {
            throw new NotImplementedException();
        }

        public override DateTime StartTime { get; }
        public override DateTime DestroyTime { get; }
    }
}
