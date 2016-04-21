using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration
{
    public abstract class ConfigurationCollection : IConfigurationCollection
    {
        public static T MakeDefault<T>() where T : IConfigurationCollection, new()
        {
            var configurationSection = Activator.CreateInstance<T>();
            foreach (var setter in 
                (from sectionInfo in typeof (T).GetProperties()
                    let sectionType = sectionInfo.PropertyType
                    where typeof (IConfigurationSection).IsAssignableFrom(sectionType)
                    where sectionType.GetConstructor(Type.EmptyTypes) != null
                    let type = Activator.CreateInstance(sectionInfo.PropertyType)
                    select new Action(() => sectionInfo.SetValue(configurationSection, type))))
                setter();
            return configurationSection;
        }
    }
}
