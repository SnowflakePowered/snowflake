using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Configuration.Generators
{
    internal class PropertyAttributeSyntaxReceiver : ISyntaxReceiver
    {
        public PropertyAttributeSyntaxReceiver(int childNodes)
        {
            this.ChildNodes = childNodes;
        }
        public List<PropertyDeclarationSyntax> CandidateProperties { get; } = new();
        private int ChildNodes { get; }

        /// <summary>
        /// Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for generation
        /// </summary>
        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is PropertyDeclarationSyntax propertyDeclarationSyntax
                && propertyDeclarationSyntax.AttributeLists.Count > 0
                && propertyDeclarationSyntax.AccessorList.ChildNodes().Count() == this.ChildNodes)
            {
                this.CandidateProperties.Add(propertyDeclarationSyntax);
            }
        }
    }
}
