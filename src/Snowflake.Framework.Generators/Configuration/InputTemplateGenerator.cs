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
            INamedTypeSymbol? configSectionAttr = compilation.GetTypeByMetadataName("Snowflake.Configuration.Attributes.ConfigurationSectionAttribute");
            INamedTypeSymbol? configOptionAttr = compilation.GetTypeByMetadataName("Snowflake.Configuration.Attributes.ConfigurationOptionAttribute");
            INamedTypeSymbol? inputOptionAttr = compilation.GetTypeByMetadataName("Snowflake.Configuration.Input.InputOptionAttribute");

            INamedTypeSymbol? configSectionInterface = compilation.GetTypeByMetadataName("Snowflake.Configuration.IConfigurationSection");
            INamedTypeSymbol? configSectionGenericInterface = compilation.GetTypeByMetadataName("Snowflake.Configuration.IConfigurationSection`1");
            INamedTypeSymbol? configInstanceAttr = compilation.GetTypeByMetadataName("Snowflake.Configuration.Generators.ConfigurationGenerationInstanceAttribute");
            INamedTypeSymbol? selectionOptionAttr = compilation.GetTypeByMetadataName("Snowflake.Configuration.Attributes.SelectionOptionAttribute");
            INamedTypeSymbol? deviceCapabilityType = compilation.GetTypeByMetadataName("Snowflake.Input.Device.DeviceCapability");
            INamedTypeSymbol guidType = compilation.GetTypeByMetadataName("System.Guid")!;

            if (configSectionAttr == null
                || configOptionAttr == null
                || inputOptionAttr == null
                || configSectionInterface == null
                || configSectionGenericInterface == null
                || configInstanceAttr == null
                || selectionOptionAttr == null
                || deviceCapabilityType == null)
            {
                context.ReportError(DiagnosticError.FrameworkNotFound,
                           "Snowflake Framework Not Found",
                           $"Snowflake framework types were not found.",
                           Location.None, ref errorOccured);
                return;
            }

            foreach (var iface in receiver.CandidateInterfaces)
            {
                var configOptionSymbols = new List<IPropertySymbol>();
                var inputOptionSymbols = new List<IPropertySymbol>();

                var model = compilation.GetSemanticModel(iface.SyntaxTree);
                var ifaceSymbol = model.GetDeclaredSymbol(iface);
                var memberSyntax = iface.Members;

                if (ifaceSymbol == null)
                {
                    context.ReportError(DiagnosticError.InvalidMembers, "Interface not found.",
                     $"Template interface '{iface.Identifier.Text}' was not found. " +
                     $"Template interface '{iface.Identifier.Text}' was not found.",
                     iface.GetLocation(), ref errorOccured);
                    return;
                }

                if (memberSyntax.FirstOrDefault(m => m is not PropertyDeclarationSyntax) is MemberDeclarationSyntax badSyntax)
                {
                    var badSymbol = model.GetDeclaredSymbol(badSyntax);
                    context.ReportError(DiagnosticError.InvalidMembers, "Invalid members in template interface.",
                        $"Template interface '{ifaceSymbol.Name}' must only declare property members. " +
                        $"{badSymbol?.Kind} '{ifaceSymbol.Name}.{badSymbol?.Name}' is not a property.",
                        badSyntax.GetLocation(), ref errorOccured);
                    continue;
                }

                if (!iface.Modifiers.Any(p => p.IsKind(SyntaxKind.PartialKeyword)))
                {
                    context.ReportError(DiagnosticError.UnextendibleInterface,
                               "Unextendible template interface",
                               $"Template interface '{ifaceSymbol.Name}' must be marked partial.",
                               iface.GetLocation(), ref errorOccured);
                    continue;
                }

                if (iface.BaseList != null && iface.BaseList.ChildNodes().Any())
                {
                    context.ReportError(DiagnosticError.UnextendibleInterface,
                               "Unextendible template interface",
                               $"Template interface '{ifaceSymbol.Name}' can not extend another interface (todo: recursively sort out extending interfaces)",
                               iface.GetLocation(), ref errorOccured);
                    continue;
                }

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
                            .Where(attr => attr.AttributeClass?.Equals(configOptionAttr, SymbolEqualityComparer.Default) == true);
                    var inputTemplateAttrs = propSymbol.GetAttributes()
                            .Where(attr => attr.AttributeClass?.Equals(inputOptionAttr, SymbolEqualityComparer.Default) == true);

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
                            .VerifyOptionProperty(context, configOptionAttrs.First(), prop, propSymbol, guidType, selectionOptionAttr, ref errorOccured);
                        if (!errorOccured)
                            configOptionSymbols.Add(propSymbol);
                    } 
                    else if (inputTemplateAttrs.Any())
                    {
                        InputTemplateGenerator.VerifyOptionProperty(context, inputTemplateAttrs.First(), prop, propSymbol, deviceCapabilityType, ref errorOccured);
                        if (!errorOccured)
                            inputOptionSymbols.Add(propSymbol);
                    }

              
                }

                if (errorOccured)
                    return;
                string? classSource = ProcessClass(ifaceSymbol, configOptionSymbols, inputOptionSymbols, configSectionInterface, configSectionGenericInterface, configInstanceAttr, context);
                if (classSource != null)
                {
                    context.AddSource($"{ifaceSymbol.Name}_InputTemplateSection.cs", SourceText.From(classSource, Encoding.UTF8));
                }
            }
        }

        public static void VerifyOptionProperty(
            GeneratorExecutionContext context,
            AttributeData attr, PropertyDeclarationSyntax prop, IPropertySymbol propSymbol,
            INamedTypeSymbol deviceCapabilityType,
            ref bool errorOccured)
        {

            if (prop.AccessorList == null || !prop.AccessorList.Accessors.Any(a => a.IsKind(SyntaxKind.GetAccessorDeclaration)))
            {
                context.ReportError(DiagnosticError.MissingSetter, "Missing get accessor",
                        $"Property {propSymbol.Name} must declare a getter.",
                    prop.GetLocation(), ref errorOccured);
            }

            if (prop.AccessorList == null || prop.AccessorList.Accessors.Any(a => a.IsKind(SyntaxKind.SetAccessorDeclaration)))
            {
                context.ReportError(DiagnosticError.UnexpectedSetter, "Unexpected set accessor",
                          $"Property {propSymbol.Name} must not declare a setter.",
                      prop.GetLocation(), ref errorOccured);
            }


            if (prop.AccessorList != null && prop.AccessorList.Accessors.Any(a => a.Body != null || a.ExpressionBody != null))
            {
                context.ReportError(DiagnosticError.UnexpectedBody, "Unexpected property body",
                          $"Property {propSymbol.Name} can not declare a body.",
                      prop.GetLocation(), ref errorOccured);
            }
        }

        public string? ProcessClass(INamedTypeSymbol classSymbol, List<IPropertySymbol> configOptionProps,
            List<IPropertySymbol> inputOptionProps,

            INamedTypeSymbol configSectionInterface,
            INamedTypeSymbol configSectionGenericInterface,
            INamedTypeSymbol configInstanceAttr,
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
    [{configInstanceAttr.ToDisplayString()}(typeof({generatedNamespaceName}.{backingClassName}))]
    public partial interface {classSymbol.Name}
        : Snowflake.Configuration.Generators.IInputTemplateGeneratedProxy
    {{
    
    }}

}}

