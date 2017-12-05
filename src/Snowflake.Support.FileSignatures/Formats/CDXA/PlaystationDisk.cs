using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Romfile.FileSignatures.Formats.CDXA
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

            return Encoding.UTF8.GetString(buf).Contains("Sony Computer Entertainment");
        }

        public string GetSystemCnf()
        {
            var file = this.disk.Files["SYSTEM.CNF"];
            byte[] buf = new byte[file.Length];
            using (Stream block = this.disk.OpenBlock(file.Lba))
            {
                block.Read(buf, 0, buf.Length);
            }

            return Encoding.UTF8.GetString(buf);
        }
    }
}
