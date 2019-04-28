using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Stone.FileSignatures.Formats.CDXA
{
    public class PlaystationDisk
    {
        private readonly CDXADisk disk;

        public PlaystationDisk(CDXADisk disk)
        {
            this.disk = disk;
        }

        public string InternalName => this.disk.VolumeDescriptor;

        public bool IsPlaystation()
        {
            byte[] buf = new byte[64];
            using (Stream block = this.disk.OpenBlock(4))
            {
                block.Read(buf, 0, buf.Length);
            }

            bool psHeader = Encoding.UTF8.GetString(buf).Contains("Sony Computer Entertainment");
            string syscnf = this.GetSystemCnf();
            if (syscnf == null) return false;
            string exe = syscnf.Substring(14, 11);
            if (!this.disk.Files.ContainsKey(exe)) return false;
            var file = this.disk.Files[exe];
            byte[] exebuf = new byte[8];
            using var exeFile = file.OpenFile();
            exeFile.Read(exebuf, 0, 8);
            string exeHeader = Encoding.UTF8.GetString(exebuf);
            return psHeader && (exeHeader == "PS-X EXE" || exeHeader == "CPE");
        }

        public string GetSystemCnf()
        {
            if (!this.disk.Files.ContainsKey("SYSTEM.CNF")) return null;
            var file = this.disk.Files["SYSTEM.CNF"];
            byte[] buf = new byte[file.Length];

            using (Stream block = file.OpenFile())
            using (MemoryStream cnfMemory = new MemoryStream())
            {
                block.CopyTo(cnfMemory);
                return Encoding.UTF8.GetString(cnfMemory.ToArray());
            }
        }
    }
}
