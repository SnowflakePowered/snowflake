namespace Snowflake.Emulator
{
    /// <summary>
    /// Types of emulator assemblies
    /// </summary>
    public enum EmulatorAssemblyType
    {
        /// <summary>
        /// The assembly is neither an executable nor a library. It is undefined.
        /// </summary>
        EMULATOR_MISC,
        /// <summary>
        /// The assembly is an executable, such as an exe on Windows.
        /// <example>retroarch.exe</example> would be an executable emulator assembly.
        /// </summary>
        EMULATOR_EXECUTABLE,
        /// <summary>
        /// The assembly is a library, and is not directly callable. If it is a libretro core, the emulator bridge
        /// is responsible for providing the libretro frontend environment.
        /// </summary>
        EMULATOR_LIBRARY

    }
}
