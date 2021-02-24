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
            bool errorOccured = false;
            var compilation = context.Compilation;
            INamedTypeSymbol? configSectionAttr = compilation.GetTypeByMetadataName("Snowflake.Configuration.Attributes.ConfigurationSectionAttribute");
            INamedTypeSymbol? configOptionAttr = compilation.GetTypeByMetadataName("Snowflake.Configuration.Attributes.ConfigurationOptionAttribute");
                            
            INamedTypeSymbol? configSectionInterface = compilation.GetTypeByMetadataName("Snowflake.Configuration.IConfigurationSection");
            INamedTypeSymbol? configSectionGenericInterface = compilation.GetTypeByMetadataName("Snowflake.Configuration.IConfigurationSection`1");
            INamedTypeSymbol? configInstanceAttr = compilation.GetTypeByMetadataName("Snowflake.Configuration.Generators.ConfigurationGenerationInstanceAttribute");
            INamedTypeSymbol? selectionOptionAttr = compilation.GetTypeByMetadataName("Snowflake.Configuration.Attributes.SelectionOptionAttribute");
            INamedTypeSymbol guidType = compilation.GetTypeByMetadataName("System.Guid")!;

            if (configSectionAttr == null 
                || configOptionAttr == null
                || configSectionInterface == null
                || configSectionGenericInterface == null
                || configInstanceAttr == null
                || selectionOptionAttr == null)
            {
                context.ReportError(DiagnosticError.FrameworkNotFound,
                             "Snowflake Framework Not Found",
                             $"Snowflake framework types were not found.",
                             Location.None, ref errorOccured);
                return;
            }

            foreach (var iface in receiver.CandidateInterfaces)
            {
                var symbols = new List<IPropertySymbol>();
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
                    var attrs = propSymbol.GetAttributes().Where(attr => attr?.AttributeClass?.Equals(configOptionAttr, SymbolEqualityComparer.Default) == true);
                    if (!attrs.Any())
                    {
                        context.ReportError(DiagnosticError.UndecoratedProperty, "Undecorated section property member",
                                   $"Property {propSymbol.Name} must be decorated with a ConfigurationOptionAttribute.",
                               prop.GetLocation(), ref errorOccured);
                        continue;
                    }

                    var attr = attrs.First();
                    ConfigurationSectionGenerator.VerifyOptionProperty(context, attr, prop, propSymbol, guidType, selectionOptionAttr, ref errorOccured);
                    if (!errorOccured) 
                        symbols.Add(propSymbol);
                }

                if (errorOccured)
                    return;

                string? classSource = ProcessClass(ifaceSymbol, symbols, configSectionInterface, configSectionGenericInterface, configInstanceAttr, context);
                if (classSource != null)
                {
                    context.AddSource($"{ifaceSymbol.Name}_ConfigurationSection.cs", SourceText.From(classSource, Encoding.UTF8));
                }
            }
        }

        
        public string? ProcessClass(INamedTypeSymbol classSymbol, List<IPropertySymbol> props,

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
            string generatedNamespaceName = $"Snowflake.Configuration.GeneratedConfigurationProxies.Section__Section{namespaceName}";

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
        readonly Snowflake.Configuration.IConfigurationSectionDescriptor __sectionDescriptor;
        readonly Snowflake.Configuration.IConfigurationValueCollection __backingCollection;

        private {backingClassName}(Snowflake.Configuration.IConfigurationSectionDescriptor sectionDescriptor, Snowflake.Configuration.IConfigurationValueCollection collection) 
        {{
            this.__sectionDescriptor = sectionDescriptor;
            this.__backingCollection = collection;
        }}
");

            foreach (var prop in props)
            {
                source.Append($@"
{prop.Type.ToDisplayString()} {classSymbol.ToDisplayString()}.{prop.Name}
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


            source.Append("}}");
            return source.ToString();
        }

        private static Random random = new Random();

        public static void VerifyOptionProperty(
            GeneratorExecutionContext context,
            AttributeData attr, PropertyDeclarationSyntax prop, IPropertySymbol propSymbol,
            INamedTypeSymbol guidType,
            INamedTypeSymbol selectionOptionAttr,
            ref bool errorOccured)
        {

            if (prop.AccessorList == null || !prop.AccessorList.Accessors.Any(a => a.IsKind(SyntaxKind.SetAccessorDeclaration)))
            {
                context.ReportError(DiagnosticError.MissingSetter, "Missing set accessor",
                          $"Property '{propSymbol.Name}' must declare a setter.",
                      prop.GetLocation(), ref errorOccured);
            }

            if (prop.AccessorList == null || !prop.AccessorList.Accessors.Any(a => a.IsKind(SyntaxKind.GetAccessorDeclaration)))
            {
                context.ReportError(DiagnosticError.MissingSetter, "Missing get accessor",
                        $"Property '{propSymbol.Name}' must declare a getter.",
                    prop.GetLocation(), ref errorOccured);
            }

            if (prop.AccessorList != null && prop.AccessorList.Accessors.Any(a => a.Body != null || a.ExpressionBody != null))
            {
                context.ReportError(DiagnosticError.UnexpectedBody, "Unexpected property body",
                          $"Property '{propSymbol.Name}' can not declare a body.",
                      prop.GetLocation(), ref errorOccured);
            }

            // If it has a second arg, then it must not be GUID-based 
            if (attr.ConstructorArguments.Skip(1).FirstOrDefault().Type is ITypeSymbol defaultType)
            {
                if (!SymbolEqualityComparer.Default.Equals(defaultType, propSymbol.Type))
                {
                    context.ReportError(DiagnosticError.MismatchedType, "Mismatched default value type",
                            $"Property {propSymbol.Name} is of type '{propSymbol.Type}' but has default value of type '{defaultType}'.",
                            prop.GetLocation(), ref errorOccured);
                }
                
                if (propSymbol.Type.TypeKind == TypeKind.Enum)
                {
                    var enumDecls = propSymbol.Type.DeclaringSyntaxReferences.Select(s => s.GetSyntax())
                        .Cast<EnumDeclarationSyntax>();
                    foreach (var enumDecl in enumDecls)
                    {
                        var enumModel = context.Compilation.GetSemanticModel(enumDecl.SyntaxTree);
                        var enumSymbol = enumModel.GetDeclaredSymbol(enumDecl);
                        foreach (var enumMember in enumDecl.Members)
                        {
                            var memberSymbol = enumModel.GetDeclaredSymbol(enumMember);
                            if (memberSymbol == null)
                            {
                                context.ReportError(DiagnosticError.InvalidMembers, "Enum member not found.",
                                    $"Enum member '{enumMember.Identifier.Text}' was not found.",
                                    enumMember.GetLocation(), ref errorOccured);
                                continue;
                            }
                            if (!memberSymbol.GetAttributes().Any(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, selectionOptionAttr)))
                            {
                                context.ReportError(DiagnosticError.UndecoratedProperty, "Undecorated selection option enum member",
                                    $"Enum member '{memberSymbol.Name}' must be decorated with SelectionOptionAttribute.",
                                    enumMember.GetLocation(), ref errorOccured);
                            }
                        }
                    }
                }
            }
            else if (attr.ConstructorArguments.Length == 1 && !SymbolEqualityComparer.Default.Equals(propSymbol.Type, guidType))
            {
                context.ReportError(DiagnosticError.MismatchedType, "Mismatched default value type",
                        $"Property {propSymbol.Name} is of type '{propSymbol.Type}' but needs to be of type '{guidType}'.",
                        prop.GetLocation(), ref errorOccured);
            }
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
