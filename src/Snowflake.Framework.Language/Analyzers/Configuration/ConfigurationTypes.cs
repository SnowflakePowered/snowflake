using Microsoft.CodeAnalysis;
using Snowflake.Language.Generators.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Language.Analyzers.Configuration
{
    internal class ConfigurationTypes
    {
#nullable disable
        public ConfigurationTypes(Compilation compilation)
        {
            ConfigurationCollectionAttribute = compilation.GetTypeByMetadataName("Snowflake.Configuration.ConfigurationCollectionAttribute");
            InputConfigurationAttribute = compilation.GetTypeByMetadataName("Snowflake.Configuration.InputConfigurationAttribute");

            ConfigurationSectionAttribute = compilation.GetTypeByMetadataName("Snowflake.Configuration.ConfigurationSectionAttribute");
            ConfigurationOptionAttribute = compilation.GetTypeByMetadataName("Snowflake.Configuration.ConfigurationOptionAttribute");
            InputOptionAttribute = compilation.GetTypeByMetadataName("Snowflake.Configuration.Input.InputOptionAttribute");

            IConfigurationSection = compilation.GetTypeByMetadataName("Snowflake.Configuration.IConfigurationSection");
            IConfigurationSectionGeneric = compilation.GetTypeByMetadataName("Snowflake.Configuration.IConfigurationSection`1");
            ConfigurationGenerationInstanceAttribute = compilation.GetTypeByMetadataName("Snowflake.Configuration.Internal.ConfigurationGenerationInstanceAttribute");
            SelectionOptionAttribute = compilation.GetTypeByMetadataName("Snowflake.Configuration.SelectionOptionAttribute");

            ConfigurationTargetAttribute = compilation.GetTypeByMetadataName("Snowflake.Configuration.ConfigurationTargetAttribute");
            ConfigurationTargetMemberAttribute = compilation.GetTypeByMetadataName("Snowflake.Configuration.ConfigurationTargetMemberAttribute");

            IConfigurationCollection = compilation.GetTypeByMetadataName("Snowflake.Configuration.IConfigurationCollection");
            IConfigurationCollectionGeneric = compilation.GetTypeByMetadataName("Snowflake.Configuration.IConfigurationCollection`1");
            IConfigurationSectionDescriptor = compilation.GetTypeByMetadataName("Snowflake.Configuration.IConfigurationSectionDescriptor");
            IConfigurationValueCollection = compilation.GetTypeByMetadataName("Snowflake.Configuration.IConfigurationValueCollection");

            DeviceCapability = compilation.GetTypeByMetadataName("Snowflake.Input.Device.DeviceCapability");
            System_Guid = compilation.GetTypeByMetadataName("System.Guid");

            IInputConfigurationTemplate = compilation.GetTypeByMetadataName("Snowflake.Configuration.Internal.IInputConfigurationTemplate");
            IConfigurationCollectionTemplate = compilation.GetTypeByMetadataName("Snowflake.Configuration.Internal.IConfigurationCollectionTemplate");

            GenericTypeAcceptsConfigurationCollectionAttribute = compilation.GetTypeByMetadataName("Snowflake.Configuration.Internal.GenericTypeAcceptsConfigurationCollectionAttribute");
            GenericTypeAcceptsConfigurationSectionAttribute = compilation.GetTypeByMetadataName("Snowflake.Configuration.Internal.GenericTypeAcceptsConfigurationSectionAttribute");
            GenericTypeAcceptsInputConfigurationAttribute = compilation.GetTypeByMetadataName("Snowflake.Configuration.Internal.GenericTypeAcceptsInputConfigurationAttribute");
        }

        public bool AllAvailable()
        {
            return ConfigurationSectionAttribute != null
                   && ConfigurationOptionAttribute != null
                   && InputOptionAttribute != null
                   && InputConfigurationAttribute != null
                   && IConfigurationSection != null
                   && IConfigurationSectionGeneric != null
                   && ConfigurationGenerationInstanceAttribute != null
                   && SelectionOptionAttribute != null
                   && ConfigurationTargetAttribute != null
                   && ConfigurationTargetMemberAttribute != null
                   && IConfigurationCollection != null
                   && IConfigurationCollectionGeneric != null
                   && DeviceCapability != null
                   && System_Guid != null
                   && IInputConfigurationTemplate != null
                   && IConfigurationCollectionTemplate != null
                   && IConfigurationSectionDescriptor != null
                   && IConfigurationValueCollection != null
                   && ConfigurationCollectionAttribute != null
                   && ConfigurationGenerationInstanceAttribute != null
                   && GenericTypeAcceptsConfigurationCollectionAttribute != null
                   && GenericTypeAcceptsConfigurationSectionAttribute != null
                   && GenericTypeAcceptsInputConfigurationAttribute != null;
        }

        public bool CheckContext(GeneratorExecutionContext context)
        {
            if (AllAvailable())
                return true;
            context.ReportFrameworkMissing();
            return false;
        }

#nullable enable
        public INamedTypeSymbol ConfigurationCollectionAttribute { get; }
        public INamedTypeSymbol InputConfigurationAttribute { get; }
        public INamedTypeSymbol ConfigurationSectionAttribute { get; }
        public INamedTypeSymbol ConfigurationOptionAttribute { get; }
        public INamedTypeSymbol InputOptionAttribute { get; }
        public INamedTypeSymbol IConfigurationSection { get; }
        public INamedTypeSymbol IConfigurationSectionGeneric { get; }
        public INamedTypeSymbol ConfigurationGenerationInstanceAttribute { get; }
        public INamedTypeSymbol SelectionOptionAttribute { get; }
        public INamedTypeSymbol ConfigurationTargetAttribute { get; }
        public INamedTypeSymbol ConfigurationTargetMemberAttribute { get; }
        public INamedTypeSymbol IConfigurationCollection { get; }
        public INamedTypeSymbol IConfigurationCollectionGeneric { get; }
        public INamedTypeSymbol IConfigurationSectionDescriptor { get; }
        public INamedTypeSymbol IConfigurationValueCollection { get; }
        public INamedTypeSymbol DeviceCapability { get; }
        public INamedTypeSymbol System_Guid { get; }
        public INamedTypeSymbol IInputConfigurationTemplate { get; }
        public INamedTypeSymbol IConfigurationCollectionTemplate { get; }
        public INamedTypeSymbol GenericTypeAcceptsConfigurationCollectionAttribute { get; }
        public INamedTypeSymbol GenericTypeAcceptsConfigurationSectionAttribute { get; }
        public INamedTypeSymbol GenericTypeAcceptsInputConfigurationAttribute { get; }
    }
}
