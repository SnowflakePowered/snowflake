using System.Collections.Generic;

namespace Snowflake.Romfile.Tokenizer
{
    internal interface ITokenClassifier
    {
        IEnumerable<StructuredFilenameToken>
            ClassifyParensTokens(IEnumerable<(string tokenValue, int tokenPosition)> tokens);

        IEnumerable<StructuredFilenameToken>
            ClassifyBracketsTokens(IEnumerable<(string tokenValue, int tokenPosition)> tokens);

        IEnumerable<StructuredFilenameToken>
            ExtractTitleTokens(string title);
    }
}
