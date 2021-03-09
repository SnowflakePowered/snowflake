using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration.Internal
{
    /// <summary>
    /// For internal use only.
    /// Triggers SFC021: This class/method/interface requires a ConfigurationCollection template interface for generic argument {T}
    /// </summary>
    [AttributeUsage(AttributeTargets.Class 
        | AttributeTargets.Interface 
        | AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class GenericTypeAcceptsConfigurationCollectionAttribute
        : Attribute
    {
        public GenericTypeAcceptsConfigurationCollectionAttribute(int typeArgPosition)
        {
        }
    }
}
