using Snowflake.Extensibility;
using Snowflake.Loader;
using Snowflake.Remoting.Orchestration;
using Snowflake.Services;
using System.Threading;
using System.Threading.Tasks;

namespace Snowflake.Support.Orchestration.Overlay.Renderer.Windows
{
    public class CefRendererComposable : IComposable
    {
        [ImportService(typeof(IServiceRegistrationProvider))]
        [ImportService(typeof(ILogProvider))]
        public void Compose(IModule composableModule, Loader.IServiceRepository serviceContainer)
        {

            var logger = serviceContainer.Get<ILogProvider>();
            var services = serviceContainer.Get<IServiceRegistrationProvider>();

            var cachePath = composableModule.ContentsDirectory.CreateSubdirectory("cache");
            var browser = new CefSharpBrowserService(logger.GetLogger("cefsharp"), cachePath);
            services.RegisterService<ICefBrowserService>(browser);
            Task.Run(async () => {
                await browser.Initialize();
                browser.Browse(new System.Uri("https://snowflakepowe.red"));
            });
        }
    }
}