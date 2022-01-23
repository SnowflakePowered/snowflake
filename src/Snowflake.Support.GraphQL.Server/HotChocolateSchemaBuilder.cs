using HotChocolate;
using Snowflake.Services;
using System;

using Snowflake.Remoting.GraphQL.Model.Configuration;
using Snowflake.Remoting.GraphQL.Model.Device;
using Snowflake.Remoting.GraphQL.Model.Device.Mapped;
using Snowflake.Remoting.GraphQL.Model.Filesystem;
using Snowflake.Remoting.GraphQL.Model.Filesystem.Contextual;
using Snowflake.Remoting.GraphQL.Model.Game;
using Snowflake.Remoting.GraphQL.Model.Installation;
using Snowflake.Remoting.GraphQL.Model.Orchestration;
using Snowflake.Remoting.GraphQL.Model.Records;
using Snowflake.Remoting.GraphQL.Model.Saving;
using Snowflake.Remoting.GraphQL.Model.Scraping;
using Snowflake.Remoting.GraphQL.Model.Stone.ControllerLayout;
using Snowflake.Remoting.GraphQL.Model.Stone.PlatformInfo;
using Snowflake.Remoting.GraphQL.Model.Queueing;
using HotChocolate.Configuration;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Snowflake.Remoting.GraphQL;
using System.Threading.Tasks;
using Snowflake.Model.Game;
using Snowflake.Input.Controller;
using HotChocolate.Execution.Configuration;
using HotChocolate.Data;
using HotChocolate.Data.Filters;

namespace Snowflake.Support.GraphQL.Server
{
    internal sealed class HotChocolateSchemaBuilder
    {
        public HotChocolateSchemaBuilder(GraphQLSchemaRegistrationProvider schemas, IServiceProvider serviceContainer)
        {
            this.Schemas = schemas;
            this.ServiceContainer = serviceContainer;
        }
        public GraphQLSchemaRegistrationProvider Schemas { get; }
        public IServiceProvider ServiceContainer { get; }

