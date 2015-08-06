using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Identifier
{
    public class IdentifiedMetadata : IIdentifiedMetadata
    {

        public string IdentifierName { get; }
        public string ValueType { get; }
        public string Value { get; }

        public IdentifiedMetadata(string identifierName, string valueType, string value)
        {
            this.IdentifierName = identifierName;
            this.ValueType = valueType;
            this.Value = value;
        }
    }
}
