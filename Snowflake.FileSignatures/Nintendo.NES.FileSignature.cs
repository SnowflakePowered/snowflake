using System.IO;
using System.Linq;
using Snowflake.Extensibility;
using Snowflake.Romfile;
using Snowflake.Service;

namespace Snowflake.FileSignatures
{
    [Plugin("SnowflakeFileSignature-NINTENDO_NES")]
    public sealed class NintendoNESFileSignature : FileSignature
    {
        public NintendoNESFileSignature(ICoreService coreInstance)
            : base(coreInstance)
        {
            this.HeaderSignature = new byte[4] {0x4E, 0x45, 0x53, 0x1A}; //'N' 'E' 'S' <EOF>
        }

        public override byte[] HeaderSignature { get; }

        public override bool HeaderSignatureMatches(Stream romStream)
        {

            byte[] buffer = new byte[4];
            romStream.Read(buffer, 0, buffer.Length);
            return buffer.SequenceEqual(this.HeaderSignature);

        }
    }
}


