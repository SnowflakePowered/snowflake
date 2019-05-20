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
    [Obsolete("Use ConfigurationTargetMemberAttribute")]
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
        /// Gets the destination the serialized form of this configuration will output to
        /// as defined by the <see cref="ConfigurationFileAttribute"/> attributes
        /// in the <see cref="IConfigurationCollection{T}"/> class.
        /// </summary>
        public string Destination { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableSectionAttribute"/> class.
        /// Default constructor for the section attribute
        /// </summary>
        /// <param name="destination">The destination the serialized form of this configuration will output to, see <see cref="Destination"/></param>
        public SerializableSectionAttribute(string destination)
        {
            this.Destination = destination;
        }
    }
}
