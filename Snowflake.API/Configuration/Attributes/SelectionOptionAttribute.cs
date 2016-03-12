using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class SelectionOptionAttribute : Attribute
    {
        public string SerializeAs { get; }
        public string DisplayName { get; set; }

        public SelectionOptionAttribute(string serializeAs)
        {
            this.SerializeAs = serializeAs;
        }
    }
}
