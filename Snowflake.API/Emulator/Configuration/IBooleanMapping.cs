namespace Snowflake.Emulator.Configuration
{
    /// <summary>
    /// Maps the value of true and false to how an emulator configuration file stores booleans
    /// </summary>
    public interface IBooleanMapping
    {
        /// <summary>
        /// The value of false in an emulator configuration file
        /// </summary>
        string False { get; }
        /// <summary>
        /// The value of true in an emulator configuration file
        /// </summary>
        string True { get; }
        /// <summary>
        /// Get the string value of a boolean
        /// </summary>
        /// <param name="value">The value to get for</param>
        /// <returns>The emulator configuration representation of the boolean</returns>
        string FromBool(bool value);

    }
}
