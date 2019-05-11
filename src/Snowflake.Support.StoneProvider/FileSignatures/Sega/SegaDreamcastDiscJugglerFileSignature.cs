using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Snowflake.Extensibility;
using Snowflake.Romfile;
using Snowflake.Services;
using Snowflake.Stone.FileSignatures.Formats.CDI;

namespace Snowflake.Stone.FileSignatures.Sega
{
    internal sealed class SegaDreamcastDiscJugglerFileSignature : IFileSignature
    {
        /// <inheritdoc/>
        public byte[] HeaderSignature => Encoding.UTF8.GetBytes("SEGA SEGAKATANA SEGA ENTERPRISES");

        /// <inheritdoc/>
        public bool HeaderSignatureMatches(Stream romStream)
        {
            try
            {
                // https://github.com/discimagechef/DiscImageChef/blob/master/DiscImageChef.DiscImages/DiscJuggler/Identify.cs#L46
                romStream.Seek(-4, SeekOrigin.End);
                byte[] cdiDescriptorLength = new byte[4];
                romStream.Read(cdiDescriptorLength, 0, 4);
                int descriptorLength = BitConverter.ToInt32(cdiDescriptorLength, 0);

                if (descriptorLength >= romStream.Length) return false;

                byte[] descriptor = new byte[descriptorLength];
                romStream.Seek(-descriptorLength, SeekOrigin.End);
                romStream.Read(descriptor, 0, descriptorLength);

                // Sessions
                if (descriptor[0] > 99 || descriptor[0] == 0) return false;

                // Seems all sessions start with this data
                if (descriptor[1] != 0x00 || descriptor[3] != 0x00 || descriptor[4] != 0x00 || descriptor[5] != 0x00 ||
                   descriptor[6] != 0x00 || descriptor[7] != 0x00 || descriptor[8] != 0x00 || descriptor[9] != 0x00 ||
                   descriptor[10] != 0x01 || descriptor[11] != 0x00 || descriptor[12] != 0x00 || descriptor[13] != 0x00 ||
                   descriptor[14] != 0xFF || descriptor[15] != 0xFF) return false;

                // Too many tracks
                if (descriptor[2] > 99) return false;

                romStream.Seek(0, SeekOrigin.Begin);
                CdiDreamcastDisc disc = new CdiDreamcastDisc(new DiscJugglerDisc(romStream));
                return disc.GetMeta().StartsWith("SEGA SEGAKATANA SEGA ENTERPRISES");
            } catch
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public string GetSerial(Stream romStream)
        {
            CdiDreamcastDisc disc = new CdiDreamcastDisc(new DiscJugglerDisc(romStream));
            return disc.GetMeta().Substring(0x40, 10).Trim();
            /*
            header = 0x0 SEGA SEGAKATANA for 0x10 bytes (0xF bytes without 0x20 space)
            internal name = 0x80 for 0x80 bytes
            serial number (id) = 0x40 for 10 bytes
            */
        }

        /// <inheritdoc/>
        public string GetInternalName(Stream romStream)
        {
            CdiDreamcastDisc disc = new CdiDreamcastDisc(new DiscJugglerDisc(romStream));
            return disc.GetMeta().Substring(0x80, 0x7f).Trim();
            /*
            header = 0x0 SEGA SEGAKATANA for 0x10 bytes (0xF bytes without 0x20 space)
            internal name = 0x80 for 0x80 bytes
            serial number (id) = 0x40 for 9 bytes
            */
        }
    }
}
