using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var types = new ConfigurationTypes(compilation);
            if (!types.CheckContext(context, ref errorOccured))
                return;

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

                var targetAttrs = ifaceSymbol.GetAttributes().Where(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.ConfigurationTargetAttribute));

                if (targetAttrs.Any())
                {
                    var rootTargets = new HashSet<string>();
                    var childTargets = new HashSet<(string, string)>();
                    foreach (var targetAttr in targetAttrs)
                    {
                        if (targetAttr.ConstructorArguments.Length == 1)
                        {
                            string rootTarget = (string)targetAttr.ConstructorArguments[0].Value!;
                            if (!rootTargets.Contains(rootTarget))
                            {
                                rootTargets.Add(rootTarget);
                            } 
                            else
                            {
                                context.ReportError(DiagnosticError.DuplicatedTarget,
                                 "Duplicate configuration target declared.",
                                 $"A root configuration target called '{rootTarget}' has already been declared for template interface '{ifaceSymbol.Name}'",
                                    targetAttr.ApplicationSyntaxReference?.GetSyntax().GetLocation() ?? Location.None, ref errorOccured);
                                continue;
                            }
                        } 
                        else if (targetAttr.ConstructorArguments.Length == 2)
                        {
                            (string childTarget, string parentTarget) = ((string)targetAttr.ConstructorArguments[0].Value!, 
                                (string)targetAttr.ConstructorArguments[1].Value!);
                            if (!childTargets.Contains((parentTarget, childTarget)))
                            {
                                childTargets.Add((parentTarget, childTarget));
                            }
                            else
                            {
                                context.ReportError(DiagnosticError.DuplicatedTarget,
                                   "Duplicate configuration target declared.",
                                   $"A configuration target from '{parentTarget}' to '{childTarget}' has already been declared for template interface '{ifaceSymbol.Name}'",
                                      targetAttr.ApplicationSyntaxReference?.GetSyntax().GetLocation() ?? Location.None, ref errorOccured);
                                    continue;
                            }
                        }
                    }
                }
                    
                foreach (var prop in memberSyntax.Cast<PropertyDeclarationSyntax>())
                {
                    var propSymbol = model.GetDeclaredSymbol(prop);

                    // todo: error check prop
                    // (only getter? idk, what are restritions on configcollections?)
                    if (propSymbol == null)
                    {
                        context.ReportError(DiagnosticError.InvalidMembers, "Property not found.",
                         $"Template property '{prop.Identifier.Text}' was not found. " +
                         $"Template property '{prop.Identifier.Text}' was not found.",
                         prop.GetLocation(), ref errorOccured);
                        continue;
                    }


                    if (prop.AccessorList == null || !prop.AccessorList.Accessors.Any(x => x.IsKind(SyntaxKind.GetAccessorDeclaration)))
                    {
                        context.ReportError(DiagnosticError.MissingGetter, "Missing get accessor",
                            $"Property '{propSymbol.Name}' must declare a getter.",
                            prop.GetLocation(), ref errorOccured);
                    }

                    if (prop.AccessorList?.Accessors.Any(x => x.IsKind(SyntaxKind.SetAccessorDeclaration)) == true)
                    {
                        context.ReportError(DiagnosticError.UnexpectedSetter,
                            "Unexpected setter in template property",
                            $"Collection template property '{propSymbol.Name}' can not have a setter.", iface.GetLocation(), ref errorOccured);
                    }

                    if (propSymbol.Type.TypeKind != TypeKind.Interface)
                    {
                        context.ReportError(DiagnosticError.NotAConfigurationSection,
                           "Configuration collection template members must be interfaces.",
                           $"'{propSymbol.Name}' is not an interface.", iface.GetLocation(), ref errorOccured);
                        continue;
                    }

                    if (!propSymbol.Type.DeclaringSyntaxReferences
                        .Select(s => s.GetSyntax())
                        .Where(s => s is InterfaceDeclarationSyntax)
                        .Cast<InterfaceDeclarationSyntax>()
                        .Select(i => compilation.GetSemanticModel(i.SyntaxTree).GetDeclaredSymbol(i))
                        .Where(i => i != null)
                        .SelectMany(i => i!.GetAttributes())
                        .Any(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.ConfigurationSectionAttribute)))
                    {
                        context.ReportError(DiagnosticError.NotAConfigurationSection,
                           "Configuration collection template members must be marked with ConfigurationSectionAttribute.",
                           $"Template type '{propSymbol.Type}' for property '{propSymbol.Name}' is not marked with ConfigurationSectionAttribute.", prop.GetLocation(), ref errorOccured);
                        continue;
                    }

                    symbols.Add(propSymbol);
                }

                if (errorOccured)
                    return;

                string? classSource = ProcessClass(ifaceSymbol, symbols, types, context);
                if (classSource != null)
                {
                    context.AddSource($"{ifaceSymbol.Name}_ConfigurationCollection.cs", SourceText.From(classSource, Encoding.UTF8));
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
            string generatedNamespaceName = $"Snowflake.Configuration.GeneratedConfigurationProxies.Collection_{namespaceName}";
            string tag = RandomString(6);
            string backingClassName = $"{classSymbol.Name}Proxy_{tag}";
            StringBuilder source = new StringBuilder($@"
namespace {namespaceName}
{{
    [{types.ConfigurationGenerationInstanceAttribute.ToDisplayString()}(typeof({generatedNamespaceName}.{backingClassName}))]
    public partial interface {classSymbol.Name}
        : Snowflake.Configuration.Generators.IConfigurationCollectionGeneratedProxy
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
        readonly Snowflake.Configuration.IConfigurationValueCollection __backingCollection;
        readonly Dictionary<string, Snowflake.Configuration.IConfigurationSection> __configurationSections; 

        IReadOnlyDictionary<string, Snowflake.Configuration.IConfigurationSection> 
            Snowflake.Configuration.Generators.IConfigurationCollectionGeneratedProxy.GetValueDictionary() 
                => ImmutableDictionary.CreateRange(__configurationSections);

        private {backingClassName}(Snowflake.Configuration.IConfigurationValueCollection collection) 
        {{
            this.__backingCollection = collection;
            this.__configurationSections = new Dictionary<string, Snowflake.Configuration.IConfigurationSection>();
        
");
            foreach (var prop in props)
            {
                source.Append($@"
this.backing__{prop.Name} = new Snowflake.Configuration.ConfigurationSection<{prop.Type.ToDisplayString()}>(this.__backingCollection, nameof({classSymbol.ToDisplayString()}.{prop.Name})); 
this.__configurationSections[nameof({classSymbol.ToDisplayString()}.{prop.Name})] = this.backing__{prop.Name};
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
            context.RegisterForSyntaxNotifications(() => new ConfigurationTemplateInterfaceSyntaxReceiver("ConfigurationCollection"));
        }
    }
}
