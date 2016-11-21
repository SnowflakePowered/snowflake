using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Ajax
{
    /// <summary>
    /// Represents a .NET exception formattable as a JSON payload
    /// </summary>
    public interface IJSException
    {
        /// <summary>
        /// The full .NET Exception incurred
        /// </summary>
        Exception FullException { get; }
        /// <summary>
        /// The exception message
        /// </summary>
        string Message { get; }
        /// <summary>
        /// The .NET name of the exception
        /// </summary>
        string ExceptionName { get; }
        /// <summary>
        /// The request that caused the exception, if applicable.
        /// </summary>
        IJSRequest SourceRequest { get; }
    }
}
