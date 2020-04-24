using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Subscriptions;
using HotChocolate.Configuration;
using HotChocolate.Execution;
using HotChocolate.Execution.Configuration;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using HotChocolate.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
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
using Snowflake.Remoting.Kestrel;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Model.Game;
using HotChocolate.Language;
using Snowflake.Remoting.GraphQL;
using Snowflake.Remoting.GraphQL.Model.Queueing;

namespace Snowflake.Services
{
    internal class HotChocolateKestrelIntegration : IKestrelServerMiddlewareProvider
    {
        public HotChocolateKestrelIntegration(GraphQLSchemaRegistrationProvider schemas, IServiceProvider serviceContainer)
        {
            this.Schemas = schemas;
            this.ServiceContainer = serviceContainer;
        }

        public GraphQLSchemaRegistrationProvider Schemas { get; }
        public IServiceProvider ServiceContainer { get; }

        public void Configure(IApplicationBuilder app)
        {
            app.UseGraphQL("/graphql");
            app.UseGraphQLSubscriptions();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInMemorySubscriptionProvider();

            // Add each query service
            foreach (var sd in this.Schemas.QueryBuilderServices)
            {
                services.Add(sd);
            }

            var kestrelSp = services.BuildServiceProvider();

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

            services.AddDataLoaderRegistry();
            services.AddGraphQLSubscriptions();

            // Add privileged newtypes for Stone
            services.AddTypeConverter<string, PlatformId>(from => from)
                .AddTypeConverter<string, ControllerId>(from => from);

            var schemaBuilder = SchemaBuilder.New()
                .EnableRelaySupport()
                .SetOptions(new SchemaOptions()
                {
                    DefaultBindingBehavior = BindingBehavior.Explicit,
                    UseXmlDocumentation = true,
                    StrictValidation = true,
                })
                .AddQueryType(descriptor =>
                {
                    descriptor.Name("Query");
                })
                .AddMutationType(descriptor =>
                {
                    descriptor.Name("Mutation");
                })
                .AddSubscriptionType(descriptor =>
                {
                    descriptor.Name("Subscription");
                })
                ;

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

            foreach (var config in this.Schemas.SchemaConfig)
            {
                config(schemaBuilder);
            }

            services.AddGraphQL(schemaBuilder.Create(),
                new QueryExecutionOptions
                {
                    TracingPreference = TracingPreference.OnDemand,
                    
                });

            services.AddQueryRequestInterceptor((context, builder, cancel) =>
            {
                builder.SetProperty(SnowflakeGraphQLExtensions.ServicesNamespace, this.ServiceContainer);
                foreach (var config in this.Schemas.QueryConfig)
                {
                    config(builder);
                }
                return Task.CompletedTask;
            });
        }
    }
}
