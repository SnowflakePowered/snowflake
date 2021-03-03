using Microsoft.CodeAnalysis;
using Snowflake.Language.Generators.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Language.Analyzers.Extensibility
{
    internal class ExtensibilityTypes
    {
#nullable disable
        public ExtensibilityTypes(Compilation compilation)
        {
            this.PluginAttribute = compilation.GetTypeByMetadataName("Snowflake.Extensibility.PluginAttribute");
            this.IPlugin = compilation.GetTypeByMetadataName("Snowflake.Extensibility.IPlugin");
            this.IComposable = compilation.GetTypeByMetadataName("Snowflake.Loader.IComposable");
            this.IServiceRepository = compilation.GetTypeByMetadataName("Snowflake.Loader.IServiceRepository");
            this.ImportServiceAttribute = compilation.GetTypeByMetadataName("Snowflake.Loader.ImportServiceAttribute");
        }

        public INamedTypeSymbol PluginAttribute { get; }
        public INamedTypeSymbol IPlugin { get; }
        public INamedTypeSymbol IComposable { get; }
        public INamedTypeSymbol IServiceRepository { get; }
        public INamedTypeSymbol ImportServiceAttribute { get; }

        public bool AllAvailable()
        {
            return this.PluginAttribute != null
                && this.IPlugin != null 
                && this.IComposable != null
                && this.IServiceRepository != null
                && this.ImportServiceAttribute != null;
        }

        public bool CheckContext(GeneratorExecutionContext context)
        {
            if (AllAvailable())
                return true;
            context.ReportFrameworkMissing();
            return false;
        }

#nullable enable
   
    }
}
