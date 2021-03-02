using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Snowflake.Generators.Analyzers;
using Snowflake.Generators.Configuration.Analyzers;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Snowflake.Configuration.Generators
{
    [Generator]
    public sealed class ConfigurationCollectionGenerator : ISourceGenerator
    {
        private static ImmutableArray<AbstractSyntaxNodeAnalyzer> Analyzers 
            = ImmutableArray.Create<AbstractSyntaxNodeAnalyzer>(
                new TemplateInterfaceTopLevelAnalyzer(),
                new UnextendibleInterfaceAnalyzer(),
                new CannotHideInheritedPropertyAnalyzer(),
                new InvalidTemplateAccessorAnalyzer(),
                new CollectionPropertyNotSectionAnalyzer(),
                new CollectionPropertyInvalidAccessorAnalyzer(),
                new CollectionPropertyMustHaveGetterAnalyzer(),
                new DuplicateConfigurationTargetAnalyzer()
            );

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is not ConfigurationTemplateInterfaceSyntaxReceiver receiver)
                return;
            bool errorOccurred = false;
            var compilation = context.Compilation;
            var types = new ConfigurationTypes(compilation);
            if (!types.CheckContext(context, ref errorOccurred))
                return;
            foreach (var ifaceSyntax in receiver.CandidateInterfaces)
            {
                var model = compilation.GetSemanticModel(ifaceSyntax.SyntaxTree);
                var ifaceSymbol = model.GetDeclaredSymbol(ifaceSyntax, context.CancellationToken);

                if (ifaceSymbol == null)
                    continue;

                if (!ifaceSymbol.GetAttributes()
                        .Any(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.ConfigurationCollectionAttribute)))
                    continue;

                var diagnostics = Analyzers.AsParallel()
                    .SelectMany(a => a.Analyze(context.Compilation, model, ifaceSyntax, context.CancellationToken))
                    .ToList();

                foreach(var diag in diagnostics)
                {
                    context.ReportDiagnostic(diag);
                }
                if (diagnostics.Any())
                    return;

                var properties = new List<(INamedTypeSymbol, IPropertySymbol)>();

                // Add collection props bottom up.
                foreach (var childIface in ifaceSymbol.AllInterfaces.Reverse().Concat(new[] { ifaceSymbol }))
                {
                    foreach (var member in childIface.GetMembers())
                    {
                        if (member is IPropertySymbol property)
                        {
                            properties.Add((childIface, property!));
                        }
                    }
                }
                string classSource = GenerateSource(ifaceSymbol, properties, types);
                context.AddSource($"{ifaceSymbol.Name}_ConfigurationCollection.cs", SourceText.From(classSource, Encoding.UTF8));
            }
        }

        private string GenerateSource(INamedTypeSymbol declaringInterface,
            List<(INamedTypeSymbol, IPropertySymbol)> properties,
            ConfigurationTypes types)
        {
            string namespaceName = declaringInterface.ContainingNamespace.ToDisplayString();
            string generatedNamespaceName = $"Snowflake.Configuration.GeneratedConfigurationProxies.Collection_{namespaceName}";
            string tag = RandomString(6);
            string backingClassName = $"{declaringInterface.Name}Proxy_{tag}";
            StringBuilder source = new StringBuilder($@"
namespace {namespaceName}
{{
    [{types.ConfigurationGenerationInstanceAttribute.ToDisplayString()}(typeof({generatedNamespaceName}.{backingClassName}))]
    public partial interface {declaringInterface.Name}
        : {types.IConfigurationCollectionTemplate.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}
    {{
    
    }}
}}

namespace {generatedNamespaceName}
{{
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.Collections.Immutable;

    [EditorBrowsable(EditorBrowsableState.Never)]
    sealed class {backingClassName} : {declaringInterface.ToDisplayString()}
    {{
        readonly {types.IConfigurationValueCollection.ToDisplayString()} __backingCollection;
        readonly Dictionary<string, {types.IConfigurationSection.ToDisplayString()}> __configurationSections; 

        IReadOnlyDictionary<string, {types.IConfigurationSection.ToDisplayString()}> 
            {types.IConfigurationCollectionTemplate.ToDisplayString()}.GetValueDictionary() 
                => ImmutableDictionary.CreateRange(__configurationSections);

        private {backingClassName}({types.IConfigurationValueCollection.ToDisplayString()} collection) 
        {{
            this.__backingCollection = collection;
            this.__configurationSections = new Dictionary<string, {types.IConfigurationSection.ToDisplayString()}>();
        
");
            foreach ((var iface, var prop) in properties)
            {
                // We can't parameterize ConfigurationSection<T> because Snowflake.Framework needs to consume 
                // Snowflake.Framework.Generator for EmptyPluginConfiguration, and it is not available at generator time.
                source.Append($@"
this.backing__{prop.Name} = new Snowflake.Configuration.ConfigurationSection<{prop.Type.ToDisplayString()}>(this.__backingCollection, nameof({iface.ToDisplayString()}.{prop.Name})); 
this.__configurationSections[nameof({iface.ToDisplayString()}.{prop.Name})] = this.backing__{prop.Name};
");
            }

            source.Append("}");
            foreach ((var iface, var prop) in properties)
            {
                source.Append($@"
private {types.IConfigurationSection.ToDisplayString()}<{prop.Type.ToDisplayString()}> backing__{prop.Name};
{prop.Type.ToDisplayString()} {iface.ToDisplayString()}.{prop.Name}
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
            context.RegisterForSyntaxNotifications(() => new ConfigurationTemplateInterfaceSyntaxReceiver("ConfigurationCollection"));
        }
    }
}
