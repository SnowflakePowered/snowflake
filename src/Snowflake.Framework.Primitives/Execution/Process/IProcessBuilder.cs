using System.Diagnostics;

namespace Snowflake.Execution.Process
{
    /// <summary>
    /// Provides a fluent API on top of <see cref="ProcessStartInfo"/>
    /// to make launching processes easier and cleaner.
    /// </summary>
    public interface IProcessBuilder
    {
        /// <summary>
        /// Create a <see cref="ProcessStartInfo"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="ProcessStartInfo"/> with the given arguments.</returns>
        ProcessStartInfo ToProcessStartInfo();

        /// <summary>
        /// Adds a switch type argument to the launching process.
        /// </summary>
        /// <param name="switchName">The argument to add.</param>
        /// <returns>The instance of <see cref="IProcessBuilder"/>.</returns>
        IProcessBuilder WithArgument(string switchName);

        /// <summary>
        /// Adds a space separated parameter-type argument to the launching process.
        /// </summary>
        /// <param name="parameterName">The name of the parameter,</param>
        /// <param name="value">The value of the argument.</param>
        /// <param name="quoted">Whether or not the value is enclosed in double quotes.</param>
        /// <returns>The instance of <see cref="IProcessBuilder"/></returns>
        IProcessBuilder WithArgument(string parameterName, string value, bool quoted = true);
    }
}