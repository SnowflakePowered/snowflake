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
using HotChocolate.Types;
using HotChocolate.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Snowflake.Framework.Remoting.GraphQL.Model.Device;
using Snowflake.Framework.Remoting.GraphQL.Model.Device.Mapped;
using Snowflake.Framework.Remoting.GraphQL.Model.Filesystem;
using Snowflake.Framework.Remoting.GraphQL.Model.Filesystem.Contextual;
using Snowflake.Framework.Remoting.GraphQL.Model.Game;
using Snowflake.Framework.Remoting.GraphQL.Model.Installation;
using Snowflake.Framework.Remoting.GraphQL.Model.Records;
using Snowflake.Framework.Remoting.GraphQL.Model.Saving;
using Snowflake.Framework.Remoting.GraphQL.Model.Stone;
using Snowflake.Framework.Remoting.GraphQL.Model.Stone.ControllerLayout;
using Snowflake.Framework.Remoting.GraphQL.Model.Stone.PlatformInfo;
using Snowflake.Framework.Remoting.GraphQL.Schema;
using Snowflake.Framework.Remoting.Kestrel;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Model.Game;
using Snowflake.Support.Remoting.GraphQL.RootProvider;

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
            app.UseGraphQL("/hotchocolate");
        }

        public void ConfigureServices(IServiceCollection services)
        {
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
                .AddObjectType<GameRecordType>()
                ;

            // Filesytem
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
                .AddObjectType<OSDriveContentsType>()
                ;

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
                .AddObjectType<ControllerElementMappingProfileType>()
                ;

            this.Schemas
               .AddEnumType<SaveManagementStrategyEnum>()
               .AddObjectType<SaveGameType>()
               .AddObjectType<SaveProfileType>()
               ;

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
                });

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

            services.AddGraphQL(schemaBuilder.Create());
           
            services.AddQueryRequestInterceptor((context, builder, cancel) =>
            {
                builder.TrySetServices(context.RequestServices.Include(this.ServiceContainer));
                return Task.CompletedTask;
            });
        }
    }
}