namespace {generatedNamespaceName}
{{
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    [EditorBrowsable(EditorBrowsableState.Never)]
    sealed class {backingClassName} : {classSymbol.ToDisplayString()}
    {{
        readonly Snowflake.Configuration.IConfigurationSectionDescriptor __sectionDescriptor;
        readonly Snowflake.Configuration.IConfigurationValueCollection __backingCollection;
        readonly Dictionary<string, Snowflake.Input.Device.DeviceCapability> __backingMap;

        IReadOnlyDictionary<string, Snowflake.Input.Device.DeviceCapability> 
            Snowflake.Configuration.Generators.IInputTemplateGeneratedProxy.GetValueDictionary() 
                => ImmutableDictionary.CreateRange(this.__backingMap);

        Snowflake.Input.Device.DeviceCapability Snowflake.Configuration.Generators.IInputTemplateGeneratedProxy.this[string keyName] 
        {{
            set {{
                if (this.__backingMap.ContainsKey(keyName))
                    this.__backingMap[keyName] = value;
            }}
        }}

        private {backingClassName}(
Snowflake.Configuration.IConfigurationSectionDescriptor sectionDescriptor, 
Snowflake.Configuration.IConfigurationValueCollection collection,
Dictionary<string, Snowflake.Input.Device.DeviceCapability> mapping
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
Snowflake.Input.Device.DeviceCapability {classSymbol.ToDisplayString()}.{prop.Name}
{{
    get {{ 
        if (this.__backingMap.TryGetValue(nameof({prop.ToDisplayString()}), out Snowflake.Input.Device.DeviceCapability value)) {{
            return value;
        }} else {{
            return Snowflake.Input.Device.DeviceCapability.None;
        }}
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
            context.RegisterForSyntaxNotifications(() => new ConfigurationTemplateInterfaceSyntaxReceiver("InputTemplate"));
        }
    }
}
