using CefSharp;
using CefSharp.OffScreen;
using Snowflake.Extensibility;
using Snowflake.Remoting.Orchestration;
using Snowflake.Support.Orchestration.Overlay.Renderer.Windows.Remoting;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silk.NET.Direct3D11;
using CefSharp.Structs;
using Snowflake.Orchestration.Ingame;
using Evergine.Bindings.RenderDoc;

namespace Snowflake.Support.Orchestration.Overlay.Renderer.Windows.Browser
{
    internal class CefSharpBrowserTab : IBrowserTab
    {
        private bool disposedValue;

        public CefSharpBrowserTab(ILogger logger, Guid tabGuid, Direct3DDevice device, Evergine.Bindings.RenderDoc.RenderDoc renderDoc)
        {
            this.Logger = logger;
            this.TabGuid = tabGuid;
            this.Device = device;
            RenderDoc = renderDoc;
        }

        private ChromiumWebBrowser Browser { get; set; }
        private bool Initialized { get; set; } = false;
        public Uri? CurrentLocation => this.Browser?.Address != null ? new Uri(this.Browser?.Address) : null;
        public IngameCommandController CommandServer { get; private set; }
        public ILogger Logger { get; }
        public Guid TabGuid { get; }
        public Direct3DDevice Device { get; }
        public RenderDoc RenderDoc { get; }
        private D3DSharedTextureRenderHandler Renderer { get; set; }

        public NamedPipeClientStream GetCommandPipe()
        {
            throw new NotImplementedException();
        }

        public async Task InitializeAsync(Uri uri)
        {
            if (this.Initialized || this.disposedValue)
                return;

            this.CommandServer = new IngameCommandController(this.Logger, this.TabGuid);
            this.CommandServer.Start();
            this.Renderer = new D3DSharedTextureRenderHandler(this.Device, this.CommandServer, this.RenderDoc);
            this.Renderer.Resize(new(300, 300));
            this.Browser = new ChromiumWebBrowser(uri.AbsoluteUri);

            this.Browser.RenderHandler = this.Renderer;
            this.CommandServer.CommandReceived += (cmd) =>
            {
                switch (cmd.Type)
                {
                    case GameWindowCommandType.WindowResizeEvent:
                        System.Drawing.Size size = new(cmd.ResizeEvent.Width, cmd.ResizeEvent.Height);
                        this.Renderer.Resize(size);
                        this.Browser.Size = size;
                        this.Browser.GetBrowserHost().Invalidate(PaintElementType.View);
                        break;
                    case GameWindowCommandType.OverlayTextureEvent:
                        this.Browser.GetBrowserHost().Invalidate(PaintElementType.View);
                        break;

                }
            };

            WindowInfo windowInfo = new()
            {
                Width = 300,
                Height = 300,
                WindowlessRenderingEnabled = true,
            };
            windowInfo.SetAsWindowless((nint)0);

            BrowserSettings browserSettings = new()
            {
                WindowlessFrameRate = 60,
            };
            await this.Browser.CreateBrowserAsync(windowInfo, browserSettings);
            await this.Browser.WaitForInitialLoadAsync();
            this.Browser.Size = new(300, 300);

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
