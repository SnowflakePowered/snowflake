using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensions;

namespace Snowflake.Emulator.Configuration.Type
{
    public class CustomType
    {

        public readonly string TypeName;
        private IList<CustomTypeValue> typeValues;
        public IReadOnlyCollection<CustomTypeValue> TypeValues { get { return this.typeValues.AsReadOnly(); } }

        public CustomType(string name, IList<CustomTypeValue> typeValues)
        {
            this.typeValues = typeValues;
            this.TypeName = name;
        }
    }
}
