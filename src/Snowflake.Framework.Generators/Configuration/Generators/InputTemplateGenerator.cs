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
    public sealed class InputTemplateGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is not ConfigurationTemplateInterfaceSyntaxReceiver receiver)
                return;
            bool errorOccured = false;
            var compilation = context.Compilation;
            var types = new ConfigurationTypes(compilation);
            if (!types.CheckContext(context, ref errorOccured))
                return;

            foreach (var iface in receiver.CandidateInterfaces)
            {
                errorOccured = false;
                var configOptionSymbols = new List<IPropertySymbol>();
                var inputOptionSymbols = new List<IPropertySymbol>();

                var model = compilation.GetSemanticModel(iface.SyntaxTree);
                var ifaceSymbol = model.GetDeclaredSymbol(iface);
                var memberSyntax = iface.Members;

                ConfigurationSectionGenerator.VerifyTemplateInterface(context, model, iface, ifaceSymbol, ref errorOccured);
                if (errorOccured)
                    continue;

                foreach (var prop in memberSyntax.Cast<PropertyDeclarationSyntax>())
                {
                    var propSymbol = model.GetDeclaredSymbol(prop);
                    if (propSymbol == null)
                    {
                        context.ReportError(DiagnosticError.InvalidMembers, "Property not found.",
                         $"Template property '{prop.Identifier.Text}' was not found. " +
                         $"Template property '{prop.Identifier.Text}' was not found.",
                         prop.GetLocation(), ref errorOccured);
                        continue;
                    }

                    var configOptionAttrs = propSymbol.GetAttributes()
                            .Where(attr => attr.AttributeClass?.Equals(types.ConfigurationOptionAttribute, SymbolEqualityComparer.Default) == true);
                    var inputTemplateAttrs = propSymbol.GetAttributes()
                            .Where(attr => attr.AttributeClass?.Equals(types.InputOptionAttribute, SymbolEqualityComparer.Default) == true);

                    if (!configOptionAttrs.Any() && !inputTemplateAttrs.Any())
                    {
                        context.ReportError(DiagnosticError.UndecoratedProperty, "Undecorated section property member",
                                   $"Property {propSymbol.Name} must be decorated with either a " +
                                   $"ConfigurationOptionAttribute or InputOptionAttribute.",
                               prop.GetLocation(), ref errorOccured);
                        continue;
                    }

                    if (configOptionAttrs.Any())
                    {
                        ConfigurationSectionGenerator
                            .VerifyOptionProperty(context, prop, propSymbol, types, ref errorOccured);
                        if (!errorOccured)
                            configOptionSymbols.Add(propSymbol);
                    } 
                    else if (inputTemplateAttrs.Any())
                    {
                        InputTemplateGenerator.VerifyOptionProperty(context, prop, propSymbol, ref errorOccured);
                        if (!errorOccured)
                            inputOptionSymbols.Add(propSymbol);
                    }

              
                }

                if (errorOccured)
                    continue;
                string? classSource = ProcessClass(ifaceSymbol!, configOptionSymbols, inputOptionSymbols, types, context);
                if (classSource != null)
                {
                    context.AddSource($"{ifaceSymbol!.Name}_InputTemplateSection.cs", SourceText.From(classSource, Encoding.UTF8));
                }
            }
        }

        public static void VerifyOptionProperty(
            GeneratorExecutionContext context,
            PropertyDeclarationSyntax prop, IPropertySymbol propSymbol,
            ref bool errorOccured)
        {

            if (prop.AccessorList == null || !prop.AccessorList.Accessors.Any(a => a.IsKind(SyntaxKind.GetAccessorDeclaration)))
            {
                context.ReportError(DiagnosticError.MissingSetter, "Missing get accessor",
                        $"Property '{propSymbol.Name}' must declare a getter.",
                    prop.GetLocation(), ref errorOccured);
            }

            if (prop.AccessorList == null || prop.AccessorList.Accessors.Any(a => a.IsKind(SyntaxKind.SetAccessorDeclaration)))
            {
                context.ReportError(DiagnosticError.UnexpectedSetter, "Unexpected set accessor",
                          $"Property '{propSymbol.Name}' must not declare a setter.",
                      prop.GetLocation(), ref errorOccured);
            }


            if (prop.AccessorList != null && prop.AccessorList.Accessors.Any(a => a.Body != null || a.ExpressionBody != null))
            {
                context.ReportError(DiagnosticError.UnexpectedBody, "Unexpected property body",
                          $"Property '{propSymbol.Name}' can not declare a body.",
                      prop.GetLocation(), ref errorOccured);
            }

        }

        private string? ProcessClass(INamedTypeSymbol classSymbol, List<IPropertySymbol> configOptionProps,
            List<IPropertySymbol> inputOptionProps,
            ConfigurationTypes types,
            GeneratorExecutionContext context)
        {
            if (!classSymbol.ContainingSymbol.Equals(classSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
            {
                bool errorOccured = false;
                context.ReportError(DiagnosticError.NotTopLevel,
                           "Template interface not top level.",
                           $"Collection template interface {classSymbol.Name} must be defined within an enclosing top-level namespace.",
                           classSymbol.Locations.First(), ref errorOccured);

                return null;
            }

            string namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
            string generatedNamespaceName = $"Snowflake.Configuration.GeneratedConfigurationProxies.Section__Input{namespaceName}";

            string tag = RandomString(6);
            string backingClassName = $"{classSymbol.Name}Proxy_{tag}";
            StringBuilder source = new StringBuilder($@"
namespace {namespaceName}
{{
    [{types.ConfigurationGenerationInstanceAttribute.ToDisplayString()}(typeof({generatedNamespaceName}.{backingClassName}))]
    public partial interface {classSymbol.Name}
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
    sealed class {backingClassName} : {classSymbol.ToDisplayString()}
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

            foreach (var prop in configOptionProps)
            {
                source.Append($@"
{prop.Type.ToDisplayString()} {classSymbol.ToDisplayString()}.{prop.Name}
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

            foreach (var prop in inputOptionProps)
            {
                source.Append($@"
{types.DeviceCapability.ToDisplayString()} {classSymbol.ToDisplayString()}.{prop.Name}
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
            context.RegisterForSyntaxNotifications(() => new ConfigurationTemplateInterfaceSyntaxReceiver("InputTemplate"));
        }
    }
}
