using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Identifier
{
    public class IdentifiedMetadata : IIdentifiedMetadata
    {

        public string IdentifierName { get; private set; }
        public string ValueType { get; private set; }
        public string Value { get; private set; }

        public IdentifiedMetadata(string identifierName, string valueType, string value)
        {
            this.IdentifierName = identifierName;
            this.ValueType = valueType;
            this.Value = value;
        }
    }
}
