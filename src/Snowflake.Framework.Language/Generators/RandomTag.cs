using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Language.Generators
{
    public static class RandomTag
    {
        private static Random random = new Random();
        public static string Tag(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
