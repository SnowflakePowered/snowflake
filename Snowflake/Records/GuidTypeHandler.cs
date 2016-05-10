using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using Newtonsoft.Json;

namespace Snowflake.Utility
{
    /// <summary>
    /// Handles conversion of string dictionaries in SQL serialization
    /// </summary>
    internal class GuidTypeHandler : SqlMapper.TypeHandler<Guid>
    {
        public override void SetValue(IDbDataParameter parameter, Guid value)
        {
            parameter.Value = value.ToByteArray();
        }

        public override Guid Parse(object value)
        {
            return new Guid((byte[])value);
        }
    }
}
