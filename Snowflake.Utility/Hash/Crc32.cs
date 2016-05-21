using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Crc32;
namespace Snowflake.Utility.Hash
{

    internal sealed class Crc32 
    {
        public static string GetHash(Stream file)
        {
            using (var crc32 = new Crc32Algorithm())
                return BitConverter.ToString(crc32.ComputeHash(file)).Replace("-", string.Empty).ToLowerInvariant();

        }
    }
}