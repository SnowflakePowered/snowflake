using System;
using System.IO;

namespace Snowflake.Utility.Hash
{
    internal class SHA1
    {
        public static string GetHash(Stream file)
        {
            using (var sha1 = System.Security.Cryptography.SHA1.Create())
                return BitConverter.ToString(sha1.ComputeHash(file)).Replace("-", string.Empty).ToLowerInvariant();

        }
    }
}
