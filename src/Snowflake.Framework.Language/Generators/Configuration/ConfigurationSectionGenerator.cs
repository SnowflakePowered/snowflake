using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Snowflake.Language.Analyzers;
using Snowflake.Language.Analyzers.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Language.Generators.Configuration
{
    [Generator]
    public sealed class ConfigurationSectionGenerator : ISourceGenerator
    {
        private static ImmutableArray<AbstractSyntaxNodeAnalyzer> Analyzers
        = ImmutableArray.Create<AbstractSyntaxNodeAnalyzer>(
            new TemplateInterfaceTopLevelAnalyzer(),
            new UnextendibleInterfaceAnalyzer(),
            new CannotHideInheritedPropertyAnalyzer(),
            new InvalidTemplateAccessorAnalyzer(),
            new SectionPropertyMismatchedTypeAnalyzer(),
            new SectionPropertyEnumUndecoratedAnalyzer(),
            new SectionPropertyInvalidAccessorAnalyzer(),
            new SectionPropertyMustHaveGetterAnalyzer(),
            new SectionPropertyMustHaveSetterAnalyzer(),
            new OnlyOneAttributeTemplateTypeAnalyzer(),
            new SectionPropertyUndecoratedAnalyzer()
        );

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is not ConfigurationTemplateInterfaceSyntaxReceiver receiver)
                return;
            var compilation = context.Compilation;
            var types = new ConfigurationTypes(compilation);
            if (!types.CheckContext(context))
                return;
            foreach (var ifaceSyntax in receiver.CandidateInterfaces)
            {
                var model = compilation.GetSemanticModel(ifaceSyntax.SyntaxTree);
                var ifaceSymbol = model.GetDeclaredSymbol(ifaceSyntax, context.CancellationToken);
                if (ifaceSymbol == null)
                    continue;
                if (!ifaceSymbol.GetAttributes()
                       .Any(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.ConfigurationSectionAttribute)))
                    return;

                var diagnostics = Analyzers.AsParallel()
                  .SelectMany(a => a.Analyze(context.Compilation, model, ifaceSyntax, context.CancellationToken))
                  .ToList();

                foreach (var diag in diagnostics)
                {
                    context.ReportDiagnostic(diag);
                }
                if (diagnostics.Any())
                    return;

                var properties = new List<(INamedTypeSymbol, IPropertySymbol)>();
                var seenProps = new HashSet<string>();

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
                context.AddSource($"{ifaceSymbol!.Name}_ConfigurationSection.cs", SourceText.From(classSource, Encoding.UTF8));
            }
        }

        
        private string GenerateSource(INamedTypeSymbol classSymbol, List<(INamedTypeSymbol, IPropertySymbol)> props, ConfigurationTypes types)
        {
            string namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
            string generatedNamespaceName = $"Snowflake.Configuration.GeneratedConfigurationProxies.Section__Section{namespaceName}";
            string tag = RandomTag.Tag(6);
            string backingClassName = $"{classSymbol.Name}Proxy_{tag}";
            StringBuilder source = new StringBuilder($@"
namespace {namespaceName}
{{
    [{types.ConfigurationGenerationInstanceAttribute.ToDisplayString()}(typeof({generatedNamespaceName}.{backingClassName}))]
    public partial interface {classSymbol.Name}
    {{
    
    }}

}}

namespace {generatedNamespaceName}
{{
#pragma warning disable CS0472
#pragma warning disable CS8073
    using System.ComponentModel;
    [EditorBrowsable(EditorBrowsableState.Never)]
    sealed class {backingClassName} : {classSymbol.ToDisplayString()}
    {{
        readonly {types.IConfigurationSectionDescriptor.ToDisplayString()}  __sectionDescriptor;
        readonly {types.IConfigurationValueCollection.ToDisplayString()}  __backingCollection;

        private {backingClassName}({types.IConfigurationSectionDescriptor.ToDisplayString()} sectionDescriptor, 
            {types.IConfigurationValueCollection.ToDisplayString()} collection) 
        {{
            this.__sectionDescriptor = sectionDescriptor;
            this.__backingCollection = collection;
        }}
");

            foreach ((var iface, var prop) in props)
            {
                source.Append($@"
{prop.Type.ToDisplayString()} {iface.ToDisplayString()}.{prop.Name}
{{
    get {{ return ({prop.Type.ToDisplayString()})this.__backingCollection[this.__sectionDescriptor, nameof({prop.ToDisplayString()})].Value; }}
    set {{ 
            var existingValue = this.__backingCollection[this.__sectionDescriptor, nameof({prop.ToDisplayString()})];
            if (existingValue != null && value != null) {{ existingValue.Value = value; }}
            if (existingValue != null && value == null && this.__sectionDescriptor[nameof({prop.ToDisplayString()})].Type == typeof(string)) 
            {{ existingValue.Value = this.__sectionDescriptor[nameof({prop.ToDisplayString()})].Unset; }}
        }}
}}
");
            }

            source.Append(@"
#pragma warning restore CS0472
#pragma warning restore CS8073
}}");
            return source.ToString();
        }

        public void Initialize(GeneratorInitializationContext context)
        {
#if DEBUG
            //if (!Debugger.IsAttached)
            //{
            //    Debugger.Launch();
            //}
#endif 
            context.RegisterForSyntaxNotifications(() => new ConfigurationTemplateInterfaceSyntaxReceiver("ConfigurationSection"));
        }
    }
}
