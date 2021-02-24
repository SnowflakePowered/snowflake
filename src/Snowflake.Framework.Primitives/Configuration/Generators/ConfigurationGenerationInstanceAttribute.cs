using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration.Generators
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class ConfigurationGenerationInstanceAttribute
        : System.Attribute
    {
        public ConfigurationGenerationInstanceAttribute(Type instanceType)
        {
            this.InstanceType = instanceType;
        }

        public Type InstanceType { get; }
    }
}
