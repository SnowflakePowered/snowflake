using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis.CodeFixes;
using System.Threading.Tasks;
using System.Composition;
using Snowflake.Language.CodeActions;
using Snowflake.Language.CodeActions.Fixes;
using Microsoft.CodeAnalysis.CSharp;

namespace Snowflake.Language.Analyzers.Configuration
{
    [ExportCodeFixProvider(LanguageNames.CSharp), Shared]
    public sealed class SectionPropertyMustHaveSetterFix
        : AbstractAddAccessorFix
    {
        public SectionPropertyMustHaveSetterFix() 
            : base(SyntaxKind.SetAccessorDeclaration)
        {
        }

        public override ImmutableArray<string> FixableDiagnosticIds 
            => ImmutableArray.Create(DiagnosticCodes.SFC015__CollectionPropertyMustHaveSetter);
    }
}
