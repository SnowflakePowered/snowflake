using Silk.NET.Direct3D11;
using Silk.NET.DXGI;
using Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Render;
using System.Runtime.CompilerServices;
using ImGuiBackends.Direct3D11;
using ImGuiNET;
using System.Numerics;
using Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks.WndProc;

namespace Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks.Direct3D11
{
    internal class Direct3D11ImGuiInstance : IDisposable
    {
        //public unsafe ID3D11RenderTargetView* renderTargetView = null;
        private nint outputWindowHandle = 0;
        private bool disposedValue;

        private bool DevicesReady { get; set; }
        private bool SurfacesReady { get; set; }
        private bool SwapchainReady { get; set; } = false;
        public ImGuiBackendDirect3D11 Backend { get; }
        public Win32Backend? Input { get; set; }
        IntPtr context;

        public Direct3D11ImGuiInstance()
        {
            context = ImGui.CreateContext();
            ImGui.SetCurrentContext(context);
            ImGui.StyleColorsDark();

            this.Backend = new ImGuiBackendDirect3D11();
        }

        public void NewFrame()
        {
            this.Backend.NewFrame();
        }

        public void Render(ImDrawDataPtr drawData)
        {
            this.Backend.RenderDrawData(drawData);
        }

        private void RefreshSwapchain()
        {
            if (!this.Backend.CreateDeviceObjects())
            {
                Console.WriteLine("Error when creating objects");
            }
            //ImGui.ImGui.ImGuiImplDX11CreateDeviceObjects();
            this.SwapchainReady = true;
        }

        //public unsafe void InvalidateRenderTarget()
        //{
        //    if (this.renderTargetView == null)
        //        return;

        //    this.renderTargetView->Release();
        //    this.renderTargetView = null;
        //    this.SurfacesReady = false;
        //}

        //public unsafe void RefreshTargetView(ComPtr<IDXGISwapChain> swapChain)
        //{
        //    this.InvalidateRenderTarget();

        //    using var device = swapChain.Cast<ID3D11Device>(static (p, g, o) => p->GetDevice(g, o), ID3D11Device.Guid, static d => d->Release());

        //    using var backBuffer =
        //              swapChain.Cast<ID3D11Texture2D>(static (p, g, o) => p->GetBuffer(0, g, o), ID3D11Texture2D.Guid, static b => b->Release());

        //    // CRT accepts a Tex2D pointer just fine.
        //    ID3D11RenderTargetView* newRenderTargetView = null;
        //    device.Ref.CreateRenderTargetView((ID3D11Resource*)~backBuffer, null, &newRenderTargetView);
        //    this.renderTargetView = newRenderTargetView;
        //    this.SurfacesReady = true;

        //    if (!this.SwapchainReady)
        //    {
        //        this.RefreshSwapchain();
        //    }
        //}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe void InitializeDevices(ComPtr<IDXGISwapChain> swapChain, nint hWnd)
        {
            using var device = swapChain.Cast<ID3D11Device>(static (p, g, o) => p->GetDevice(g, o),
                    ID3D11Device.Guid, static d => d->Release());
            using var deviceContext = device.Cast<ID3D11DeviceContext>(static (p, o) => p->GetImmediateContext(o), static r => r->Release());

            //this.WndProc.InitializeIO(hWnd);
            this.Input = new Win32Backend(hWnd);
            this.Backend.Init((nint)~device, (nint)~deviceContext, false);
            this.DevicesReady = true;
        }

        private void InvalidateDevices()
        {
            //this.WndProc.InvalidateIO();
            this.Input?.Dispose();
            this.Input = null;
            this.Backend.Shutdown();
            this.DevicesReady = false;
        }

        public unsafe bool PrepareForPaint(ComPtr<IDXGISwapChain> swapChain, Vector2 screenDim)
        {
            SwapChainDesc desc = new();
            swapChain.Ref.GetDesc(ref desc);

            if (desc.OutputWindow != this.outputWindowHandle)
            {
                Console.WriteLine("Swapchain outdated and so discarded.");
                //this.InvalidateRenderTarget();
                this.InvalidateDevices();
            }

            if (!this.DevicesReady)
            {
                this.InitializeDevices(swapChain, desc.OutputWindow);
            }

            //if (!this.SurfacesReady)
            //{
            //    this.RefreshTargetView(swapChain);
            //}

            ImGuiIOPtr io = ImGui.GetIO();
            io.DisplaySize = screenDim;

            /* TEMP INPUT */
            this.Input?.NewFrame((int)screenDim.X, (int)screenDim.Y);

            this.outputWindowHandle = desc.OutputWindow;
            return true;
        }

        //public unsafe void SetRenderContext(ComPtr<ID3D11DeviceContext> deviceContext)
        //{
        //    deviceContext.Ref.OMSetRenderTargets(1, ref renderTargetView, null);
        //}

        protected virtual void Dispose(bool disposing)
        {
            //if (!disposedValue)
            //{
            //    if (disposing)
            //    {
            //        unsafe 
            //        {
            //            if (renderTargetView != null)
            //                renderTargetView->Release();
            //        }
            //        //this.Context?.Dispose();
            //    }
            //    disposedValue = true;
            //}
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
