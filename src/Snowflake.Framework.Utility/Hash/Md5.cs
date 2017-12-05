using System;
using System.IO;

namespace Snowflake.Utility.Hash
{
    internal class MD5
    {
        public static string GetHash(Stream file)
        {
            file.Seek(0, SeekOrigin.Begin);
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                return md5.ComputeHash(file).ToHex(true).Replace("-", string.Empty);
            }
        }
    }
}
