using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DiscUtils.Iso9660;
using Snowflake.Romfile;
using Snowflake.Service;

namespace Snowflake.FileSignatures.SONY_PS2_ISO
{
    public sealed class SonyPS2ISOFileSignature : FileSignature
    {
        [ImportingConstructor]
        public SonyPS2ISOFileSignature([Import("coreInstance")] ICoreService coreInstance)
            : base(Assembly.GetExecutingAssembly(), coreInstance)
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
