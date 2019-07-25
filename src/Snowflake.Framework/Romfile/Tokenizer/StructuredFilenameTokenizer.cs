using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Snowflake.Romfile.Tokenizer
{
    internal class StructuredFilenameTokenizer
    {
        private readonly string filename;

        public StructuredFilenameTokenizer(string filename)
        {
            this.filename = filename;
        }

        public string GetTitle()
        {
            return Regex.Match(WithoutFileExtension(this.filename), @"(\([^]]*\))*(\[[^]]*\])*([\w\+\~\@\!\#\$\%\^\&\*\;\,\'\""\?\-\.\-\s]+)")
                .Groups[3].Value.Trim();
        }

        private static string WithoutFileExtension(string rawTitle)
        {
            int index = rawTitle.LastIndexOf('.');
            if (index < 0) return rawTitle;
            string ext = rawTitle[(index + 1)..];
            if (String.IsNullOrWhiteSpace(ext)) return rawTitle; // if whats after the period is whitespace
            if (ext.StartsWith(' ')) return rawTitle; // if whats after the period is a space
            if (!Regex.IsMatch(ext, "^[a-zA-Z0-9]*$")) return rawTitle; // if the extension is not alphanumeric
            if (ext.Contains(' ')) return rawTitle; // if there is whitespace after the period
            return rawTitle[0..index];
        }

        public IEnumerable<(string tokenValue, int tokenPosition)> GetParensTokens()
        {
            var tagData = Regex.Matches(this.filename, @"(\()([^)]+)(\))").ToList();
            for (int i = 0; i < tagData.Count; i++)
            {
                yield return (tagData[i].Groups[2].Value, i);
            }
        }

        public IEnumerable<(string tokenValue, int tokenPosition)> GetBracketTokens()
        {
            var tagData = Regex.Matches(this.filename, @"(\[)([^)]+)(\])").ToList();
            for (int i = 0; i < tagData.Count; i++)
            {
                yield return (tagData[i].Groups[2].Value, i);
            }
        }
    }
}
