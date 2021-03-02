using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration.Internal
{
    /// <summary>
    /// For internal use only.
    /// Triggers SFC022: This class/method/interface requires an ConfigurationSection template interface for generic argument {T}
    /// </summary>
    [AttributeUsage(AttributeTargets.Class 
        | AttributeTargets.Interface 
        | AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class GenericTypeAcceptsConfigurationSectionAttribute
        : Attribute
    {
        /// <summary>
        /// </summary>
        /// <param name="typeArgPosition">The position of the generic argument to compare.</param>
        public GenericTypeAcceptsConfigurationSectionAttribute(int typeArgPosition)
        {
        }
    }
}
