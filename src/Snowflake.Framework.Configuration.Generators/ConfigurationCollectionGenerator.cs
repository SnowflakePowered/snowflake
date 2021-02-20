using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Configuration.Generators
{
    [Generator]
    public sealed class ConfigurationCollectionGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is not ConfigurationTemplateInterfaceSyntaxReceiver receiver)
                return;
            bool errorOccured = false;
            var compilation = context.Compilation;
            CSharpParseOptions options = (compilation as CSharpCompilation).SyntaxTrees[0].Options as CSharpParseOptions;
            INamedTypeSymbol configTargetAttribute = compilation.GetTypeByMetadataName("Snowflake.Configuration.Attributes.ConfigurationTargetAttribute");
            INamedTypeSymbol configTargetMember = compilation.GetTypeByMetadataName("Snowflake.Configuration.Attributes.ConfigurationTargetMemberAttribute");

            INamedTypeSymbol configCollectionInterface = compilation.GetTypeByMetadataName("Snowflake.Configuration.IConfigurationCollection");
            INamedTypeSymbol configSectionGenericInterface = compilation.GetTypeByMetadataName("Snowflake.Configuration.IConfigurationCollection`1");
            INamedTypeSymbol configInstanceAttr = compilation.GetTypeByMetadataName("Snowflake.Configuration.Generators.ConfigurationGenerationInstanceAttribute");
            List<IPropertySymbol> symbols = new();

            foreach (var iface in receiver.CandidateInterfaces)
            {
                var model = compilation.GetSemanticModel(iface.SyntaxTree);
                var ifaceSymbol = model.GetDeclaredSymbol(iface);
                var memberSyntax = iface.Members;
                
                if (memberSyntax.FirstOrDefault(m => m is not PropertyDeclarationSyntax) is MemberDeclarationSyntax badSyntax)
                {
                    var badSymbol = model.GetDeclaredSymbol(badSyntax);
                    context.ReportError(1004, "Invalid members in template interface.", 
                        $"Template interface '{ifaceSymbol.Name}' must only declare property members. " +
                        $"{badSymbol.Kind} '{ifaceSymbol.Name}.{badSymbol?.Name}' is not a property.",
                        badSyntax.GetLocation(), ref errorOccured);
                    continue;
                }

                if (!iface.Modifiers.Any(p => p.IsKind(SyntaxKind.PartialKeyword)))
                {
                    context.ReportError(1001,
                               "Unextendible template interface",
                               $"Template interface '{ifaceSymbol.Name}' must be marked partial.",
                               iface.GetLocation(), ref errorOccured);
                    continue;
                }

                foreach (var prop in memberSyntax.Cast<PropertyDeclarationSyntax>())
                {
                    var propSymbol = model.GetDeclaredSymbol(prop);
                    
                    // todo: error check prop
                    // (only getter? idk, what are restritions on configcollections?)

                    var attrs = propSymbol.GetAttributes()
                        .Where(attr => attr.AttributeClass.Equals(configTargetMember, SymbolEqualityComparer.Default));
                    if (attrs.Any())
                    {
                        symbols.Add(propSymbol);
                    }
                }
            }

            if (errorOccured)
                return;

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
            string generatedNamespaceName = $"Snowflake.Configuration.GeneratedConfigurationProxies.Collection_{namespaceName}";
            string tag = RandomString(6);
            string backingClassName = $"{classSymbol.Name}Proxy_{tag}";
            StringBuilder source = new StringBuilder($@"
namespace {namespaceName}
{{
    [{configInstanceAttr.ToDisplayString()}(typeof({generatedNamespaceName}.{backingClassName}))]
    public partial interface {classSymbol.Name}
    {{
    
    }}
}}

namespace {generatedNamespaceName}
{{
    using System.ComponentModel;

    [EditorBrowsable(EditorBrowsableState.Never)]
    sealed class {backingClassName} : {classSymbol.ToDisplayString()}
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
{prop.Type.ToDisplayString()} {classSymbol.ToDisplayString()}.{prop.Name}
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
            // todo: explicit configuration collection attribute
            context.RegisterForSyntaxNotifications(() => new ConfigurationTemplateInterfaceSyntaxReceiver("ConfigurationTarget"));
        }
    }
}
