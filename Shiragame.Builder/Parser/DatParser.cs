using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Service;

namespace Shiragame.Builder.Parser
{
    internal abstract class DatParser
    {
        protected static readonly IStoneProvider stoneProvider = new StoneProvider();

        internal ParserClass GetParser(string firstLine)
        {
            if (firstLine.Contains("TITLES = http://www.gametdb.com")) return ParserClass.Tdb;
            if (firstLine.Contains("clrmamepro")) return ParserClass.Cmp;
            if (firstLine.Contains("<?xml version=\"1.0\" encoding=\"UTF-8\"?>")) return ParserClass.Xml;
            return ParserClass.None;
        }
    }

    internal enum ParserClass
    {
        None,
        Cmp, //clearmamepro
        Tdb, //gametdb
        Xml //xml logiqx
    }
}
