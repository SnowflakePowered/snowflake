using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using Snowflake.Romfile.Tokenizer;


namespace Snowflake.Romfile
{
    public sealed class StructuredFilename : IStructuredFilename
    {
        /// <inheritdoc/>
        public NamingConvention NamingConvention { get; private set; }

        /// <inheritdoc/>
        public string RegionCode { get; }

        /// <inheritdoc/>
        public string Title { get; }

        /// <inheritdoc/>
        public string Year { get; }

        /// <inheritdoc/>
        public string OriginalFilename { get; }

        public StructuredFilename(string originalFilename)
        {
            this.OriginalFilename = Path.GetFileName(originalFilename);

            // todo: expose tokens to api
            (NamingConvention namingConvention, IEnumerable<StructuredFilenameToken> tokens) = GetBestMatch();
            this.Title = Path.GetFileNameWithoutExtension(StructuredFilename.ParseTitle(tokens.FirstOrDefault(t => t.Type ==
                FieldType.Title)?.Value ?? "Unknown??!?"));
            this.NamingConvention = namingConvention;
            this.RegionCode = string.Join('-', tokens.Where(t => t.Type == FieldType.Country).Select(t => t.Value));
            if (string.IsNullOrEmpty(this.RegionCode))
            {
                this.RegionCode = "ZZ";
            }

            this.Year = tokens.FirstOrDefault(t => t.Type == FieldType.Date)?.Value.Split("-")[0] ?? "XXXX";
        }

        private (NamingConvention namingConvention, IEnumerable<StructuredFilenameToken> tokens) GetBestMatch()
        {
            var tokens = new StructuredFilenameTokenizer(this.OriginalFilename);
            var brackets = tokens.GetBracketTokens().ToList();
            var parens = tokens.GetParensTokens().ToList();
            var title = tokens.GetTitle();

            var goodTools = new GoodToolsTokenClassifier();
            var goodToolsTokens = goodTools.ClassifyBracketsTokens(brackets)
                .Concat(goodTools.ClassifyParensTokens(parens))
                .Concat(goodTools.ExtractTitleTokens(title)).ToList();

            var noIntro = new NoIntroTokenClassifier();
            var noIntroTokens = noIntro.ClassifyBracketsTokens(brackets)
              .Concat(noIntro.ClassifyParensTokens(parens))
              .Concat(noIntro.ExtractTitleTokens(title)).ToList();

            var tosec = new TosecTokenClassifier();
            var tosecTokens = tosec.ClassifyBracketsTokens(brackets)
              .Concat(tosec.ClassifyParensTokens(parens))
              .Concat(tosec.ExtractTitleTokens(title)).ToList();

            var aggregate = new List<(IEnumerable<StructuredFilenameToken> tokens, int uniqueDatatypes)>()
            {
                { (goodToolsTokens, StructuredFilename.GetUniqueDatatypeCount(goodToolsTokens)) },
                { (noIntroTokens, StructuredFilename.GetUniqueDatatypeCount(noIntroTokens)) },
                { (tosecTokens, StructuredFilename.GetUniqueDatatypeCount(tosecTokens)) },
            };

            var bestMatch = aggregate.OrderByDescending(p => p.uniqueDatatypes).First().tokens;
            return (bestMatch.First().NamingConvention, bestMatch);
        }

        private static int GetUniqueDatatypeCount(IEnumerable<StructuredFilenameToken> tokens)
        {
            return tokens.Select(t => t.Type).Distinct().Count();
        }

        private static string ParseTitle(string rawTitle)
        {
            return rawTitle.WithoutLastArticle("The")
                .WithoutLastArticle("A")
                .WithoutLastArticle("Die")
                .WithoutLastArticle("De")
                .WithoutLastArticle("La")
                .WithoutLastArticle("Le")
                .WithoutLastArticle("Les");
        }
    }

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

        public static string ToTitleCase(this string str)
        {
            var tokens = str.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < tokens.Length; i++)
            {
                var token = tokens[i];
                tokens[i] = token.Substring(0, 1).ToUpper() + token.Substring(1);
            }

            return string.Join(" ", tokens);
        }
    }
}
