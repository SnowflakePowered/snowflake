﻿using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using DiscUtils.Iso9660;
using Snowflake.Extensibility;
using Snowflake.Romfile;
using Snowflake.Service;

namespace Snowflake.FileSignatures
{
    [Plugin("SnowflakeFileSignature-SONY_PS2_ISO")]
    public sealed class SonyPS2ISOFileSignature : FileSignature
    {
        public SonyPS2ISOFileSignature(ICoreService coreInstance)
            : base(coreInstance)
        {

            this.HeaderSignature = Encoding.UTF8.GetBytes("BOOT2");

        }


        public override byte[] HeaderSignature { get; }

        public override bool HeaderSignatureMatches(string fileName)
        {
            try
            {
                using (FileStream isoStream = File.OpenRead(fileName))
                {
                    var reader = new CDReader(isoStream, true);
                    var system = reader.OpenFile("SYSTEM.CNF", FileMode.Open);
                    return new StreamReader(system).ReadToEnd().Contains("BOOT2");
                }
            }
            catch
            {
                return false;
            }
        }
        

        public override string GetGameId(string fileName)
        {
            try
            {
                using (FileStream isoStream = File.OpenRead(fileName))
                {
                    var reader = new CDReader(isoStream, true);
                    var system = reader.OpenFile("SYSTEM.CNF", FileMode.Open);
                    return Regex.Match(new StreamReader(system).ReadToEnd(), "[A-Z]+_[0-9][0-9][0-9].[0-9][0-9]",
                                    RegexOptions.IgnoreCase).Value.Replace(".", "");
                }
            }
            catch
            {
                return "";
            }
        }

    }
}
