using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Snowflake.Language.Analyzers;
using Snowflake.Language.Analyzers.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Language.Generators.Configuration
{
    [Generator]
    public sealed class InputConfigurationGenerator : ISourceGenerator
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
              new OnlyOneAttributePropertyTypeAnalyzer(),
              new InputPropertyUndecoratedAnalyzer(),
              new InputPropertyTypeMismatchAnalyzer()
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
                 .Any(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.InputConfigurationAttribute)))
                    continue;

                var diagnostics = Analyzers.AsParallel()
                  .SelectMany(a => a.Analyze(context.Compilation, model, ifaceSyntax, context.CancellationToken))
                  .ToList();

                foreach (var diag in diagnostics)
                {
                    context.ReportDiagnostic(diag);
                }
                if (diagnostics.Any())
                    return;

                var configProperties = new List<(INamedTypeSymbol, IPropertySymbol)>();
                var inputProperties = new List<(INamedTypeSymbol, IPropertySymbol)>();

                foreach (var childIface in ifaceSymbol.AllInterfaces.Reverse().Concat(new[] { ifaceSymbol }))
                {
                    // No need to check if child interfaces are partial because we only add to the root interface.
                    foreach (var member in childIface.GetMembers())
                    {
                        if (member is IMethodSymbol accessor && accessor.AssociatedSymbol is IPropertySymbol)
                            continue;
                        
                        bool isConfigOption = member.GetAttributes()
                            .Where(attr => attr?.AttributeClass?
                                .Equals(types.ConfigurationOptionAttribute, SymbolEqualityComparer.Default) == true)
                            .Any();
                        bool isInputOption = member.GetAttributes()
                          .Where(attr => attr?.AttributeClass?
                              .Equals(types.InputOptionAttribute, SymbolEqualityComparer.Default) == true)
                          .Any();

                        // case where neither/both handled by analyzer guards above.
                        if (isConfigOption && member is IPropertySymbol optionProperty)
                        {
                            configProperties.Add((childIface, optionProperty));
                        } else if (isInputOption && member is IPropertySymbol inputProperty)
                        {
                            inputProperties.Add((childIface, inputProperty));
                        }
                    }
                }

                string classSource = GenerateSource(ifaceSymbol, configProperties, inputProperties, types);
                context.AddSource($"{ifaceSymbol.Name}_InputConfigurationSection.cs", SourceText.From(classSource, Encoding.UTF8));
            }
        }

        private string GenerateSource(INamedTypeSymbol rootInterface,
            List<(INamedTypeSymbol, IPropertySymbol)> configOptionProps,
            List<(INamedTypeSymbol, IPropertySymbol)> inputOptionProps,
            ConfigurationTypes types)
        {
            string namespaceName = rootInterface.ContainingNamespace.ToDisplayString();
            string generatedNamespaceName = $"Snowflake.Configuration.GeneratedConfigurationProxies.Section__Input{namespaceName}";

            string tag = RandomTag.Tag(6);

            string backingClassName = $"{rootInterface.Name}Proxy_{tag}";
            StringBuilder source = new StringBuilder($@"
namespace {namespaceName}
{{
    [{types.ConfigurationGenerationInstanceAttribute.ToDisplayString()}(typeof({generatedNamespaceName}.{backingClassName}))]
    public partial interface {rootInterface.Name}
        : {types.IInputConfigurationTemplate.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}
    {{
    
    }}

}}

namespace {generatedNamespaceName}
{{
#pragma warning disable CS0472
#pragma warning disable CS8073
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    [EditorBrowsable(EditorBrowsableState.Never)]
    sealed class {backingClassName} : {rootInterface.ToDisplayString()}
    {{
        readonly {types.IConfigurationSectionDescriptor.ToDisplayString()} __sectionDescriptor;
        readonly {types.IConfigurationValueCollection.ToDisplayString()}  __backingCollection;
        readonly Dictionary<string, {types.DeviceCapability.ToDisplayString()}> __backingMap;

        IReadOnlyDictionary<string, {types.DeviceCapability.ToDisplayString()}> 
            {types.IInputConfigurationTemplate.ToDisplayString()}.GetValueDictionary() 
                => ImmutableDictionary.CreateRange(this.__backingMap);

        {types.DeviceCapability.ToDisplayString()} 
            {types.IInputConfigurationTemplate.ToDisplayString()}.this[string keyName] 
        {{
            set {{
                if (this.__backingMap.ContainsKey(keyName))
                    this.__backingMap[keyName] = value;
            }}
        }}

        private {backingClassName}(
{types.IConfigurationSectionDescriptor.ToDisplayString()} sectionDescriptor, 
{types.IConfigurationValueCollection.ToDisplayString()} collection,
Dictionary<string, {types.DeviceCapability.ToDisplayString()}> mapping
) 
        {{
            this.__sectionDescriptor = sectionDescriptor;
            this.__backingCollection = collection;
            this.__backingMap = mapping;
        }}
");

            foreach ((var iface, var prop) in configOptionProps)
            {
                source.Append($@"
{prop.Type.ToDisplayString()} {iface.ToDisplayString()}.{prop.Name}
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

            foreach ((var iface, var prop) in inputOptionProps)
            {
                source.Append($@"
{types.DeviceCapability.ToDisplayString()} {iface.ToDisplayString()}.{prop.Name}
{{
    get {{ 
        if (this.__backingMap.TryGetValue(nameof({prop.ToDisplayString()}), out Snowflake.Input.Device.DeviceCapability value)) {{
            return value;
        }} else {{
            return {types.DeviceCapability.ToDisplayString()}.None;
        }}
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
            context.RegisterForSyntaxNotifications(() => new ConfigurationTemplateInterfaceSyntaxReceiver("InputConfiguration"));
        }
    }
}
