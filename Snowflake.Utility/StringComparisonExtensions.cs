using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Snowflake.Utility
{
    public static class StringComparisonExtensions
    {

        /// <summary>
        /// Normalizes a title string using certain rules
        /// </summary>
        private static string NormalizeTitle(this string input)
        {
            var normalized = String.Join(" ", input.Trim().ToUpperInvariant().Split(' ', ':', '-')
                .Where(w => !String.IsNullOrWhiteSpace(w)).ToArray())
                 .Replace("&", "AND")
                    .Replace("!", "")
                    .Replace("\"", "")
                    .Replace("$", "")
                    .Replace("'", "")
                    .Replace("(", "")
                    .Replace(")", "")
                    .Replace(",", "")
                    .Replace("?", "")
                    .Trim()
                    .RemoveDiacritics();
            return normalized;
        }

        private static string RemoveDiacritics(this string str)
        {
            if (str == null) return null;
            var chars =
                from c in str.Normalize(NormalizationForm.FormD).ToCharArray()
                let uc = CharUnicodeInfo.GetUnicodeCategory(c)
                where uc != UnicodeCategory.NonSpacingMark
                select c;

            var cleanStr = new string(chars.ToArray()).Normalize(NormalizationForm.FormC);

            return cleanStr;
        }
        public static int CompareTitle(this string s, string t) => s.NormalizeTitle().Levenshtein(t.NormalizeTitle());

        /// <summary>
        /// Compute the distance between two strings.
        /// </summary>
        public static int Levenshtein(this string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            var d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++);

            for (int j = 0; j <= m; d[0, j] = j++);

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }
    }
}
