using System;

namespace Snowflake.Configuration.Attributes
{
    /// <summary>
    /// Defines an alias to an output configuration file for a given configuration collection.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = true)]
    public class ConfigurationFileAttribute : Attribute
    {
        /// <summary>
        /// The key with which the section properties of the collection will refer to this output.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// The filename that the section properties of the collection will output to.
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Defines an alias to an output configuration file for a given configuration collection.
        /// </summary>
        /// <param name="key">The key with which the section properties of the collection will refer to this output.</param>
        /// <param name="filename"> The filename that the section properties of the collection will output to.</param>
        public ConfigurationFileAttribute(string key, string filename)
        {
            this.Key = key;
            this.FileName = filename;
        }
    }
}
