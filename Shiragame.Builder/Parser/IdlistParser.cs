using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shiragame.Builder.Parser
{
    public class IdlistParser
    {
        /// <summary>
        /// Parse idlist
        /// </summary>
        /// <param name="datFile">Path to dat file</param>
        /// <param name="platformId">Platform of dat file</param>
        /// <returns>The information records contained in the file</returns>
        internal static IEnumerable<SerialInfo> ParseSerials(string datFile, string platformId)
        {
            var lines = File.ReadAllLines(datFile);
            return from line in lines.AsParallel() select IdlistParser.GetSerial(line, platformId);
        }

        private static SerialInfo GetSerial(string idlist, string platformId)
        {
            var line = idlist.Split(new [] {' '}, 2);
            string serial = line[0];
            string name = line[1].Trim('"');
            string region = IdlistParser.GetSonyRegionCode(serial);
            return new SerialInfo(platformId, name, region, serial);
        }

        private static string GetSonyRegionCode(string serial)
        {
            if (serial.StartsWith("A")) return "AS";
            char region = serial[2];
            switch (region)
            {
                case 'P': //asia
                    return "JP";
                case 'E': //europe
                    return "EU";
                case 'U': //usa
                    return "US";
                default:
                    return "ZZ";
            }
        }
    }
}
