using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Snowflake.Game
{
  
    public partial class GameInfo
    {
        /// <summary>
        /// <em>Required for SQLite serialzation. Do Not Use.</em>
        /// Dapper requires a constructor that is structured like so:
        /// 
        /// (string uuid, string platformId, string fileName, string name, string crc32, string metadata).
        /// 
        /// Supplying raw string JSON is a very bad idea, and the only reason it is here is for use with the DapperGameDatabase,
        /// where it can be guaranteed that the JSON is a dictionary.
        /// </summary>
        [Obsolete("Required for SQLite serialzation. Do Not Use.")]
        internal GameInfo(string uuid, string platformId, string fileName, string name, string crc32, string metadata)
            : this(uuid, platformId,
                fileName, name, crc32, new Dictionary<string, string>())
			{
            if (!String.IsNullOrEmpty(metadata))
            {
                this.Metadata = JsonConvert.DeserializeObject<IDictionary<string, string>>(metadata);
            }
        }
    }
}
