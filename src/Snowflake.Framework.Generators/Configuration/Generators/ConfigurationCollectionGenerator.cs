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
                     ifaceSyntax.GetLocation(), ref errorOccurred);
                    return;
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
                var targetAttrs = new List<AttributeData>();

                // Verify collection props bottom up.
                foreach (var childIface in ifaceSymbol.AllInterfaces.Reverse().Concat(new[] { ifaceSymbol }))
                {
                    // No need to check if child interfaces are partial because we only add to the root interface.
                    foreach (var member in childIface.GetMembers())
                    {
                        if (ConfigurationCollectionGenerator.VerifyCollectionProperty(context, 
                            compilation, types, member, ifaceSymbol, in seenProps, 
                            out var property))
                        {
                            properties.Add((childIface, property!));
                        }
                    }

                    // Add child targets to list while we're at it.
                    targetAttrs.AddRange(childIface.GetAttributes()
                        .Where(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.ConfigurationTargetAttribute)));
                }

                errorOccurred = errorOccurred || !VerifyConfigurationTargets(context, ifaceSymbol, targetAttrs);

                if (errorOccurred)
                    continue;

                string classSource = GenerateSource(ifaceSymbol, properties, types);
                context.AddSource($"{ifaceSymbol.Name}_ConfigurationCollection.cs", SourceText.From(classSource, Encoding.UTF8));
            }
        }

        private static bool VerifyConfigurationTargets(GeneratorExecutionContext context, 
            INamedTypeSymbol ifaceSymbol, List<AttributeData> targetAttrs)
        {
            if (!targetAttrs.Any())
                return true;
            bool errorOccurred = false;
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
                            targetAttr.ApplicationSyntaxReference?.GetSyntax().GetLocation() ?? Location.None, ref errorOccurred);
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
                              targetAttr.ApplicationSyntaxReference?.GetSyntax().GetLocation() ?? Location.None, ref errorOccurred);
                    }
                }
            }
            return !errorOccurred;
        }

        private static bool VerifyCollectionProperty(GeneratorExecutionContext context, 
            Compilation compilation, ConfigurationTypes types, ISymbol member, 
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

            if (property.Type.TypeKind != TypeKind.Interface)
            {
                context.ReportError(DiagnosticError.NotAConfigurationSection,
                   "Configuration collection template members must be interfaces.",
                   $"'{property.Name}' is not an interface.", propertyLocation, ref errorOccurred);
            }

            if (propertySyntax.AccessorList == null || !propertySyntax.AccessorList.Accessors.Any(x => x.IsKind(SyntaxKind.GetAccessorDeclaration)))
            {
                context.ReportError(DiagnosticError.MissingGetter, "Missing get accessor",
               $"Property '{property.Name}' must declare a getter.",
               propertyLocation, ref errorOccurred);
            }

            if (propertySyntax.AccessorList?.Accessors.Any(x => x.IsKind(SyntaxKind.SetAccessorDeclaration)) == true)
            {
                context.ReportError(DiagnosticError.UnexpectedSetter,
                    "Unexpected setter in template property",
                    $"Collection template property '{property.Name}' can not have a setter.", propertyLocation, ref errorOccurred);
            }

            if (!property.Type.DeclaringSyntaxReferences
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
                   $"Template type '{property.Type}' for property '{property.Name}' is not marked with ConfigurationSectionAttribute.", propertyLocation, ref errorOccurred);
            }

            if (property.Type.TypeKind != TypeKind.Interface)
            {
                context.ReportError(DiagnosticError.NotAConfigurationSection,
                   "Configuration collection template members must be interfaces.",
                   $"'{property.Name}' is not an interface.", propertyLocation, ref errorOccurred);
            }

            if (!errorOccurred)
                seenProperties.Add(property.Name);
            propertyResult = property;
            return !errorOccurred;
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
