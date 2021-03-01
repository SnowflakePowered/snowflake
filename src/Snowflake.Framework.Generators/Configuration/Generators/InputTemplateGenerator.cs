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
            bool errorOccurred = false;
            var compilation = context.Compilation;
            var types = new ConfigurationTypes(compilation);
            if (!types.CheckContext(context, ref errorOccurred))
                return;

            foreach (var ifaceSyntax in receiver.CandidateInterfaces)
            {
                errorOccurred = false;
                var model = compilation.GetSemanticModel(ifaceSyntax.SyntaxTree);
                var ifaceSymbol = model.GetDeclaredSymbol(ifaceSyntax);
                if (ifaceSymbol == null)
                    continue;
                if (!ifaceSymbol.GetAttributes()
                 .Any(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.InputConfigurationAttribute)))
                    continue;

                if (!ifaceSymbol.ContainingSymbol.Equals(ifaceSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
                {
                    bool errorOccured = false;
                    context.ReportError(DiagnosticError.NotTopLevel,
                               "Template interface not top level.",
                               $"Collection template interface {ifaceSymbol.Name} must be defined within an enclosing top-level namespace.",
                               ifaceSyntax.GetLocation(), ref errorOccured);
                }

                if (!ifaceSyntax.Modifiers.Any(p => p.IsKind(SyntaxKind.PartialKeyword)))
                {
                    context.ReportError(DiagnosticError.UnextendibleInterface,
                               "Unextendible template interface",
                               $"Template interface '{ifaceSymbol.Name}' must be marked partial.",
                               ifaceSyntax.GetLocation(), ref errorOccurred);
                }

                var configProperties = new List<(INamedTypeSymbol, IPropertySymbol)>();
                var inputProperties = new List<(INamedTypeSymbol, IPropertySymbol)>();

                var seenProps = new HashSet<string>();


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
                        if (!isConfigOption && !isInputOption)
                        {
                            context.ReportError(DiagnosticError.UndecoratedProperty, "Undecorated section property member",
                                  $"Property {member.Name} must be decorated with either a " +
                                  $"ConfigurationOptionAttribute or InputOptionAttribute.",
                              member.Locations.First(), ref errorOccurred);
                            continue;
                        }
                        if (isConfigOption && isInputOption)
                        {
                            context.ReportError(DiagnosticError.BothOptionTypes, "Duplicated property type",
                                  $"Property {member.Name} can not be marked with both InputOptionAttribute and ConfigurationOptionAttribute.",
                              member.Locations.First(), ref errorOccurred);
                            continue;
                        }

                        if (isConfigOption && ConfigurationSectionGenerator.VerifyOptionProperty(context,
                            types, member, childIface, in seenProps,
                            out var property))
                        {
                            configProperties.Add((childIface, property!));
                        } else if (isInputOption && InputTemplateGenerator.VerifyOptionProperty(context, types, member, childIface, 
                            in seenProps, out property))
                        {
                            inputProperties.Add((childIface, property!));
                        }
                    }
                }

                string? classSource = ProcessClass(ifaceSymbol, configProperties, inputProperties, types, context);
                if (classSource != null)
                {
                    context.AddSource($"{ifaceSymbol.Name}_InputTemplateSection.cs", SourceText.From(classSource, Encoding.UTF8));
                }
            }
        }


        internal static bool VerifyOptionProperty(
               GeneratorExecutionContext context,
               ConfigurationTypes types,
               ISymbol member,
               INamedTypeSymbol ifaceSymbol,
               in HashSet<string> seenProperties,
               out IPropertySymbol? propertyResult)
        {

            bool errorOccurred = false;
            // Ignore accessors, we only care about the property declaration.
            if (member is IMethodSymbol accessor && accessor.AssociatedSymbol is IPropertySymbol)
            {
                propertyResult = null;
                return false;
            }

            if (member is not IPropertySymbol property)
            {
                context.ReportError(DiagnosticError.InvalidMembers, "Invalid members in template interface.",
                   $"Template interface '{ifaceSymbol.Name}' must only declare property members. " +
                   $"{member.Kind} '{ifaceSymbol.Name}.{member.Name}' is not a property.",
                   member.Locations.First(), ref errorOccurred);
                propertyResult = null;
                return !errorOccurred;
            }

            var propertySyntax = (PropertyDeclarationSyntax)property.DeclaringSyntaxReferences.First().GetSyntax();
            var propertyLocation = propertySyntax.GetLocation();

            if (seenProperties.Contains(property.Name))
            {
                context.ReportError(DiagnosticError.DuplicateProperty,
                   "Duplicate template property key.",
                   $"A template property named '{property.Name}' already exists. Template interfaces are not allowed to hide or override inherited properties.",
                   propertyLocation, ref errorOccurred);
            }

            var propAttr = property.GetAttributes()
                       .Where(attr => attr?.AttributeClass?.Equals(types.InputOptionAttribute, SymbolEqualityComparer.Default) == true);

            if (!propAttr.Any())
            {
                context.ReportError(DiagnosticError.UndecoratedProperty, "Undecorated section property member",
                           $"Property {property.Name} must be decorated with a InputOptionAttribute.",
                       propertySyntax.GetLocation(), ref errorOccurred);
            }

            if (propertySyntax.AccessorList == null || propertySyntax.AccessorList.Accessors.Any(a => a.IsKind(SyntaxKind.SetAccessorDeclaration)))
            {
                context.ReportError(DiagnosticError.UnexpectedSetter, "Unexpected set accessor",
                          $"Property '{property.Name}' must not declare a setter.",
                      propertySyntax.GetLocation(), ref errorOccurred);
            }

            if (propertySyntax.AccessorList == null || !propertySyntax.AccessorList.Accessors.Any(a => a.IsKind(SyntaxKind.GetAccessorDeclaration)))
            {
                context.ReportError(DiagnosticError.MissingSetter, "Missing get accessor",
                        $"Property '{property.Name}' must declare a getter.",
                    propertySyntax.GetLocation(), ref errorOccurred);
            }

            if (propertySyntax.AccessorList != null && propertySyntax.AccessorList.Accessors.Any(a => a.Body != null || a.ExpressionBody != null))
            {
                context.ReportError(DiagnosticError.UnexpectedBody, "Unexpected property body",
                          $"Property '{property.Name}' can not declare a body.",
                      propertySyntax.GetLocation(), ref errorOccurred);
            }

            if (!propAttr.Any())
            {
                propertyResult = null;
                return false;
            }

            if (!errorOccurred)
            {
                seenProperties.Add(property.Name);
            }
            propertyResult = property;
            return !errorOccurred;
        }

        private string? ProcessClass(INamedTypeSymbol rootInterface,
            List<(INamedTypeSymbol, IPropertySymbol)> configOptionProps,
            List<(INamedTypeSymbol, IPropertySymbol)> inputOptionProps,
            ConfigurationTypes types,
            GeneratorExecutionContext context)
        {
            string namespaceName = rootInterface.ContainingNamespace.ToDisplayString();
            string generatedNamespaceName = $"Snowflake.Configuration.GeneratedConfigurationProxies.Section__Input{namespaceName}";

            string tag = RandomString(6);
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
            context.RegisterForSyntaxNotifications(() => new ConfigurationTemplateInterfaceSyntaxReceiver("InputConfiguration"));
        }
    }
}
