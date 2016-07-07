namespace Snowflake.Emulator
{
    /// <summary>
    /// Represents the assembly that an IEmulatorBridge will bridge to. An Emulator Assembly does not need to be 
    /// executable, it can be a callable library file such as a libretro core. As long as the game runs when the
    /// emulator bridge endpoints are called it is able to be used with Snowflake.
    /// </summary>
    public interface IEmulatorAssembly
    {
        /// <summary>
        /// The type of assembly the emulator is
        /// </summary>
        EmulatorAssemblyType AssemblyType { get; }
        /// <summary>
        /// The ID that emulator bridges can refer to 
        /// </summary>
        string EmulatorID { get; }
        /// <summary>
        /// The name of this emulator
        /// </summary>
        string EmulatorName { get; }
        /// <summary>
        /// The filename of the main emulator assembly that can be called into
        /// </summary>
        string MainAssembly { get; }
    }
}