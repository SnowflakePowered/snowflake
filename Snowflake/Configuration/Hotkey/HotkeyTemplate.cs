using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;
using Snowflake.Configuration.Input.Hotkey;

namespace Snowflake.Configuration.Hotkey
{
    public abstract class HotkeyTemplate : IHotkeyTemplate
    {
        public string SectionName { get; }
        public string FileName { get; }
        public IEnumerable<IHotkeyOption> HotkeyOptions { get; }
        public IEnumerable<IConfigurationOption> ConfigurationOptions { get; }
        public HotkeyTemplateType TemplateType { get; }



        protected HotkeyTemplate(string sectionName, string fileName, HotkeyTemplateType templateType)
        {
            this.SectionName = sectionName;
            this.FileName = fileName;
            //cache the configuration properties of this section
            this.ConfigurationOptions = this.GetConfigProperties();
            this.HotkeyOptions = this.GetHotkeyOptions();
            this.TemplateType = templateType;
        }


        private IList<IConfigurationOption> GetConfigProperties()
        {
            return (from propertyInfo in this.GetType().GetRuntimeProperties()
                    where propertyInfo.IsDefined(typeof(ConfigurationOptionAttribute), true)
                    select new ConfigurationOption(propertyInfo, this) as IConfigurationOption).ToList();
        }


        private IList<IHotkeyOption> GetHotkeyOptions()
        {
            return (from propertyInfo in this.GetType().GetRuntimeProperties()
                    where propertyInfo.IsDefined(typeof(HotkeyOptionAttribute), true)
                    select new HotkeyOption(propertyInfo, this) as IHotkeyOption).ToList();
        }
    }
}
