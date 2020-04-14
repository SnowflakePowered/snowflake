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
using HotChocolate.Stitching;
using HotChocolate.Types;
using HotChocolate.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.Extensions.DependencyInjection;
using Snowflake.Framework.Remoting.GraphQL.Model.Filesystem;
using Snowflake.Framework.Remoting.GraphQL.Model.Filesystem.Contextual;
using Snowflake.Framework.Remoting.GraphQL.Model.Game;
using Snowflake.Framework.Remoting.GraphQL.Model.PlatformInfo;
using Snowflake.Framework.Remoting.GraphQL.Model.Records;
using Snowflake.Framework.Remoting.GraphQL.Model.Stone;
using Snowflake.Framework.Remoting.GraphQL.Schema;
using Snowflake.Framework.Remoting.Kestrel;
using Snowflake.Input.Controller;
using Snowflake.Model.Game;
using Snowflake.Support.Remoting.GraphQL.RootProvider;

namespace Snowflake.Services
{
    internal class HotChocolateKestrelIntegration : IKestrelServerMiddlewareProvider
    {
        public HotChocolateKestrelIntegration(GraphQLSchemaRegistrationProvider schemas, ServiceContainer serviceContainer)
        {
            this.Schemas = schemas;
            ServiceContainer = serviceContainer;
        }

        public GraphQLSchemaRegistrationProvider Schemas { get; }
        public ServiceContainer ServiceContainer { get; }

        public void Configure(IApplicationBuilder app)
        {
            app.UseWebSockets();
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
            this.Schemas
                .AddScalarType<PlatformIdType>()
                .AddScalarType<ControllerIdType>()
                .AddScalarType<OSFilePathType>()
                .AddScalarType<OSDirectoryPathType>()
                .AddScalarType<DirectoryPathType>()
                .AddScalarType<FilePathType>();

            this.Schemas
                .AddInterfaceType<FileInfoInterface>()
                .AddInterfaceType<DirectoryInfoInterface>()
                .AddInterfaceType<DirectoryContentsInterface>()
                .AddInterfaceType<OSDirectoryInfoInterface>()
                .AddInterfaceType<OSDirectoryContentsInterface>()
                ;

            this.Schemas
                
                .AddObjectType<PlatformInfoType>()
                .AddObjectType<GameType>()
                .AddObjectType<RecordMetadataType>()
                .AddObjectType<GameRecordType>()

                .AddObjectType<ContextualFileInfoType>()
                .AddObjectType<ContextualDirectoryInfoType>()
                .AddObjectType<ContextualDirectoryContentsType>()

                .AddObjectType<OSFileInfoType>()
                .AddObjectType<OSDirectoryInfoType>()
                .AddObjectType<OSDirectoryContentsType>()
                .AddObjectType<OSDriveInfoType>()
                .AddObjectType<OSDriveContentsType>()
                ;

            services.AddDataLoaderRegistry();
            services.AddGraphQLSubscriptions();

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
