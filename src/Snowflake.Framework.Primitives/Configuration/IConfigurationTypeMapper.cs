using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration
{
    /// <summary>
    /// Maps types to a correct string value that can be interpreted by the emulator configuration.
    /// Convention requires that there be a default converver for Enum types implemented by default
    /// </summary>
    public interface IConfigurationTypeMapper
    {
        /// <summary>
        /// Adds a converter for this type mapper to support.
        /// </summary>
        /// <typeparam name="T">The type to convert</typeparam>
        /// <param name="converter">The converter function</param>
        void Add<T>(Func<T, string> converter);

        /// <summary>
        /// Converts a value to the format using a previously configured converter function
        /// </summary>
        /// <typeparam name="T">The type to convert</typeparam>
        /// <param name="value">The value to convert</param>
        /// <returns>The converted value</returns>
        string ConvertValue<T>(T value);

        /// <summary>
        /// Converts a value to the format using a previously configured converter function
        /// </summary>
        /// <param name="t">The type to convert</param>
        /// <param name="value">The value to convert</param>
        /// <returns>The converted value</returns>
        string this[Type t, object value] { get; }
    }
}
