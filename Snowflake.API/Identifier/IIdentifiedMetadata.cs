using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Identifier
{
    public interface IIdentifiedMetadata
    {
        string IdentifierName { get; }
        string ValueType { get; }
        string Value { get; }
    }
}
