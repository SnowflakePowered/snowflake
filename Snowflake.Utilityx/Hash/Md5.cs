using System;
using System.IO;

namespace Snowflake.Utility.Hash
{
    internal class MD5
    {
        public static string GetHash(FileStream file)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
                return BitConverter.ToString(md5.ComputeHash(file)).Replace("-", string.Empty).ToLowerInvariant();

        }
    }
}
