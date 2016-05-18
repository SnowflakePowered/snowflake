using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Snowflake.Utility;

namespace Shiragame.Builder
{
    class Program
    {
        static void Main(string[] args)
        {
            /*     var openvgdb = new OpenVgdb("openvgdb.sqlite");
                 var records = openvgdb.GetEverything();
                 var memoryDb = new ShiragameDb();
                 memoryDb.Commit(records);
                 var diskDb = new SqliteDatabase("shiragame.db");
                 memoryDb.SaveTo(diskDb);*/

            var entries = DatParser.ParseClrMamePro("cmp.dat", "NINTENDO_NES").ToList();
            //var entries = DatParser.ParseLogiqx("tosec.dat", "NINTENDO_N64").ToList();
        }
    }
}
