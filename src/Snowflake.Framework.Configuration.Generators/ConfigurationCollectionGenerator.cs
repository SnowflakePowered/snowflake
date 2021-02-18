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
    public sealed class ConfigurationCollectionGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            if (!(context.SyntaxReceiver is PropertyAttributeSyntaxReceiver receiver))
                return;
            var compilation = context.Compilation;
            CSharpParseOptions options = (compilation as CSharpCompilation).SyntaxTrees[0].Options as CSharpParseOptions;
            INamedTypeSymbol configTargetAttribute = compilation.GetTypeByMetadataName("Snowflake.Configuration.Attributes.ConfigurationTargetAttribute");
            INamedTypeSymbol configTargetMember = compilation.GetTypeByMetadataName("Snowflake.Configuration.Attributes.ConfigurationTargetMemberAttribute");

            INamedTypeSymbol configCollectionInterface = compilation.GetTypeByMetadataName("Snowflake.Configuration.IConfigurationCollection");
            INamedTypeSymbol configSectionGenericInterface = compilation.GetTypeByMetadataName("Snowflake.Configuration.IConfigurationCollection`1");
            INamedTypeSymbol configInstanceAttr = compilation.GetTypeByMetadataName("Snowflake.Configuration.Generators.ConfigurationGenerationInstanceAttribute");
            List<IPropertySymbol> symbols = new();
            foreach (var prop in receiver.CandidateProperties)
            {
                SemanticModel model = compilation.GetSemanticModel(prop.SyntaxTree);
                var propSymbol = model.GetDeclaredSymbol(prop);
                if (!propSymbol.ContainingType.GetAttributes()
                    .Any(attr => attr.AttributeClass.Equals(configTargetAttribute, SymbolEqualityComparer.Default))
                    && propSymbol.ContainingType.TypeKind != TypeKind.Interface)
                {
                    continue;
                }

                var attrs = propSymbol.GetAttributes().Where(attr => attr.AttributeClass.Equals(configTargetMember, SymbolEqualityComparer.Default));
                if (attrs.Any())
                {
                    symbols.Add(propSymbol);
                }
            }

            foreach (IGrouping<INamedTypeSymbol, IPropertySymbol> group in symbols.GroupBy(f => f.ContainingType))
            {
                string classSource = ProcessClass(group.Key, group.ToList(), configCollectionInterface, configSectionGenericInterface, configInstanceAttr, context);
                context.AddSource($"{group.Key.Name}_ConfigurationCollection.cs", SourceText.From(classSource, Encoding.UTF8));
            }
        }

        public string ProcessClass(INamedTypeSymbol classSymbol, List<IPropertySymbol> props,

            INamedTypeSymbol configCollectionInterface,
            INamedTypeSymbol configCollectionGenericInterface,
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
    sealed class {backingClassName} : {classSymbol.Name}
    {{
        readonly Snowflake.Configuration.IConfigurationValueCollection __backingCollection;
        private {backingClassName}(Snowflake.Configuration.IConfigurationValueCollection collection) 
        {{
            this.__backingCollection = collection;
        
");
            foreach (var prop in props)
            {
                source.Append($@"
 this.backing__{prop.Name} = new Snowflake.Configuration.ConfigurationSection<{prop.Type.ToDisplayString()}>(this.__backingCollection, ""{prop.Name}""); 
");
            }

            source.Append("}");
            foreach (var prop in props)
            {
                source.Append($@"
private Snowflake.Configuration.ConfigurationSection<{prop.Type.ToDisplayString()}> backing__{prop.Name};
{prop.Type.ToDisplayString()} {classSymbol.Name}.{prop.Name}
{{
    get {{ return this.backing__{prop.Name}.Configuration; }}
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
            context.RegisterForSyntaxNotifications(() => new PropertyAttributeSyntaxReceiver(1));
        }
    }
}
