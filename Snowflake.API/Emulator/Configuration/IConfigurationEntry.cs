namespace Snowflake.Emulator.Configuration
{
    /// <summary>
    /// An IConfigurationEntry represents 1 entry in an emulator configuration
    /// </summary>
    public interface IConfigurationEntry
    {
        /// <summary>
        /// The default value of the entry
        /// </summary>
        dynamic DefaultValue { get; }
        /// <summary>
        /// The description of the entry
        /// </summary>
        string Description { get; }
        /// <summary>
        /// The name of the entry
        /// </summary>
        string Name { get; }
    }
}
