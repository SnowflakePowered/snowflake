using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Snowflake.Language.Generators.Configuration
{
    internal class ConfigurationTemplateInterfaceSyntaxReceiver : ISyntaxReceiver
    {
        public ConfigurationTemplateInterfaceSyntaxReceiver(string attributeName)
        {
            this.AttributeName = attributeName;
        }
        public List<InterfaceDeclarationSyntax> CandidateInterfaces { get; } = new();
        private string AttributeName { get; }

        /// <summary>
        /// Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for generation
        /// </summary>
        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is InterfaceDeclarationSyntax interfaceDeclarationSyntax
                && interfaceDeclarationSyntax.AttributeLists.SelectMany(syntaxNode => syntaxNode.Attributes)
                    .Any(attr => attr.Name.ToFullString().EndsWith(this.AttributeName)))
            {
                this.CandidateInterfaces.Add(interfaceDeclarationSyntax);
            }

        }
    }
}
