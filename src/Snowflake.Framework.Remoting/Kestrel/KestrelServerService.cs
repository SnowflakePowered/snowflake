using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.IO;
using Snowflake.Extensibility;
using Microsoft.Extensions.Configuration;

namespace Snowflake.Framework.Remoting.Kestrel
{
    internal class KestrelServerService : IKestrelWebServerService
    {
        public KestrelServerService(string appdataDirectory, ILogger logger)
        {
            this.Providers = new ConcurrentDictionary<Type, IKestrelServerMiddlewareProvider>();
            this.WebRoot = Path.Combine(appdataDirectory, "webroot");
            this.Logger = logger;
          //  this.StartServer();
        }

        internal void Start()
        {
            new WebHostBuilder()
                             .UseWebRoot(this.WebRoot)
                             .UseKestrel()
                             
                             .UseUrls("http://localhost:9797")
                             .ConfigureServices(this.ConfigureServices)
                             .Configure(this.Configure)
                             .Build()
                             .StartAsync();
        }

        public string WebRoot { get; }

        public IWebHost WebHost { get; }
        private CancellationTokenSource ServerToken { get; set; }

        private ConcurrentDictionary<Type, IKestrelServerMiddlewareProvider> Providers { get; }

        public ILogger Logger { get; }

        public void AddService<T>(T kestrelServerMiddleware)
            where T: IKestrelServerMiddlewareProvider
        {
            this.Logger.Info($"Adding middleware {typeof(T).Name}");
            this.Providers.TryAdd(typeof(T), kestrelServerMiddleware);
        }

        private void ConfigureServices(IServiceCollection configureServices)
        {
            configureServices.AddCors();
            foreach (var provider in this.Providers.Values)
            {
                provider.ConfigureServices(configureServices);
            }
        }

        private void Configure(IApplicationBuilder app)
        {
            app.UseWebSockets();
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            foreach (var provider in this.Providers.Values)
            {
                provider.Configure(app);
            }
        }
    }
}
