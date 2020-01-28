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
        public KestrelServerService(string appdataDirectory, string hostname, ILogger logger)
        {
            this.Hostname = hostname;
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
                             
                             .UseUrls(this.Hostname)
                             .ConfigureServices(this.ConfigureServices)
                             .Configure(this.Configure)
                             .Build()
                             .StartAsync();
        }

        public string WebRoot { get; }

        public IWebHost WebHost { get; }
        private CancellationTokenSource ServerToken { get; set; }
        public string Hostname { get; }
        private ConcurrentDictionary<Type, IKestrelServerMiddlewareProvider> Providers { get; }

        public ILogger Logger { get; }

        public void AddService<T>(T kestrelServerMiddleware)
            where T: IKestrelServerMiddlewareProvider
        {
            this.Logger.Info($"Adding middleware {typeof(T).Name}");
            this.Providers.TryAdd(typeof(T), kestrelServerMiddleware);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddResponseCompression();
            foreach (var provider in this.Providers.Values)
            {
                provider.ConfigureServices(services);
            }
        }

        private void Configure(IApplicationBuilder app)
        {
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseResponseCompression();

            foreach (var provider in this.Providers.Values)
            {
                provider.Configure(app);
            }
        }
    }
}
