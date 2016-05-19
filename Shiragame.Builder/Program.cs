using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Shiragame.Builder.Parser;
using Snowflake.Platform;
using Snowflake.Service;
using Snowflake.Utility;

namespace Shiragame.Builder
{
    class Program
    {
        static void Main(string[] args)
        {
            var memoryDb = new ShiragameDb();
            if (!Directory.Exists("PlatformDats"))
            {
                Console.WriteLine("PlatformDats folder does not exist.. Creating Directory Structure");
                Directory.CreateDirectory("PlatformDats");
                var stone = new StoneProvider();
                Console.WriteLine("Using Stone v" + stone.StoneVersion);
                foreach (var platform in stone.Platforms)
                {
                    Directory.CreateDirectory(Path.Combine("PlatformDats", platform.Key));
                }
            }
            if (File.Exists("PlatformDats\\openvgdb.sqlite"))
            {
                Console.WriteLine("OpenVGDB Found. Parsing...");
                var openvgdb = new OpenVgdb("openvgdb.sqlite");
                var serials = openvgdb.GetSerialInfos().ToList();
                var dats = openvgdb.GetDatInfos().ToList();
                memoryDb.Commit(dats);
                memoryDb.Commit(serials);

            }

            Console.WriteLine("Saving Database to shiragame.db ...");
            var diskDb = new SqliteDatabase("shiragame.db");
            memoryDb.SaveTo(diskDb);


            var entries = GameTdbParser.ParseSerials("wiitdb.txt", "NINTENDO_WII").ToList();
            //var entries = DatParser.ParseLogiqx("tosec.dat", "NINTENDO_N64").ToList();
        }
    }
}
