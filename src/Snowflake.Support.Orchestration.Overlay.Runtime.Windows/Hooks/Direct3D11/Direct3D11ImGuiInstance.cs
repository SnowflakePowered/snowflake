using ImGui = DearImguiSharp;
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;
using Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Render;
using System.Runtime.CompilerServices;


namespace Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks.Direct3D11
{
    internal class Direct3D11ImGuiInstance : IDisposable
    {
        public unsafe ID3D11RenderTargetView* renderTargetView = null;
        private nint outputWindowHandle = 0;
        private bool disposedValue;

        private bool DevicesReady { get; set; }
        private bool SurfacesReady { get; set; }
        private bool SwapchainReady { get; set; } = false;
        public ImGui.ImGuiContext Context { get; }
        private ImGuiWndProcHandler WndProc { get; }

        public Direct3D11ImGuiInstance()
        {
            this.Context = ImGui.ImGui.CreateContext(null);
            this.WndProc = new ImGuiWndProcHandler();
            ImGui.ImGui.StyleColorsDark(null);
        }

        public unsafe void DiscardSwapchain()
        {
            this.InvalidateRenderTarget();
            ImGui.ImGui.ImGuiImplDX11InvalidateDeviceObjects();
            this.SwapchainReady = false;
        }

        private void RefreshSwapchain()
        {
            ImGui.ImGui.ImGuiImplDX11CreateDeviceObjects();
            this.SwapchainReady = true;
        }

        private unsafe void InvalidateRenderTarget()
        {
            if (this.renderTargetView == null)
                return;

            this.renderTargetView->Release();
            this.renderTargetView = null;
            this.SurfacesReady = false;
        }

        public unsafe void RefreshTargetView(ComPtr<IDXGISwapChain> swapChain)
        {
            this.InvalidateRenderTarget();

            SwapChainDesc desc = new();
            swapChain.Ref.GetDesc(ref desc);

            using var device = swapChain.Cast<ID3D11Device>(static (p, g, o) => p->GetDevice(g, o), ID3D11Device.Guid, static d => d->Release());

            using var backBuffer =
                      swapChain.Cast<ID3D11Texture2D>(static (p, g, o) => p->GetBuffer(0, g, o), ID3D11Texture2D.Guid, static b => b->Release());

            //using var backBufferResource =
            //    backBuffer.Cast<ID3D11Resource>(static (p, g, o) => p->QueryInterface(g, o), ID3D11Resource.Guid, static p => p->Release());

            // CRT accepts a Tex2D pointer just fine.
            ID3D11RenderTargetView* newRenderTargetView = null;
            device.Ref.CreateRenderTargetView((ID3D11Resource*)~backBuffer, null, &newRenderTargetView);
            this.renderTargetView = newRenderTargetView;
            this.SurfacesReady = true;

            if (!this.SwapchainReady)
            {
                this.RefreshSwapchain();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe void InitializeDevices(ComPtr<IDXGISwapChain> swapChain, nint hWnd)
        {
            using var device = swapChain.Cast<ID3D11Device>(static (p, g, o) => p->GetDevice(g, o),
                    ID3D11Device.Guid, static d => d->Release());
            using var deviceContext = device.Cast<ID3D11DeviceContext>(static (p, o) => p->GetImmediateContext(o), static r => r->Release());

            this.WndProc.InitializeIO(hWnd);
            ImGui.ImGui.ImGuiImplDX11Init(new(~device), new(~deviceContext));
            this.DevicesReady = true;
        }

        private void InvalidateDevices()
        {
            this.WndProc.InvalidateIO();
            ImGui.ImGui.ImGuiImplDX11Shutdown();
            this.DevicesReady = false;
        }

        public unsafe bool PrepareForPaint(ComPtr<IDXGISwapChain> swapChain)
        {
            SwapChainDesc desc = new();
            swapChain.Ref.GetDesc(ref desc);

            if (desc.OutputWindow != this.outputWindowHandle)
            {
                Console.WriteLine("Swapchain outdated and so discarded.");
                this.DiscardSwapchain();
                this.InvalidateDevices();
            }

            if (!this.DevicesReady)
            {
                this.InitializeDevices(swapChain, desc.OutputWindow);
            }

            if (!this.SurfacesReady)
            {
                this.RefreshTargetView(swapChain);
            }

            this.outputWindowHandle = desc.OutputWindow;
            return true;
        }

        public unsafe void SetRenderTargets(ComPtr<ID3D11DeviceContext> deviceContext)
        {
            deviceContext.Ref.OMSetRenderTargets(1, ref renderTargetView, null);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    unsafe 
                    {
                        if (renderTargetView != null)
                            renderTargetView->Release();
                    }
                    this.Context?.Dispose();
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
