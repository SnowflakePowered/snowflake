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
            return Regex.Match(this.filename, @"(\([^]]*\))*(\[[^]]*\])*([\w\+\~\@\!\#\$\%\^\&\*\;\,\'\""\?\-\.\-\s]+)").Groups[3].Value.Trim();
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
