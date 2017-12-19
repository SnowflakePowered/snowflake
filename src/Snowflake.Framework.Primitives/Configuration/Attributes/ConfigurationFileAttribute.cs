using System;

namespace Snowflake.Configuration.Attributes
{
    /// <summary>
    /// Defines an alias to an output configuration file for a given configuration collection.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
    public class ConfigurationFileAttribute : Attribute
    {
        /// <summary>
        /// Gets the key with which the section properties of the collection will refer to this output.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Gets the filename that the section properties of the collection will output to.
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Gets the boolean mapping for this file for true
        /// </summary>
        public string TrueMapping { get; }

        /// <summary>
        /// Gets the boolean mapping fo
        /// </summary>
        public string FalseMapping { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationFileAttribute"/> class.
        /// Defines an alias to an output configuration file for a given configuration collection.
        /// </summary>
        /// <param name="key">The key with which the section properties of the collection will refer to this output.</param>
        /// <param name="filename"> The filename that the section properties of the collection will output to.</param>
        /// <param name="trueMapping">The string that true maps to in the configuration file.</param>
        /// <param name="falseMapping">The string that false maps to in the configuration file.</param>
        public ConfigurationFileAttribute(string key, string filename, string trueMapping = "True", string falseMapping = "False")
        {
            this.Key = key;
            this.FileName = filename;
            this.TrueMapping = trueMapping;
            this.FalseMapping = falseMapping;
        }
    }
}
