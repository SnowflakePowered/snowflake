using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration.Internal
{
    /// <summary>
    /// For internal use only.
    /// Triggers SFC023: This class/method/interface requires an InputConfiguration template interface for generic argument {T}
    /// </summary>
    [AttributeUsage(AttributeTargets.Class 
        | AttributeTargets.Interface 
        | AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class GenericTypeAcceptsInputConfigurationAttribute
        : Attribute
    {
        /// <summary>
        /// </summary>
        /// <param name="typeArgPosition">The position of the generic argument to compare.</param>
        public GenericTypeAcceptsInputConfigurationAttribute(int typeArgPosition)
        {
        }
    }
}
