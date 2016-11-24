using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration.Attributes
{
    /// <summary>
    /// Marks a configuration section property as serializable to configuration
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SerializableSectionAttribute : Attribute
    {
        /// <summary>
        /// The flags pseudo-destination
        /// </summary>
        public const string FlagsOutputPath = "#flags";
        /// <summary>
        /// The input config pseudo-destination
        /// </summary>
        public const string InputOutputPath = "#input";
        /// <summary>
        /// The destination the serialized form of this configuration will output to 
        /// as defined by the <see cref="ConfigurationFileAttribute"/> attributes
        /// in the <see cref="IConfigurationCollection{T}"/> class.
        /// </summary>
        public string Destination { get; }

        /// <summary>
        /// Default constructor for the section attribute
        /// </summary>
        /// <param name="destination">The destination the serialized form of this configuration will output to, see <see cref="Destination"/></param>
        public SerializableSectionAttribute(string destination)
        {
            this.Destination = destination;
        }
    }
}
