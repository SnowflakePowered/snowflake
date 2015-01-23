using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Snowflake.Utility.Hash;

namespace Snowflake.Utility
{
    public static class FileHash
    {

        public static string GetCRC32(string fileName)
        {
            return Crc32.GetHash(File.OpenRead(fileName));
        }
        public static string GetMD5(string fileName)
        {
            return MD5.GetHash(File.OpenRead(fileName));
        }
        public static string GetSHA1(string fileName)
        {
            return SHA1.GetHash(File.OpenRead(fileName));
        }
    }
}
