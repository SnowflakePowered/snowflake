using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Snowflake.Utility
{
    public class MD5
    {

        public static string GetMD5(FileStream file)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
                return BitConverter.ToString(md5.ComputeHash(file)).Replace("-", String.Empty).ToLowerInvariant();

        }

        public static string GetMD5(string fileName)
        {
            return MD5.GetMD5(File.OpenRead(fileName));
        }
    }
}
