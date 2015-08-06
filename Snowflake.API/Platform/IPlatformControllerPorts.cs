namespace Snowflake.Platform
{
    public interface IPlatformControllerPorts
    {
        /// <summary>
        /// The controller in first port of the platform
        /// </summary>
        string Port1 { get; set; }
        /// <summary>
        /// The controller in second port of the platform
        /// </summary>
        string Port2 { get; set; }
        /// <summary>
        /// The controller in third port of the platform
        /// </summary>
        string Port3 { get; set; }
        /// <summary>
        /// The controller in fourth port of the platform
        /// </summary>
        string Port4 { get; set; }
        /// <summary>
        /// The controller in fifth port of the platform
        /// </summary>
        string Port5 { get; set; }
        /// <summary>
        /// The controller in sixth port of the platform
        /// </summary>
        string Port6 { get; set; }
        /// <summary>
        /// The controller in seventh port of the platform
        /// </summary>
        string Port7 { get; set; }
        /// <summary>
        /// The controller in eigth port of the platform
        /// </summary>
        string Port8 { get; set; }
        /// <summary>
        /// Gets the port for a certain port number 1-8
        /// </summary>
        /// <param name="portNumber">The port number 1-8</param>
        /// <returns>The corresponding port</returns>
        string GetPort(int portNumber);
        /// <summary>
        /// Indexer shim for GetPort
        /// </summary>
        /// <param name="portNumber">The port number 1-8</param>
        /// <returns>The corresponding port</returns>
        string this[int portNumber] { get; }

    }
}
