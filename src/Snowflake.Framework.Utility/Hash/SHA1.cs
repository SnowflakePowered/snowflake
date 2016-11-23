using System;
using System.IO;

namespace Snowflake.Utility.Hash
{
    internal class SHA1
    {
        public static string GetHash(Stream file)
        {
            file.Seek(0, SeekOrigin.Begin);
            using (var sha1 = System.Security.Cryptography.SHA1.Create())
                return sha1.ComputeHash(file).ToHex(true).Replace("-", string.Empty).ToUpperInvariant();

        }
    }
}
