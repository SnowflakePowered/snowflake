
using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using Newtonsoft.Json;

namespace Snowflake.Game.Database
{
    /// <summary>
    /// Handles conversion of string dictionaries in SQL serialization
    /// </summary>
    internal class DictionaryStringStringTypeHandler : SqlMapper.TypeHandler<IDictionary<string, string>>
    {
        public override void SetValue(IDbDataParameter parameter, IDictionary<string, string> value)
        {
            if (value == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = JsonConvert.SerializeObject(value);
            }
        }

        public override IDictionary<string, string> Parse(object value)
        {
            string strValue = value as string;
            return string.IsNullOrEmpty(strValue) ? null : JsonConvert.DeserializeObject<IDictionary<string, string>>(strValue);
        }
    }
}
