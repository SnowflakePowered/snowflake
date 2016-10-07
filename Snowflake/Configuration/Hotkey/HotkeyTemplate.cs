using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Configuration.Hotkey
{
    public abstract class HotkeyTemplate : IHotkeyTemplate
    {
        public string SectionName { get; }
        public IEnumerable<IHotkeyOption> HotkeyOptions { get; }
        public IEnumerable<IConfigurationOption> ConfigurationOptions { get; }

        public abstract HotkeyTrigger ModifierTrigger { get; set; }

        protected HotkeyTemplate(string sectionName)
        {
            this.SectionName = sectionName;
            //cache the configuration properties of this section
            this.ConfigurationOptions = this.GetConfigProperties();
            this.HotkeyOptions = this.GetHotkeyOptions();
        }


        private IList<IConfigurationOption> GetConfigProperties()
        {
            return (from propertyInfo in this.GetType().GetRuntimeProperties()
                    where propertyInfo.IsDefined(typeof(ConfigurationOptionAttribute), true)
                    select new ConfigurationOption(propertyInfo, this, Guid.Empty) as IConfigurationOption).ToList();
        }


        private IList<IHotkeyOption> GetHotkeyOptions()
        {
            return (from propertyInfo in this.GetType().GetRuntimeProperties()
                    where propertyInfo.IsDefined(typeof(HotkeyOptionAttribute), true)
                    select new HotkeyOption(propertyInfo, this) as IHotkeyOption).ToList();
        }
    }
}
