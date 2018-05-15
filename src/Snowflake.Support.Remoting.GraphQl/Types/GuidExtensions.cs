using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Types
{
    public static class GuidExtensions
    {
        public static string ToGraphQlUniqueId(this Guid guid, string recordType)
        {
            var recordTypeBytes = Encoding.UTF8.GetBytes($"{recordType}:");
            return Convert.ToBase64String(recordTypeBytes.Concat(guid.ToByteArray()).ToArray());
        }
    }
}
