using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Extensibility
{
    /// <summary>
    /// Specifies the load priority for this container
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, Inherited = true)]
    public sealed class ContainerLoadPriorityAttribute : Attribute
    {
        /// <summary>
        /// The load priority of this container
        /// </summary>
        public ContainerLoadPriority LoadPriority { get; }
     
        public ContainerLoadPriorityAttribute(ContainerLoadPriority loadPriority)
        {
            this.LoadPriority = loadPriority;
        }
    }
}
