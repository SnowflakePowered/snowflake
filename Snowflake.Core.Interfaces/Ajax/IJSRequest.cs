using System.Collections.Generic;

namespace Snowflake.Ajax
{
    /// <summary>
    /// Represents an Ajax Javascript Request
    /// </summary>
    public interface IJSRequest
    {
        /// <summary>
        /// Gets a parameter from the request
        /// </summary>
        /// <param name="paramKey">The parameter key</param>
        /// <returns>The value of the parameter with the given key</returns>
        string GetParameter(string paramKey);
        /// <summary>
        /// The name of the method called
        /// </summary>
        string MethodName { get; }
        /// <summary>
        /// The method parameters the request was called with 
        /// </summary>
        IDictionary<string, string> MethodParameters { get; }
        /// <summary>
        /// The Javascript namespace the called method belongs to
        /// </summary>
        string NameSpace { get; }
    }
}
