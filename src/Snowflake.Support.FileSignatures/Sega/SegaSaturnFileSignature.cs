﻿using System.IO;
using System.Linq;
using System.Text;
using Snowflake.Extensibility;
using Snowflake.Services;

namespace Snowflake.Romfile.FileSignatures.Sega
{
    public sealed class SegaSATFileSignature : IFileSignature
    {
        /// <inheritdoc/>
        public byte[] HeaderSignature => Encoding.UTF8.GetBytes("SEGA SEGASATURN ");

        /// <inheritdoc/>
        public bool HeaderSignatureMatches(Stream romStream)
        {
            romStream.Seek(0x10, SeekOrigin.Begin);
            byte[] buffer = new byte[16];
            romStream.Read(buffer, 0, buffer.Length);
            return buffer.SequenceEqual(this.HeaderSignature);
        }

        /// <inheritdoc/>
        public string GetSerial(Stream romStream)
        {
            romStream.Seek(0x30, SeekOrigin.Begin);
            byte[] buffer = new byte[7];
            romStream.Read(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer).Trim('\0').Trim();
        }

        /// <inheritdoc/>
        public string GetInternalName(Stream romStream)
        {
            romStream.Seek(0x70, SeekOrigin.Begin);
            byte[] buffer = new byte[0x70];
            romStream.Read(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer).Trim('\0').Trim();
        }
    }
}
