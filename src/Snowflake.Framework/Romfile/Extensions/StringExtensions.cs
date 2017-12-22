using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Romfile.Extensions
{
    public static class StringExtensions
    {
        public static string WithoutLastArticle(this string title, string article)
        {
            if (!title.Contains($", {article}"))
            {
                return title;
            }

            string[] titleWithoutArticle = title.Split($", {article}");
            return string.Join(string.Empty, titleWithoutArticle.Prepend(article + " "));
        }

        private static readonly string[] Articles = new string[] { "the", "from", "a", "of", "to", "an", "in", "at", "for", "by", "or", "but", "on", };
        public static string ToTitleCase(this string str)
        {
            var tokens = str.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < tokens.Length; i++)
            {
                var token = tokens[i];

                // Always capitalize the first word, but ignore articles otherwise.
                if (i == 0 || !Articles.Contains(token, StringComparer.OrdinalIgnoreCase))
                {
                    tokens[i] = token.Substring(0, 1).ToUpper() + token.Substring(1);
                }
            }

            return string.Join(" ", tokens);
        }
    }
}
