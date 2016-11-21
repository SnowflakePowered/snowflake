using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Snowflake.Romfile;
using Snowflake.Services;

namespace Shiragame.Builder.Parser
{
    internal class GameTdbParser : DatParser
    {

        /// <summary>
        /// Parse gametdb dat files
        /// </summary>
        /// <param name="datFile">Path to dat file</param>
        /// <param name="platformId">Platform of dat file</param>
        /// <returns>The information records contained in the file</returns>
        internal static IEnumerable<SerialInfo> ParseSerials(string datFile, string platformId)
        {
            var lines = File.ReadAllLines(datFile);
            return from line in lines.AsParallel() select GameTdbParser.GetSerial(line, platformId);
        }

        private static SerialInfo GetSerial(string tdbLine, string platformId)
        {
            var line = tdbLine.Split('=').Select(x => x.Trim()).ToList();
            string serial = line[0];

            string region = "ZZ";
            if(platformId.StartsWith("NINTENDO")) region = GameTdbParser.GetNintendoRegionCode(serial);
            if(platformId.StartsWith("SONY")) region = GameTdbParser.GetSonyRegionCode(serial);
            string name = line[1];
            return new SerialInfo(platformId, name, region, serial);
        }

        private static string GetSonyRegionCode(string serial)
        {
            if (serial.StartsWith("A")) return "AS";
            char region = serial[2];
            switch (region)
            {
                case 'A': //asia
                    return "AS";
                case 'E': //europe
                    return "EU";
                case 'H': //southeast asia
                    return "AS";
                case 'K': //hong kong
                    return "HK";
                case 'J':
                    return "JP";
                case 'U':
                    return "US";
                default:
                    return "ZZ";
            }
        }
        private static string GetNintendoRegionCode(string serial)
        {
            char region = serial[3];
            switch (region)
            {
                case 'D': //germany
                    return "DE";
                case 'E': //usa
                case 'N': //jp import to usa
                    return "US";
                case 'F':
                    return "FR";
                case 'I':
                    return "IT";
                case 'J':
                    return "JP";
                case 'Q': //kr with japanese lang
                case 'K':
                    return "KR";
                case 'P': //pal 
                case 'L':
                case 'M': //jp import to eu
                    return "EU";
                case 'R':
                    return "RU";
                case 'S':
                    return "ES";
                case 'T': 
                    return "TW";
                case 'U': //u for australia
                    return "AU";
                case 'C':
                    return "CN";
                default:
                    return "ZZ";
            }
        }


    }
}
