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


            foreach (var rootInterface in receiver.CandidateInterfaces)
            {
                errorOccurred = false;
                var symbols = new List<IPropertySymbol>();
                var model = compilation.GetSemanticModel(rootInterface.SyntaxTree);
                var ifaceSymbol = model.GetDeclaredSymbol(rootInterface);
                if (ifaceSymbol == null)
                {
                    context.ReportError(DiagnosticError.InvalidMembers, "Interface not found.",
                         $"Template interface '{rootInterface.Identifier.Text}' was not found. " +
                         $"Template interface '{rootInterface.Identifier.Text}' was not found.",
                         Location.None, ref errorOccurred);
                    continue;
                }

                var allInterfaces = ifaceSymbol.AllInterfaces;
  
                // todo ensure not null
                var allDecls = ifaceSymbol.DeclaringSyntaxReferences;
                // get partials..
                var memberSyntax = allDecls.Select(s => s.GetSyntax())
                    .Cast<InterfaceDeclarationSyntax>().SelectMany(i => i.Members);

                ConfigurationSectionGenerator.VerifyTemplateInterface(context, model, memberSyntax, rootInterface.Identifier.Text, ifaceSymbol, ref errorOccurred);
                if (errorOccurred)
                    continue;

                foreach (var prop in memberSyntax.Cast<PropertyDeclarationSyntax>())
                {
                    var propSymbol = model.GetDeclaredSymbol(prop);
                    if (propSymbol == null)
                    {
                        context.ReportError(DiagnosticError.InvalidMembers, "Property not found.",
                         $"Template property '{prop.Identifier.Text}' was not found. " +
                         $"Template property '{prop.Identifier.Text}' was not found.",
                         prop.GetLocation(), ref errorOccurred);
                        continue;
                    }
                   
                    ConfigurationSectionGenerator.VerifyOptionProperty(context, prop, propSymbol, types, ref errorOccurred);
                    if (!errorOccurred) 
                        symbols.Add(propSymbol);
                }

                if (errorOccurred)
                    continue;

                string? classSource = ProcessClass(ifaceSymbol!, symbols, types, context);
                if (classSource != null)
                {
                    context.AddSource($"{ifaceSymbol!.Name}_ConfigurationSection.cs", SourceText.From(classSource, Encoding.UTF8));
                }
            }
        }

        
        private string? ProcessClass(INamedTypeSymbol classSymbol, List<IPropertySymbol> props,
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

            source.Append(@"
#pragma warning restore CS0472
#pragma warning restore CS8073
}}");
            return source.ToString();
        }

        private static Random random = new Random();

        internal static void VerifyOptionProperty(
            GeneratorExecutionContext context,
            PropertyDeclarationSyntax prop, IPropertySymbol propSymbol,
            ConfigurationTypes types,
            ref bool errorOccurred)
        {
            var attrs = propSymbol.GetAttributes()
                       .Where(attr => attr?.AttributeClass?.Equals(types.ConfigurationOptionAttribute, SymbolEqualityComparer.Default) == true);

            if (!attrs.Any())
            {
                context.ReportError(DiagnosticError.UndecoratedProperty, "Undecorated section property member",
                           $"Property {propSymbol.Name} must be decorated with a ConfigurationOptionAttribute.",
                       prop.GetLocation(), ref errorOccurred);
            }
            if (prop.AccessorList == null || !prop.AccessorList.Accessors.Any(a => a.IsKind(SyntaxKind.SetAccessorDeclaration)))
            {
                context.ReportError(DiagnosticError.MissingSetter, "Missing set accessor",
                          $"Property '{propSymbol.Name}' must declare a setter.",
                      prop.GetLocation(), ref errorOccurred);
            }

            if (prop.AccessorList == null || !prop.AccessorList.Accessors.Any(a => a.IsKind(SyntaxKind.GetAccessorDeclaration)))
            {
                context.ReportError(DiagnosticError.MissingSetter, "Missing get accessor",
                        $"Property '{propSymbol.Name}' must declare a getter.",
                    prop.GetLocation(), ref errorOccurred);
            }

            if (prop.AccessorList != null && prop.AccessorList.Accessors.Any(a => a.Body != null || a.ExpressionBody != null))
            {
                context.ReportError(DiagnosticError.UnexpectedBody, "Unexpected property body",
                          $"Property '{propSymbol.Name}' can not declare a body.",
                      prop.GetLocation(), ref errorOccurred);
            }
            if (!attrs.Any())
                return;
            var attr = attrs.First();
            // If it has a second arg, then it must not be GUID-based 
            if (attr.ConstructorArguments.Skip(1).FirstOrDefault().Type is ITypeSymbol defaultType)
            {
                if (!SymbolEqualityComparer.Default.Equals(defaultType, propSymbol.Type))
                {
                    context.ReportError(DiagnosticError.MismatchedType, "Mismatched default value type",
                            $"Property {propSymbol.Name} is of type '{propSymbol.Type}' but has default value of type '{defaultType}'.",
                            prop.GetLocation(), ref errorOccurred);
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
                                    enumMember.GetLocation(), ref errorOccurred);
                                continue;
                            }
                            if (!memberSymbol.GetAttributes().Any(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.SelectionOptionAttribute)))
                            {
                                context.ReportError(DiagnosticError.UndecoratedProperty, "Undecorated selection option enum member",
                                    $"Enum member '{memberSymbol.Name}' must be decorated with SelectionOptionAttribute.",
                                    enumMember.GetLocation(), ref errorOccurred);
                            }
                        }
                    }
                }
            }
            else if (attr.ConstructorArguments.Length == 1 && !SymbolEqualityComparer.Default.Equals(propSymbol.Type, types.System_Guid))
            {
                context.ReportError(DiagnosticError.MismatchedType, "Mismatched default value type",
                        $"Property {propSymbol.Name} is of type '{propSymbol.Type}' but needs to be of type '{types.System_Guid}'.",
                        prop.GetLocation(), ref errorOccurred);
            }
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

            if (memberSyntax.FirstOrDefault(m => m is not PropertyDeclarationSyntax) is MemberDeclarationSyntax badSyntax)
            {
                var badSymbol = model.GetDeclaredSymbol(badSyntax);
                context.ReportError(DiagnosticError.InvalidMembers, "Invalid members in template interface.",
                    $"Template interface '{ifaceSymbol.Name}' must only declare property members. " +
                    $"{badSymbol?.Kind} '{ifaceSymbol.Name}.{badSymbol?.Name}' is not a property.",
                    badSyntax.GetLocation(), ref errorOccurred);
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
