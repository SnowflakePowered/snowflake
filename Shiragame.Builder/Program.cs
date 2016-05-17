using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Utility;

namespace Shiragame.Builder
{
    class Program
    {
        static void Main(string[] args)
        {
            var openvgdb = new OpenVgdb("ovgdb.db");
            var records = openvgdb.GetEverything();
            var memoryDb = new ShiragameDb();
            memoryDb.Commit(records);
            var diskDb = new SqliteDatabase("shiragame.db");
            memoryDb.SaveTo(diskDb);
        }
    }
}
