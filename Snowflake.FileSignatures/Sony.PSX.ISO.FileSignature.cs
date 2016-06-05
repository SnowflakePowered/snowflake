using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Snowflake.Extensibility;
using Snowflake.Romfile;
using Snowflake.Service;

namespace Snowflake.FileSignatures
{
    [Plugin("SnowflakeFileSignature-SONY_PSX_ISO")]
    public sealed class SonyPSXISOFileSignature : FileSignature
    {
        public SonyPSXISOFileSignature(ICoreService coreInstance)
            : base(coreInstance)
        {
            this.HeaderSignature = Encoding.UTF8.GetBytes("PLAYSTATION");

        }


        public override byte[] HeaderSignature { get; }

        public override bool HeaderSignatureMatches(Stream romStream)
        {
            byte[] buffer = new byte[1024 * 128]; // read the first 128 KiB
            romStream.Read(buffer, 0, buffer.Length);
            string code = Encoding.UTF8.GetString(buffer);
            return code.Contains(Encoding.UTF8.GetString(this.HeaderSignature)); //perhaps we can be more surgical..
        }

        public override string GetSerial(Stream romStream)
        {
            byte[] buffer = new byte[1024 * 128]; // read the first 128 KiB
            romStream.Read(buffer, 0, buffer.Length);

            string raw =
                new String(
                    Encoding.UTF8.GetString(buffer)
                        .Replace("\0", "")
                        .Where(
                            c =>
                                char.IsLetter(c) || char.IsDigit(c) || char.IsPunctuation(c) || char.IsWhiteSpace(c))
                        .ToArray());
            string match = Regex.Match(raw,
            "(SLUS|SLES|SLPM|SLED|SLPS|SCUS|SCES|SCED|SCPS|ESPM|SIPS|PBPX|SLKA)_[0-9][0-9][0-9].[0-9][0-9];", RegexOptions.IgnoreCase).Value;
            return match.Replace(";", "").Replace(".", "");

        }
    }
}
