using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Stone.FileSignatures.Formats.CDXA
{
    /// <summary>
    /// Represents a Playstation Disc from a raw CDXA image.
    /// </summary>
    internal class PlaystationDisc
    {
        private readonly CDXADisc disk;

        /// <summary>
        /// Creates a PlaystationDisc from the <see cref="CDXADisc"/>
        /// </summary>
        /// <param name="disk">The <see cref="CDXADisc"/> stream reader to get from.</param>
        public PlaystationDisc(CDXADisc disk)
        {
            this.disk = disk;
        }

        /// <summary>
        /// Returns the volume descriptor of the disc.
        /// </summary>
        public string InternalName => this.disk.VolumeDescriptor;

        /// <summary>
        /// Checks for the existence of a PS-EXE or CPE executable at the root, and the
        /// Sony Computer Entertainment anti-piracy string.
        /// </summary>
        /// <returns>Whether or not this is a valid Playstation Disc.</returns>
        public bool IsPlaystation()
        {
            byte[] buf = new byte[64];
            using (Stream block = this.disk.OpenBlock(4))
            {
                block.Read(buf, 0, buf.Length);
            }

            bool psHeader = Encoding.UTF8.GetString(buf).Contains("Sony Computer Entertainment");
            string syscnf = this.GetMeta();
            if (syscnf == null) return false;
            string exe = syscnf.Substring(14, 11);
            byte[] exebuf = new byte[8];
            using var exeFile = this.disk.OpenFile(exe);
            exeFile.Read(exebuf, 0, 8);
            string exeHeader = Encoding.UTF8.GetString(exebuf);
            return psHeader && (exeHeader == "PS-X EXE" || exeHeader == "CPE");
        }

        /// <summary>
        /// Gets the SYSTEM.CNF file from the disc if it exists.
        /// </summary>
        /// <returns>Returns the contents of the SYSTEM.CNF file if it exists, or null otherwise.</returns>
        public string GetMeta()
        {
            if (!this.disk.Files.ContainsKey("SYSTEM.CNF")) return null;
            var file = this.disk.Files["SYSTEM.CNF"];
            byte[] buf = new byte[file.Length];

            using Stream block = file.OpenFile();
            using MemoryStream cnfMemory = new MemoryStream();
            block.CopyTo(cnfMemory);
            return Encoding.UTF8.GetString(cnfMemory.ToArray());
        }
    }
}
