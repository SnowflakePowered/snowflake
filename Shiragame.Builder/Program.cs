using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Shiragame.Builder.Parser;
using Snowflake.Utility;

namespace Shiragame.Builder
{
    class Program
    {
        static void Main(string[] args)
        {
             //    var openvgdb = new OpenVgdb("openvgdb.sqlite");
           // var records = openvgdb.GetSerialInfos().ToList();
            //     var memoryDb = new ShiragameDb();
            //   memoryDb.Commit(records);
            // var diskDb = new SqliteDatabase("shiragame.db");
            //memoryDb.SaveTo(diskDb);*/

            var entries = GameTdbParser.ParseSerials("wiitdb.txt", "NINTENDO_WII").ToList();
            //var entries = DatParser.ParseLogiqx("tosec.dat", "NINTENDO_N64").ToList();
        }
    }
}
