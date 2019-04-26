using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Snowflake.Support.Scraping.Primitives.Utility
{
    public static class StringExtensions
    {
        /// <summary>
        /// Normalizes a title string using certain rules
        /// </summary>
        public static string NormalizeTitle(this string input)
        {
            var normalized = string.Join(" ", input.Trim().ToUpperInvariant().Split(' ', ':', '-')
                    .Where(w => !string.IsNullOrWhiteSpace(w)).ToArray())
                .Replace("&", "AND")
                .Replace("!", string.Empty)
                .Replace("\"", string.Empty)
                .Replace("$", string.Empty)
                .Replace("'", string.Empty)
                .Replace("(", string.Empty)
                .Replace(")", string.Empty)
                .Replace(",", string.Empty)
                .Replace("?", string.Empty)
                .Trim()
                .RemoveDiacritics();
            return normalized;
        }

        private static string RemoveDiacritics(this string str)
        {
            if (str == null)
            {
                return null;
            }

            var chars =
                from c in str.Normalize(NormalizationForm.FormD).ToCharArray()
                let uc = CharUnicodeInfo.GetUnicodeCategory(c)
                where uc != UnicodeCategory.NonSpacingMark
                select c;

            var cleanStr = new string(chars.ToArray()).Normalize(NormalizationForm.FormC);

            return cleanStr;
        }
    }
}
