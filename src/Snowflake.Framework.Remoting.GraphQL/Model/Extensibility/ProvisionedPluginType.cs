using HotChocolate.Types;
using Snowflake.Extensibility.Provisioning;
using Snowflake.Extensibility.Provisioning.Standalone;
using Snowflake.Remoting.GraphQL.Model.Configuration;
using Snowflake.Remoting.GraphQL.Model.Filesystem;
using Snowflake.Remoting.GraphQL.Model.Filesystem.Contextual;
using System;
using System.Collections.Generic;
using System.Text;
using Zio;

namespace Snowflake.Remoting.GraphQL.Model.Extensibility
{
    public sealed class ProvisionedPluginType
        : ObjectType<IProvisionedPlugin>
    {
        protected override void Configure(IObjectTypeDescriptor<IProvisionedPlugin> descriptor)
        {
            descriptor.Name("ProvisionedPlugin")
                .Description("Provides access to provisioned plugin resources.");
            descriptor.Field("commonResourceDirectory")
                .Description("Provides filesystem access the plugin's common resource directory.")
                .Argument("directoryPath", a => a.Type<DirectoryPathType>())
                .Type<ContextualDirectoryContentsType>()
                .Resolve(context =>
                {
                    var provision = context.Parent<IProvisionedPlugin>();
                    if (provision.Provision is StandalonePluginProvision) return null;
                    if (context.ArgumentKind("directoryPath") == ValueKind.Null)
                        return provision.Provision.CommonResourceDirectory;
                    var path = context.ArgumentValue<UPath>("directoryPath");
                    return provision.Provision.CommonResourceDirectory.OpenDirectory(path.FullName);
                });
            descriptor.Field("resourceDirectory")
               .Description("Provides filesystem access the plugin's resource directory.")
               .Argument("directoryPath", a => a.Type<DirectoryPathType>())
               .Type<ContextualDirectoryContentsType>()
               .Resolve(context =>
               {
                   var provision = context.Parent<IProvisionedPlugin>();
                   if (provision.Provision is StandalonePluginProvision) return null;
                   if (context.ArgumentKind("directoryPath") == ValueKind.Null)
                       return provision.Provision.ResourceDirectory;
                   var path = context.ArgumentValue<UPath>("directoryPath");
                   return provision.Provision.ResourceDirectory.OpenDirectory(path.FullName);
               });
            descriptor.Field("dataDirectory")
               .Description("Provides filesystem access the plugin's data directory.")
               .Argument("directoryPath", a => a.Type<DirectoryPathType>())
               .Type<ContextualDirectoryContentsType>()
               .Resolve(context =>
               {
                   var provision = context.Parent<IProvisionedPlugin>();
                   if (provision.Provision is StandalonePluginProvision) return null;

                   if (context.ArgumentKind("directoryPath") == ValueKind.Null)
                       return provision.Provision.DataDirectory;
                   var path = context.ArgumentValue<UPath>("directoryPath");
                   return provision.Provision.DataDirectory.OpenDirectory(path.FullName);
               });
            descriptor.Field("configuration")
              .Description("Provides access to the plugin configuration section.")
              .Type<ConfigurationSectionType>()
              .Resolve(context =>
              {
                  var provision = context.Parent<IProvisionedPlugin>();
                  return provision.GetPluginConfiguration();
              });
        }
    }
}
