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
using SFOSharp;
using Snowflake.Romfile;
using Snowflake.Service;

namespace Snowflake.FileSignatures.SONY_PSP_ISO
{
    public sealed class SonyPSPISOFileSignature : FileSignature
    {
        [ImportingConstructor]
        public SonyPSPISOFileSignature([Import("coreInstance")] ICoreService coreInstance)
            : base(Assembly.GetExecutingAssembly(), coreInstance)
        {

            this.HeaderSignature = Encoding.UTF8.GetBytes("PSP_GAME");

        }


        public override byte[] HeaderSignature { get; }

        public override bool HeaderSignatureMatches(string fileName)
        {
            try
            {
                using (FileStream isoStream = File.Open(fileName,
                        FileMode.Open))
                {
                    CDReader reader = new CDReader(isoStream, true);
                    return reader.DirectoryExists(Encoding.UTF8.GetString(this.HeaderSignature));
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
                using (
                    FileStream isoStream = File.Open(fileName,
                        FileMode.Open))
                {
                    CDReader reader = new CDReader(isoStream, true);
                    var system = reader.OpenFile(@"PSP_GAME\PARAM.SFO", FileMode.Open);
                    var sfo = new SFOReader(system);
                    return sfo.KeyValues.ContainsKey("DISC_ID") ? sfo.KeyValues["DISC_ID"] : sfo.KeyValues["TITLE_ID"];
                }
            }
            catch
            {
                return ""; 
            }
        }

        public override string GetInternalName(string fileName)
        {
            try
            {
                using (
                    FileStream isoStream = File.Open(fileName,
                        FileMode.Open))
                {
                    CDReader reader = new CDReader(isoStream, true);
                    var system = reader.OpenFile(@"PSP_GAME\PARAM.SFO", FileMode.Open);
                    var sfo = new SFOReader(system);
                    return sfo.KeyValues["TITLE"];
                }
            }
            catch
            {
                return "";
            }
        }
    }
}
