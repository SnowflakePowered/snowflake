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
                {
                    context.ReportError(DiagnosticError.InvalidMembers, "Interface not found.",
                         $"Template interface '{ifaceSyntax.Identifier.Text}' was not found. " +
                         $"Template interface '{ifaceSyntax.Identifier.Text}' was not found.",
                         Location.None, ref errorOccurred);
                    continue;
                }

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

                var properties = new List<(INamedTypeSymbol, IPropertySymbol)>();
                var seenProps = new HashSet<string>();

                foreach (var childIface in ifaceSymbol.AllInterfaces.Reverse().Concat(new[] { ifaceSymbol }))
                {
                    // No need to check if child interfaces are partial because we only add to the root interface.
                    foreach (var member in childIface.GetMembers())
                    {
                        if (ConfigurationSectionGenerator.VerifyOptionProperty(context,
                            types, member, ifaceSymbol, in seenProps,
                            out var property))
                        {
                            properties.Add((childIface, property!));
                        }
                    }
                }
            
                string? classSource = ProcessClass(ifaceSymbol!, properties, types, context);
                if (classSource != null)
                {
                    context.AddSource($"{ifaceSymbol!.Name}_ConfigurationSection.cs", SourceText.From(classSource, Encoding.UTF8));
                }
            }
        }

        
        private string? ProcessClass(INamedTypeSymbol classSymbol, List<(INamedTypeSymbol, IPropertySymbol)> props,
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
            string generatedNamespaceName = $"Snowflake.Configuration.GeneratedConfigurationProxies.Section__Section{namespaceName}";

            string tag = RandomString(6);
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

        private static Random random = new Random();

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
                       .Where(attr => attr?.AttributeClass?.Equals(types.ConfigurationOptionAttribute, SymbolEqualityComparer.Default) == true);

            if (!propAttr.Any())
            {
                context.ReportError(DiagnosticError.UndecoratedProperty, "Undecorated section property member",
                           $"Property {property.Name} must be decorated with a ConfigurationOptionAttribute.",
                       propertySyntax.GetLocation(), ref errorOccurred);
            }

            if (propertySyntax.AccessorList == null || !propertySyntax.AccessorList.Accessors.Any(a => a.IsKind(SyntaxKind.SetAccessorDeclaration)))
            {
                context.ReportError(DiagnosticError.MissingSetter, "Missing set accessor",
                          $"Property '{property.Name}' must declare a setter.",
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

            var attr = propAttr.First();
            // If it has a second arg, then it must not be GUID-based 
            if (attr.ConstructorArguments.Skip(1).FirstOrDefault().Type is ITypeSymbol defaultType)
            {
                if (!SymbolEqualityComparer.Default.Equals(defaultType, property.Type))
                {
                    context.ReportError(DiagnosticError.MismatchedType, "Mismatched default value type",
                            $"Property {property.Name} is of type '{property.Type}' but has default value of type '{defaultType}'.",
                            propertySyntax.GetLocation(), ref errorOccurred);
                }
                
                if (property.Type.TypeKind == TypeKind.Enum)
                {
                    var enumMembers = property.Type.GetMembers();
                    foreach (var enumMember in enumMembers)
                    {
                        // Enums should only have Fields and Constructors (Methods)
                        if (enumMember.Kind != SymbolKind.Field)
                            continue;
                        if (!enumMember.GetAttributes()
                            .Any(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.SelectionOptionAttribute)))
                        {
                            context.ReportError(DiagnosticError.UndecoratedProperty, "Undecorated selection option enum member",
                                $"Enum member '{enumMember.Name}' must be decorated with SelectionOptionAttribute.",
                                enumMember.Locations.First(), ref errorOccurred);
                        }
                    }
                }
            }
            else if (attr.ConstructorArguments.Length == 1 && !SymbolEqualityComparer.Default.Equals(property.Type, types.System_Guid))
            {
                context.ReportError(DiagnosticError.MismatchedType, "Mismatched default value type",
                        $"Property {property.Name} is of type '{property.Type}' but needs to be of type '{types.System_Guid}'.",
                        propertySyntax.GetLocation(), ref errorOccurred);
            }

            if (!errorOccurred)
            {
                seenProperties.Add(property.Name);
            }
            propertyResult = property;
            return !errorOccurred;
        }

        internal static void VerifyTemplateInterface(
            GeneratorExecutionContext context,
            SemanticModel model, 
            IEnumerable<MemberDeclarationSyntax> memberSyntax,
            string ifaceName,
            INamedTypeSymbol? ifaceSymbol,
            ref bool errorOccurred)
        {
            if (ifaceSymbol == null)
            {
                context.ReportError(DiagnosticError.InvalidMembers, "Interface not found.",
                 $"Template interface '{ifaceName}' was not found. " +
                 $"Template interface '{ifaceName}' was not found.",
                 Location.None, ref errorOccurred);
                return;
            }



            //if (!iface.Modifiers.Any(p => p.IsKind(SyntaxKind.PartialKeyword)))
            //{
            //    context.ReportError(DiagnosticError.UnextendibleInterface,
            //               "Unextendible template interface",
            //               $"Template interface '{ifaceSymbol.Name}' must be marked partial.",
            //               iface.GetLocation(), ref errorOccurred);
            //}

            //if (iface.BaseList != null && iface.BaseList.ChildNodes().Any())
            //{
            //    context.ReportError(DiagnosticError.UnextendibleInterface,
            //               "Unextendible template interface",
            //               $"Template interface '{ifaceSymbol.Name}' is not allowed to extend another interface.",
            //               iface.GetLocation(), ref errorOccurred);
            //}

        }

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
            context.RegisterForSyntaxNotifications(() => new ConfigurationTemplateInterfaceSyntaxReceiver("ConfigurationSection"));
        }
    }
}
