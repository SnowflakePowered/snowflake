using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Services;

namespace Shiragame.Builder.Parser
{
    internal abstract class DatParser
    {
        protected static readonly IStoneProvider stoneProvider = new StoneProvider();

        internal static ParserClass GetParser(string firstLine)
        {
            if (firstLine.Contains("TITLES = http://www.gametdb.com"))
            {
                return ParserClass.Tdb;
            }

            if (firstLine.Contains("clrmamepro"))
            {
                return ParserClass.Cmp;
            }

            if (firstLine.Contains("xml version=\"1.0\""))
            {
                return ParserClass.Xml;
            }

            return ParserClass.None;
        }

        protected static string GetMimeType(string filename, string platformId)
        {
            filename = filename.ToLowerInvariant();
            return DatParser.stoneProvider.Platforms[platformId].FileTypes.ContainsKey(Path.GetExtension(filename))
                ? DatParser.stoneProvider.Platforms[platformId].FileTypes[Path.GetExtension(filename)]
                : null;
        }
    }

    internal enum ParserClass
    {
        None,
        Cmp, // clearmamepro
        Tdb, // gametdb
        Xml, // xml logiqx
    }
}