        public IRequestExecutorBuilder AddSnowflakeGraphQl(IRequestExecutorBuilder schemaBuilder, bool queryOnly = false)
        {
            this.Schemas
                .AddObjectType<DummyNodeType>();

            // Stone and Game Model
            this.Schemas
                .AddScalarType<PlatformIdType>()
                .AddScalarType<ControllerIdType>()

                .AddObjectType<PlatformInfoType>()

                .AddEnumType<ControllerElementEnum>()
                .AddEnumType<ControllerElementTypeEnum>()
                .AddObjectType<ControllerElementCollectionType>()
                .AddObjectType<ControllerElementInfoElementType>()
                .AddObjectType<ControllerElementInfoType>()
                .AddObjectType<ControllerLayoutType>()

                .AddObjectType<GameType>()
                .AddObjectType<RecordMetadataType>()
                .AddObjectType<GameRecordType>();

            // Filesystem
            this.Schemas
                .AddScalarType<OSFilePathType>()
                .AddScalarType<OSDirectoryPathType>()
                .AddScalarType<DirectoryPathType>()
                .AddScalarType<FilePathType>()

                .AddInterfaceType<FileInfoInterface>()
                .AddInterfaceType<DirectoryInfoInterface>()
                .AddInterfaceType<DirectoryContentsInterface>()
                .AddInterfaceType<OSDirectoryInfoInterface>()
                .AddInterfaceType<OSDirectoryContentsInterface>()

                .AddObjectType<ContextualFileInfoType>()
                .AddObjectType<ContextualDirectoryInfoType>()
                .AddObjectType<ContextualDirectoryContentsType>()

                .AddObjectType<OSFileInfoType>()
                .AddObjectType<OSDirectoryInfoType>()
                .AddObjectType<OSDirectoryContentsType>()
                .AddObjectType<OSDriveInfoType>()
                .AddObjectType<OSDriveContentsType>();

            this.Schemas
                .AddScalarType<OSTaggedFileSystemPathType>()
                .AddObjectType<InstallableType>();

            // Device
            this.Schemas
                .AddEnumType<DeviceCapabilityEnum>()
                .AddEnumType<InputDriverEnum>()

                .AddObjectType<DeviceCapabilityLabelElementType>()
                .AddObjectType<DeviceCapabilityLabelsType>()
                .AddObjectType<InputDeviceInstanceType>()
                .AddObjectType<InputDeviceType>()

                .AddObjectType<ControllerElementMappingType>()
                .AddObjectType<ControllerElementMappingProfileType>();

            this.Schemas
               .AddEnumType<SaveManagementStrategyEnum>()
               .AddObjectType<SaveGameType>()
               .AddObjectType<SaveProfileType>()
               .AddEnumType<EmulatorCompatibilityEnum>();

            this.Schemas
               .AddEnumType<ConfigurationOptionTypeEnum>()
               .AddEnumType<PathTypeEnum>()

               .AddObjectType<ConfigurationCollectionType>()
               .AddObjectType<ConfigurationSectionType>()

               .AddObjectType<ConfigurationValueType>()
               .AddObjectType<NamedConfigurationValueType>()
               .AddObjectType<OptionDescriptorType>()
               .AddObjectType<OptionMetadataType>()
               .AddObjectType<SectionDescriptorType>()
               .AddObjectType<SelectionOptionDescriptorType>();

            this.Schemas
                .AddObjectType<ScrapeContextType>()
                .AddObjectType<SeedContentType>()
                .AddObjectType<SeedRootContextType>()
                .AddObjectType<SeedType>();
            this.Schemas
              .AddInterfaceType<JobQueueInterface>()
              .AddInterfaceType<QueuableJobInterface>();

            this.Schemas
                .AddObjectType<EmulatedControllerType>()
                .AddObjectType<EmulatedPortDeviceEntryType>()
                .AddObjectType<GameEmulationType>();

            schemaBuilder.AddGlobalObjectIdentification()
                .AddQueryFieldToMutationPayloads()
                .SetOptions(new SchemaOptions()
                {
                    DefaultBindingBehavior = BindingBehavior.Explicit,
                    UseXmlDocumentation = true,
                    StrictValidation = true,
                    RemoveUnreachableTypes = false,
                })
                .AddApolloTracing(HotChocolate.Execution.Options.TracingPreference.OnDemand);

            schemaBuilder.ConfigureSchema(schemaBuilder =>
            {
                schemaBuilder.AddQueryType(descriptor =>
                {
                    descriptor.Name("Query")
                        .Description("The query root of Snowflake's GraphQL interface.");
                });

                if (!queryOnly)
                {
                    schemaBuilder.AddMutationType(descriptor =>
                    {
                        descriptor.Name("Mutation")
                            .Description("The mutation root of Snowflake's GraphQL interface. " +
                            "Snowflake provides mutations that implement Relay Input Object Mutations Specifications.");
                    })
                    .AddSubscriptionType(descriptor =>
                    {
                        descriptor.Name("Subscription")
                            .Description("The subscription root of Snowflake's GraphQL interface.");
                    });
                }

                foreach (var type in this.Schemas.ScalarTypes)
                {
                    schemaBuilder.AddType(type);
                }

                foreach (var type in this.Schemas.EnumTypes)
                {
                    schemaBuilder.AddType(type);
                }

                foreach (var type in this.Schemas.InterfaceTypes)
                {
                    schemaBuilder.AddType(type);
                }

                foreach (var type in this.Schemas.ObjectTypes)
                {
                    schemaBuilder.AddType(type);
                }

                foreach (var type in this.Schemas.ObjectTypeExtensions)
                {
                    schemaBuilder.AddType(type);
                }
            });
            foreach (var config in this.Schemas.SchemaConfig)
            {
                config(schemaBuilder);
            }
            schemaBuilder.AddFiltering(configure =>
            {
                configure.AddDefaults();
                configure.BindRuntimeType<PlatformId, ComparableOperationFilterInputType<string>>();
                configure.BindRuntimeType<ControllerId, ComparableOperationFilterInputType<string>>();

            });
            return schemaBuilder;
        }
        
        public void AddSnowflakeQueryRequestInterceptor(IRequestExecutorBuilder services)
        {
            services.AddHttpRequestInterceptor((context, executor, builder, ct) =>
            {
                builder.SetProperty(SnowflakeGraphQLExtensions.ServicesNamespace, this.ServiceContainer);
                foreach (var config in this.Schemas.QueryConfig)
                {
                    config(builder);
                }
                return ValueTask.CompletedTask;
            });
        }

        public void AddStoneIdTypeConverters(IRequestExecutorBuilder services)
        {
            services.BindRuntimeType<PlatformId, PlatformIdType>()
                .BindRuntimeType<ControllerId, ControllerIdType>();

            services
               .AddTypeConverter<string, PlatformId>(from => from)
               .AddTypeConverter<PlatformId, string>(from => from)
               .AddTypeConverter<string, ControllerId>(from => from)
               .AddTypeConverter<ControllerId, string>(from => from);
        }
    }
}
