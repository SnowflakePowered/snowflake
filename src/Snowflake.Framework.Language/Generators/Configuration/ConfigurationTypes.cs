using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Language.Generators.Configuration
{
    internal class ConfigurationTypes
    {
#nullable disable
        public ConfigurationTypes(Compilation compilation)
        {
            this.ConfigurationCollectionAttribute = compilation.GetTypeByMetadataName("Snowflake.Configuration.ConfigurationCollectionAttribute");
            this.InputConfigurationAttribute = compilation.GetTypeByMetadataName("Snowflake.Configuration.InputConfigurationAttribute");

            this.ConfigurationSectionAttribute = compilation.GetTypeByMetadataName("Snowflake.Configuration.ConfigurationSectionAttribute");
            this.ConfigurationOptionAttribute = compilation.GetTypeByMetadataName("Snowflake.Configuration.ConfigurationOptionAttribute");
            this.InputOptionAttribute = compilation.GetTypeByMetadataName("Snowflake.Configuration.Input.InputOptionAttribute");

            this.IConfigurationSection = compilation.GetTypeByMetadataName("Snowflake.Configuration.IConfigurationSection");
            this.IConfigurationSectionGeneric = compilation.GetTypeByMetadataName("Snowflake.Configuration.IConfigurationSection`1");
            this.ConfigurationGenerationInstanceAttribute = compilation.GetTypeByMetadataName("Snowflake.Configuration.Internal.ConfigurationGenerationInstanceAttribute");
            this.SelectionOptionAttribute = compilation.GetTypeByMetadataName("Snowflake.Configuration.SelectionOptionAttribute");

            this.ConfigurationTargetAttribute = compilation.GetTypeByMetadataName("Snowflake.Configuration.ConfigurationTargetAttribute");
            this.ConfigurationTargetMemberAttribute = compilation.GetTypeByMetadataName("Snowflake.Configuration.ConfigurationTargetMemberAttribute");

            this.IConfigurationCollection = compilation.GetTypeByMetadataName("Snowflake.Configuration.IConfigurationCollection");
            this.IConfigurationCollectionGeneric = compilation.GetTypeByMetadataName("Snowflake.Configuration.IConfigurationCollection`1");
            this.IConfigurationSectionDescriptor = compilation.GetTypeByMetadataName("Snowflake.Configuration.IConfigurationSectionDescriptor");
            this.IConfigurationValueCollection = compilation.GetTypeByMetadataName("Snowflake.Configuration.IConfigurationValueCollection");

            this.DeviceCapability = compilation.GetTypeByMetadataName("Snowflake.Input.Device.DeviceCapability");
            this.System_Guid = compilation.GetTypeByMetadataName("System.Guid");

            this.IInputConfigurationTemplate = compilation.GetTypeByMetadataName("Snowflake.Configuration.Internal.IInputConfigurationTemplate");
            this.IConfigurationCollectionTemplate = compilation.GetTypeByMetadataName("Snowflake.Configuration.Internal.IConfigurationCollectionTemplate");

            this.GenericTypeAcceptsConfigurationCollectionAttribute = compilation.GetTypeByMetadataName("Snowflake.Configuration.Internal.GenericTypeAcceptsConfigurationCollectionAttribute");
            this.GenericTypeAcceptsConfigurationSectionAttribute = compilation.GetTypeByMetadataName("Snowflake.Configuration.Internal.GenericTypeAcceptsConfigurationSectionAttribute");
            this.GenericTypeAcceptsInputConfigurationAttribute = compilation.GetTypeByMetadataName("Snowflake.Configuration.Internal.GenericTypeAcceptsInputConfigurationAttribute");

        }

        public bool AllAvailable()
        {
            return this.ConfigurationSectionAttribute != null
                   && this.ConfigurationOptionAttribute != null
                   && this.InputOptionAttribute != null
                   && this.InputConfigurationAttribute != null
                   && this.IConfigurationSection != null
                   && this.IConfigurationSectionGeneric != null
                   && this.ConfigurationGenerationInstanceAttribute != null
                   && this.SelectionOptionAttribute != null
                   && this.ConfigurationTargetAttribute != null
                   && this.ConfigurationTargetMemberAttribute != null
                   && this.IConfigurationCollection != null
                   && this.IConfigurationCollectionGeneric != null
                   && this.DeviceCapability != null
                   && this.System_Guid != null
                   && this.IInputConfigurationTemplate != null
                   && this.IConfigurationCollectionTemplate != null
                   && this.IConfigurationSectionDescriptor != null 
                   && this.IConfigurationValueCollection != null
                   && this.ConfigurationCollectionAttribute != null
                   && this.ConfigurationGenerationInstanceAttribute != null
                   && this.GenericTypeAcceptsConfigurationCollectionAttribute != null
                   && this.GenericTypeAcceptsConfigurationSectionAttribute != null
                   && this.GenericTypeAcceptsInputConfigurationAttribute != null;
        }

        public bool CheckContext(GeneratorExecutionContext context)
        {
            if (this.AllAvailable())
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
