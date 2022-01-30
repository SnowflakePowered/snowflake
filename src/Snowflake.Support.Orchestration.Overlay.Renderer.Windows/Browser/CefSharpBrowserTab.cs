using CefSharp;
using CefSharp.OffScreen;
using Snowflake.Remoting.Orchestration;
using Snowflake.Support.Orchestration.Overlay.Renderer.Windows.Remoting;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Support.Orchestration.Overlay.Renderer.Windows.Browser
{
    internal class CefSharpBrowserTab : IBrowserTab
    {
        private bool disposedValue;

        public CefSharpBrowserTab(Guid tabGuid)
        {
            this.TabGuid = tabGuid;
        }

        private ChromiumWebBrowser Browser { get; set; }

        private bool Initialized { get; set; } = false;
        public Uri? CurrentLocation => this.Browser?.Address != null ? new Uri(this.Browser?.Address) : null;

        public RendererCommandServer CommandServer { get; private set; }
        public Guid TabGuid { get; }

        public NamedPipeClientStream GetCommandPipe()
        {
            throw new NotImplementedException();
        }

        public async Task InitializeAsync(Uri uri)
        {
            if (this.Initialized || this.disposedValue)
                return;

            this.CommandServer = new RendererCommandServer(this.TabGuid);
            this.CommandServer.Activate();
            this.Browser = new ChromiumWebBrowser(uri.AbsoluteUri);
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
            await this.Browser.CreateBrowserAsync(windowInfo, browserSettings);
            this.Initialized = true;
        }

        public void Navigate(Uri uri)
        {
            if (uri.Equals(this.CurrentLocation))
            {
                this.Browser?.Reload();
                return;
            }

            this.Browser?.Load(uri.AbsoluteUri);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.Browser.Dispose();
                    this.CommandServer.Stop();
                }
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
