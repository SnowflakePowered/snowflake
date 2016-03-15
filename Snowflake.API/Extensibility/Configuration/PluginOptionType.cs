namespace Snowflake.Extensibility.Configuration
{
    /// <summary>
    /// Types of Plugin Configuration Flags
    /// Part of the Plugin Configuration API
    /// </summary>
    public enum PluginOptionType
    {
        /// <summary>
        /// The flag is either true or false
        /// </summary>
        BOOLEAN_FLAG,
        /// <summary>
        /// The flag is an integer
        /// </summary>
        INTEGER_FLAG,
        /// <summary>
        /// The flag is a string selectable from a choice of other selectable strings
        /// </summary>
        SELECT_FLAG,
        /// <summary>
        /// The flag is a freely-inputtable UTF-8 string
        /// </summary>
        STRING_FLAG
    }
}
