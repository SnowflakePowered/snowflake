using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Snowflake.Configuration;
using Snowflake.Framework.Remoting.GraphQL;
using Snowflake.Framework.Remoting.Kestrel;
using Snowflake.Input;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Loader;
using Snowflake.Services.Logging;
using Snowflake.Support.Remoting.GraphQL.RootProvider;
using Zio;
using Zio.FileSystems;

namespace Snowflake.Services
{
    /// <inheritdoc />
    internal class ServiceContainer : IServiceContainer
    {
        #region Loaded Objects

        /// <inheritdoc/>
        public string AppDataDirectory { get; }

        private readonly IDictionary<Type, object> serviceContainer;

        #endregion

        // Flag: Has Dispose already been called?
        bool disposed;

        /// <inheritdoc />
        public ServiceContainer(string appDataDirectory, string kestrelHostname)
        {
            this.AppDataDirectory = appDataDirectory;
            var directoryProvider = new ContentDirectoryProvider(this.AppDataDirectory);
            this.serviceContainer = new ConcurrentDictionary<Type, object>();

            this.RegisterService<ILogProvider>(new LogProvider());
            this.RegisterService<IModuleEnumerator>(new ModuleEnumerator(appDataDirectory));
            this.RegisterService<IKestrelWebServerService>(new KestrelServerService(appDataDirectory,
                kestrelHostname,
                this.Get<ILogProvider>().GetLogger("kestrel")));

            this.RegisterGraphQLRootSchema();

            this.RegisterService<IContentDirectoryProvider>(directoryProvider);
            this.RegisterService<IServiceRegistrationProvider>(new ServiceRegistrationProvider(this));
            this.RegisterService<IServiceEnumerator>(new ServiceEnumerator(this));
            this.RegisterService<IFileSystem>(new PhysicalFileSystem());
        }

        private void RegisterGraphQLRootSchema()
        {
            var schema = new GraphQLRootSchema();
            this.RegisterService<IGraphQLService>(schema);
            var kestrel = this.Get<IKestrelWebServerService>();
            kestrel.AddService(new GraphQLKestrelIntegration(schema));
        }

        /// <inheritdoc/>
        public void RegisterService<T>(T serviceObject)
        {
            if (this.serviceContainer.ContainsKey(typeof(T)))
            {
                throw new ArgumentException($"A service of type {typeof(T).AssemblyQualifiedName} already exists!");
            }

            this.serviceContainer.Add(typeof(T), serviceObject);
        }

        /// <inheritdoc/>
        public IEnumerable<string> AvailableServices()
        {
            return this.serviceContainer.Keys.Select(service => service.FullName);
        }

        /// <inheritdoc/>
        public T Get<T>()
        {
            // todo throw?
            return this.serviceContainer.ContainsKey(typeof(T)) ? (T) this.serviceContainer[typeof(T)] : default;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.Get<IPluginManager>().Dispose();
#pragma warning disable S1215 // "GC.Collect" should not be called
                GC.Collect();
#pragma warning restore S1215 // "GC.Collect" should not be called
            }

            // Free any unmanaged objects here.
            this.disposed = true;
        }
    }
}
