using System.IO;
using Snowflake.Utility.Hash;
namespace Snowflake.Utility
{
    public static class FileHash
    {
        public static string GetCRC32(string fileName)
        {
            return Hash.Crc32.GetHash(File.OpenRead(fileName));
        }

        public static string GetCRC32(Stream fileStream)
        {
            return Hash.Crc32.GetHash(fileStream);
        }

        public static string GetMD5(string fileName)
        {
            return MD5.GetHash(File.OpenRead(fileName));
        }

        public static string GetMD5(Stream fileStream)
        {
            return MD5.GetHash(fileStream);
        }

        public static string GetSHA1(string fileName)
        {
            return SHA1.GetHash(File.OpenRead(fileName));
        }

        public static string GetSHA1(Stream fileStream)
        {
            return SHA1.GetHash(fileStream);
        }
    }
}
