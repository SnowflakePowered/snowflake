using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FastMember;
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
            var accessor = TypeAccessor.Create(typeof(T));
            foreach (var setter in 
                (from sectionInfo in accessor.GetMembers()
                    let sectionType = sectionInfo.Type
                    where typeof (IConfigurationSection).IsAssignableFrom(sectionType)
                    where sectionType.GetConstructor(Type.EmptyTypes) != null
                    let type = Instantiate.CreateInstance(sectionInfo.Type)
                    select new Action(() => accessor[configurationSection, sectionInfo.Name] = type)))
                setter();
            return configurationSection;
        }

        public override string ToString()
        {
            var sectionBuilder = new StringBuilder();

            foreach (var section in this)
            {
                sectionBuilder.Append(this.Serializer.Serialize(section));
            }
            return sectionBuilder.ToString();
        }

        public IEnumerator<IConfigurationSection> GetEnumerator()
        {
            var accessor = TypeAccessor.Create(this.GetType());
            return (from properties in accessor.GetMembers()
                where typeof(IConfigurationSection).IsAssignableFrom(properties.Type)
                select accessor[this, properties.Name] as IConfigurationSection).GetEnumerator();
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
