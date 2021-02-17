using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration.Generators
{
    [Generator]
    public sealed class ConfigurationSectionGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            if (!(context.SyntaxReceiver is SyntaxReceiver receiver))
                return;
            var compilation = context.Compilation;
            CSharpParseOptions options = (compilation as CSharpCompilation).SyntaxTrees[0].Options as CSharpParseOptions;
            INamedTypeSymbol configSectionAttr = compilation.GetTypeByMetadataName("Snowflake.Configuration.Attributes.ConfigurationSectionAttribute");
            INamedTypeSymbol configOptionAttr = compilation.GetTypeByMetadataName("Snowflake.Configuration.Attributes.ConfigurationOptionAttribute");

            INamedTypeSymbol configSectionInterface = compilation.GetTypeByMetadataName("Snowflake.Configuration.IConfigurationSection");
            INamedTypeSymbol configSectionGenericInterface = compilation.GetTypeByMetadataName("Snowflake.Configuration.IConfigurationSection`1");
            INamedTypeSymbol configInstanceAttr = compilation.GetTypeByMetadataName("Snowflake.Configuration.Generators.ConfigurationGenerationInstanceAttribute");
            List<IPropertySymbol> symbols = new();
            foreach (var prop in receiver.CandidateProperties)
            {
                SemanticModel model = compilation.GetSemanticModel(prop.SyntaxTree);
                var propSymbol = model.GetDeclaredSymbol(prop);
                if (!propSymbol.ContainingType.GetAttributes()
                    .Any(attr => attr.AttributeClass.Equals(configSectionAttr, SymbolEqualityComparer.Default))
                    && propSymbol.ContainingType.TypeKind != TypeKind.Interface)
                {
                    continue;
                }

                var attrs = propSymbol.GetAttributes().Where(attr => attr.AttributeClass.Equals(configOptionAttr, SymbolEqualityComparer.Default));
                if (attrs.Any())
                {
                    symbols.Add(propSymbol);
                }
            }

            foreach (IGrouping<INamedTypeSymbol, IPropertySymbol> group in symbols.GroupBy(f => f.ContainingType))
            {
                string classSource = ProcessClass(group.Key, group.ToList(), configSectionInterface, configSectionGenericInterface, configInstanceAttr, context);
                context.AddSource($"{group.Key.Name}_ConfigurationSection.cs", SourceText.From(classSource, Encoding.UTF8));
            }
        }

        public string ProcessClass(INamedTypeSymbol classSymbol, List<IPropertySymbol> props,

            INamedTypeSymbol configSectionInterface,
            INamedTypeSymbol configSectionGenericInterface,
            INamedTypeSymbol configInstanceAttr,
            GeneratorExecutionContext context)
        {
            if (!classSymbol.ContainingSymbol.Equals(classSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
            {
                return null; //TODO: issue a diagnostic that it must be top level
            }

            string namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
            string tag = RandomString(6);
            string backingClassName = $"{classSymbol.Name}Proxy_{tag}";
            StringBuilder source = new StringBuilder($@"
namespace {namespaceName}
{{
    using System.ComponentModel;

    [{configInstanceAttr.ToDisplayString()}(typeof({backingClassName}))]
    public partial interface {classSymbol.Name}
    {{
    
    }}

    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class {backingClassName} : {classSymbol.Name}
    {{
        readonly Snowflake.Configuration.IConfigurationSectionDescriptor __sectionDescriptor;
        readonly Snowflake.Configuration.IConfigurationValueCollection __backingCollection;

        public {backingClassName}(Snowflake.Configuration.IConfigurationSectionDescriptor sectionDescriptor, Snowflake.Configuration.IConfigurationValueCollection collection) 
        {{
            this.__sectionDescriptor = sectionDescriptor;
            this.__backingCollection = collection;
        }}
");

            foreach (var prop in props)
            {
                source.Append($@"
{prop.Type.ToDisplayString()} {classSymbol.Name}.{prop.Name}
{{
    get {{ return ({prop.Type.ToDisplayString()})this.__backingCollection[this.__sectionDescriptor, nameof({prop.ToDisplayString()})]?.Value; }}
    set {{ 
            var existingValue = this.__backingCollection[this.__sectionDescriptor, nameof({prop.ToDisplayString()})];
            if (existingValue != null && value != null) {{ existingValue.Value = value; }}
            if (existingValue != null && value == null && this.__sectionDescriptor[nameof({prop.ToDisplayString()})].Type == typeof(string)) 
            {{ existingValue.Value = this.__sectionDescriptor[nameof({prop.ToDisplayString()})].Unset; }}
        }}
}}
");
            }

            
            source.Append("}}");
            return source.ToString();
        }

        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public void Initialize(GeneratorInitializationContext context)
        {
#if DEBUG
            //if (!Debugger.IsAttached)
            //{
            //    Debugger.Launch();
            //}
#endif 
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }
    }

    /// <summary>
    /// Created on demand before each generation pass
    /// </summary>
    class SyntaxReceiver : ISyntaxReceiver
    {
        public List<PropertyDeclarationSyntax> CandidateProperties { get; } = new();

        /// <summary>
        /// Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for generation
        /// </summary>
        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is PropertyDeclarationSyntax propertyDeclarationSyntax
                && propertyDeclarationSyntax.AttributeLists.Count > 0
                && propertyDeclarationSyntax.AccessorList.ChildNodes().Count() == 2)
            {
                this.CandidateProperties.Add(propertyDeclarationSyntax);
            }
        }
    }
}
