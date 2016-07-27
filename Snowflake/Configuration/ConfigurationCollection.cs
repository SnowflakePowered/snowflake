using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Utility;

namespace Snowflake.Configuration
{
    /// <summary>
    /// Provides an abstract implementation for configuration collection.
    /// Implementations must have a zero-parameter constructor.
    /// </summary>
    public abstract class ConfigurationCollection : IConfigurationCollection
    {
        /// <summary>
        /// Initializes a configuration collection with default values for all sections.
        /// </summary>
        /// <typeparam name="T">The configuration collection to initialize</typeparam>
        /// <returns>The configuration section with default values.</returns>
        public static T MakeDefault<T>() where T : IConfigurationCollection, new()
        {
            var configurationSection = new T();
            foreach (var setter in 
                (from sectionInfo in typeof (T).GetProperties()
                    let sectionType = sectionInfo.PropertyType
                    where typeof (IConfigurationSection).IsAssignableFrom(sectionType)
                    where sectionType.GetConstructor(Type.EmptyTypes) != null
                    let type = Instantiate.CreateInstance(sectionInfo.PropertyType)
                    select new Action(() => sectionInfo.SetValue(configurationSection, type))))
                setter();
            return configurationSection;
        }

        public IEnumerator<IConfigurationSection> GetEnumerator()
        {
            return (from properties in this.GetType().GetProperties()
                where typeof(IConfigurationSection).IsAssignableFrom(properties.PropertyType)
                select properties.GetValue(this) as IConfigurationSection).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IConfigurationSerializer Serializer { get; }
        public string FileName { get; }

        protected ConfigurationCollection(IConfigurationSerializer serializer, string fileName)
        {
            this.Serializer = serializer;
            this.FileName = fileName;
        }
    }
}
