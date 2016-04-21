using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration
{
    public abstract class IterableConfigurationSection : ConfigurationSection, IIterableConfigurationSection
    {
        public int InterationNumber { get; set; }

        //todo add iteration number construct
        protected IterableConfigurationSection(string sectionName, string displayName, string configurationFilename, string description) : base(sectionName, displayName, configurationFilename, description)
        {
        }

        protected IterableConfigurationSection(string sectionName, string displayName, string configurationFilename) : base(sectionName, displayName, configurationFilename)
        {
        }
    }
}
