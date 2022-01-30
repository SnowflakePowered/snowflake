using CefSharp;
using CefSharp.OffScreen;
using Snowflake.Extensibility;
using Snowflake.Remoting.Orchestration;
using Snowflake.Support.Orchestration.Overlay.Renderer.Windows.Remoting;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snowflake.Support.Orchestration.Overlay.Renderer.Windows
{
    internal class CefSharpBrowserService : ICefBrowserService, IDisposable
    {
        private bool disposedValue;

        public CefSharpBrowserService(ILogger logger, DirectoryInfo cachePath)
        {
            this.Logger = logger;
            this.CacheDirectory = cachePath;
            this.ShutdownEvent = new ManualResetEventSlim();
            this.StartEvent = new ManualResetEventSlim();
            this.InitializedEvent = new SemaphoreSlim(0, 1);
            this.CefThread = new Thread(this.MainCefLoop);
            this.CommandServer = new RendererCommandServer();
            this.CommandServer.Activate();

            this.CefThread.Start();

        }

        public RendererCommandServer CommandServer { get; }
        public ManualResetEventSlim StartEvent { get; }
        public SemaphoreSlim InitializedEvent { get; }
        public ManualResetEventSlim ShutdownEvent { get; }
        public Thread CefThread { get; }
        public ILogger Logger { get; }
        public DirectoryInfo CacheDirectory { get; }
        private ChromiumWebBrowser? Browser { get; set; } = null;
        public Uri? CurrentLocation => this.Browser?.Address != null ? new Uri(this.Browser?.Address) : null;
        private bool Initialized { get; set; }
        public void Browse(Uri uri)
        {
            if (uri.Equals(this.CurrentLocation))
            {
                this.Browser?.Reload();
                return;
            }

            this.Browser?.Load(uri.AbsoluteUri);
        }

        public NamedPipeClientStream GetCommandPipe()
        {
            throw new NotImplementedException();
        }

        private void MainCefLoop()
        {
            this.Logger.Info("Entered CEF Loop thread, waiting for init.");
            this.StartEvent.Wait();
            this.Logger.Info("CEF start event received.");

            CefSettings settings = new()
            {
                CachePath = this.CacheDirectory.FullName,
                RemoteDebuggingPort = 10037,
                // stop CEF from clogging  up the console.
                LogSeverity = LogSeverity.Fatal,
            };
            Cef.EnableHighDPISupport();

            settings.CefCommandLineArgs["autoplay-policy"] = "no-user-gesture-required";
            settings.SetOffScreenRenderingBestPerformanceArgs();
            settings.EnableAudio();
            Cef.Initialize(settings, true, browserProcessHandler: null);
            this.Logger.Info("CEF started.");
            this.Browser = new ChromiumWebBrowser("about:blank");
            this.InitializedEvent.Release();
            this.ShutdownEvent.Wait();
            this.Logger.Info("CEF shutting down...");
            Cef.Shutdown();
            this.Logger.Info("CEF shut down.");
        }

        public async Task Initialize()
        {
            if (this.Initialized)
                return;
            this.StartEvent.Set();
            await this.InitializedEvent.WaitAsync();
            WindowInfo windowInfo = new()
            {
                Width = 100,
                Height = 100,
                WindowlessRenderingEnabled = true,
            };
            windowInfo.SetAsWindowless((nint)0);

            BrowserSettings browserSettings = new() 
            {
                WindowlessFrameRate = 60,
            };
            this.Browser.CreateBrowser(windowInfo, browserSettings);
            this.Initialized = true;
        }

        public void Shutdown()
        {
            this.ShutdownEvent.Set();
            this.CommandServer.Stop();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.Shutdown();
                    this.Logger.Info("Waiting for CEF Thread to exit...");
                    this.CefThread.Join();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
