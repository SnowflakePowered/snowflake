using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Emulator.Configuration.Type
{
    public class CustomTypeValue
    {
        public readonly string Value;
        public readonly string Description;

        public CustomTypeValue(string value, string description)
        {
            this.Value = value;
            this.Description = description;
        }
    }
}
