using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Snowflake.Utility.Hash
{
    internal class SHA1
    {
        public static string GetHash(FileStream file)
        {
            using (var sha1 = System.Security.Cryptography.SHA1.Create())
                return BitConverter.ToString(sha1.ComputeHash(file)).Replace("-", String.Empty).ToLowerInvariant();

        }
    }
}
