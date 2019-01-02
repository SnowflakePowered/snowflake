using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Shiragame.Builder.Parser;
using Snowflake.Persistence;
using Snowflake.Platform;
using Snowflake.Services;

namespace Shiragame.Builder
{
    class Program
    {
        static void Main(string[] args)
        {
            var stone = new StoneProvider();
            Console.WriteLine("Using Stone Platforms v" + stone.StoneVersion);

            IEnumerable<RomInfo> datInfos = new List<RomInfo>();
            IEnumerable<SerialInfo> serialInfos = new List<SerialInfo>();
            IEnumerable<string> mameFilenames = new List<string>();
            if (!Directory.Exists("PlatformDats"))
            {
                Console.WriteLine("PlatformDats folder does not exist.. Creating Directory Structure");
                Directory.CreateDirectory("PlatformDats");
                foreach (var platform in stone.Platforms)
                {
                    Directory.CreateDirectory(Path.Combine("PlatformDats", platform.Key));
                }
            }

            if (File.Exists(Path.Combine("PlatformDats", "openvgdb.sqlite")))
            {
                Console.WriteLine("OpenVGDB Found. Parsing...");
                var openvgdb = new OpenVgdb(Path.Combine("PlatformDats", "openvgdb.sqlite"));
                serialInfos = serialInfos.Concat(openvgdb.GetSerialInfos().ToList());
                datInfos = datInfos.Concat(openvgdb.GetDatInfos().ToList());
                mameFilenames = mameFilenames.Concat(openvgdb.GetMameFiles().ToList());
            }

            foreach (string platformId in stone.Platforms.Select(p => p.Key))
            {
                if (!Directory.Exists(Path.Combine("PlatformDats", platformId)))
                {
                    continue;
                }

                foreach (string file in Directory.EnumerateFiles(Path.Combine("PlatformDats", platformId)))
                {
                    Console.Write(platformId + " found: " + Path.GetFileName(file));
                    if (Path.GetExtension(file) == ".idlist")
                    {
                        Console.WriteLine(" is type of ID List");

                        serialInfos = serialInfos.Concat(IdlistParser.ParseSerials(file, platformId));
                        continue;
                    }

                    switch (DatParser.GetParser(File.ReadLines(file).First()))
                    {
                        case ParserClass.Cmp:
                            Console.WriteLine(" is type of ClrMamePro");
                            serialInfos = serialInfos.Concat(CmpParser.ParseSerials(file, platformId));
                            datInfos = datInfos.Concat(CmpParser.Parse(file, platformId));
                            continue;
                        case ParserClass.Tdb:
                            Console.WriteLine(" is type of GameTDB");
                            serialInfos = serialInfos.Concat(GameTdbParser.ParseSerials(file, platformId));
                            continue;
                        case ParserClass.Xml:
                            Console.WriteLine(" is type of Logiqix XML");
                            datInfos = datInfos.Concat(XmlParser.Parse(file, platformId));
                            continue;
                        default:
                            Console.WriteLine(" is invalid.");
                            continue;
                    }
                }
            }

            Console.WriteLine("Generating shiragame.db ...");
            var memoryDb = new ShiragameDb();
            if (!Directory.Exists("out"))
            {
                Directory.CreateDirectory("out");
            }

            var diskDb = new SqliteDatabase("out\\shiragame.db");
            memoryDb.Commit(datInfos.ToList());
            memoryDb.Commit(serialInfos.DistinctBy(x => new {x.PlatformId, x.Serial}).ToList());
            memoryDb.Commit(mameFilenames.ToList());
            memoryDb.SaveTo(diskDb); // todo fix online backup API
        }
    }
}
