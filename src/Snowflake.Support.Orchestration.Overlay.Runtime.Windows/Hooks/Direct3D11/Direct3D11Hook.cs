using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Reloaded.Hooks;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Tools;
using Silk.NET.Direct3D11;
using Silk.NET.Core.Native;

using X64 = Reloaded.Hooks.Definitions.X64;
using X86 = Reloaded.Hooks.Definitions.X86;

using Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Render;
using Silk.NET.DXGI;
using Snowflake.Orchestration.Ingame;
using ImGuiNET;
using System.Numerics;

namespace Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks.Direct3D11
{
    internal class Direct3D11Hook
    {
        const int D3D11_DEVICE_METHOD_COUNT = 43;
        const int D3D11_SWAPCHAIN_METHOD_COUNT = 18;

        private bool resizeBuffersLock = false;
        private bool presentLock = false;

        public Direct3D11OverlayTexture Overlay { get; }
        public Direct3D11ImGuiInstance ImGuiInst { get; }

        D3D11 API { get; }

        public Direct3D11Hook(IngameIpc ipc)
        {
            this.IngameIpc = ipc;
            this.API = D3D11.GetApi();
            this.Overlay = new();

            (VirtualFunctionTable deviceVtable, VirtualFunctionTable swapChainVtable) = GetDirect3D11VTable();
            unsafe
            {
                this.PresentHook = swapChainVtable.CreateFunctionHook<Present>((int)DXGISwapChainOrdinals.Present, this.PresentImpl);
                this.ResizeBuffersHook = swapChainVtable.CreateFunctionHook<ResizeBuffers>((int)DXGISwapChainOrdinals.ResizeBuffers, this.ResizeBuffersImpl);
            }
     
            this.IngameIpc.CommandReceived += CommandReceivedHandler;
            this.ImGuiInst = new Direct3D11ImGuiInstance();
        }

        private void CommandReceivedHandler(GameWindowCommand command)
        {
            if (command.Type == GameWindowCommandType.OverlayTextureEvent)
            {
                Console.WriteLine($"Got texhandle {command.TextureEvent.TextureHandle.ToString("x")} from PID {command.TextureEvent.SourceProcessId}");
                this.Overlay.Refresh(command.TextureEvent);
            }
        }

        public void Activate()
        {
            Console.WriteLine("Activated D3D11 Hooks");
            this.PresentHook.Activate();
            this.ResizeBuffersHook.Activate();
        }

        private unsafe int ResizeBuffersImpl(IDXGISwapChain* swapChain, uint bufferCount, uint width, uint height, Format newFormat, uint swapChainFlags)
        {
            if (resizeBuffersLock)
            {
                Console.WriteLine("[DX11 ResizeBuffers] Entered under lock");
                return this.ResizeBuffersHook.OriginalFunction(swapChain, bufferCount, width, height, newFormat, swapChainFlags);
            }
            
            resizeBuffersLock = true;
            try
            {
                Console.WriteLine($"rz {width}x{height} a");
                //this.ImGuiInst.InvalidateRenderTarget();
                return this.ResizeBuffersHook.OriginalFunction(swapChain, bufferCount, width, height, newFormat, swapChainFlags);
            }
            finally
            {
                this.resizeBuffersLock = false;
            }
        }

        private unsafe int PresentImpl(IDXGISwapChain* swapChain, uint syncInterval, uint flags)
        {
            if (presentLock)
            {
                Console.WriteLine("[DX11 Present] Entered under lock");
                return this.PresentHook.OriginalFunction(swapChain, syncInterval, flags);
            }

            presentLock = true;
            try
            {
                ComPtr<IDXGISwapChain> swapChainWrap = new(swapChain);
                SwapChainDesc swapChainDesc = new();

                using var device = swapChainWrap.Cast<ID3D11Device>(static (p, g, o) => p->GetDevice(g, o), ID3D11Device.Guid, static d => d->Release());
                using var device1 = swapChainWrap.Cast<ID3D11Device1>(static (p, g, o) => p->GetDevice(g, o), ID3D11Device1.Guid, static d => d->Release());
                using var deviceContext = device.Cast<ID3D11DeviceContext>(static (p, o) => p->GetImmediateContext(o), static r => r->Release());

                swapChain->GetDesc(ref swapChainDesc);

                Texture2DDesc backBufferDesc = new();
                using var backBuffer =
                    swapChainWrap.Cast<ID3D11Texture2D>(static (p, g, o) => p->GetBuffer(0, g, o), ID3D11Texture2D.Guid, b => b->Release());
                backBuffer.Ref.GetDesc(ref backBufferDesc);

                if (!this.Overlay.SizeMatchesViewport(backBufferDesc.Width, backBufferDesc.Height))
                {
                    Console.WriteLine($"Requesting resize to {backBufferDesc.Width} x {backBufferDesc.Height}");
                    this.IngameIpc.SendRequest(new()
                    {
                        Magic = GameWindowCommand.GameWindowMagic,
                        Type = GameWindowCommandType.WindowResizeEvent,
                        ResizeEvent = new()
                        {
                            Height = (int)(backBufferDesc.Height),
                            Width = (int)(backBufferDesc.Width),
                        }
                    });
                }

                // Haven't received texture handle yet
                if (!this.Overlay.ReadyToInitialize)
                {
                    Console.WriteLine("Texture handle not ready.");
                    return this.PresentHook.OriginalFunction(swapChain, syncInterval, flags);
                }

                // need to refresh texture
                if (!this.Overlay.PrepareForPaint(device1, swapChainDesc.OutputWindow))
                {
                    Console.WriteLine("Failed to refresh texture for output window");
                    return this.PresentHook.OriginalFunction(swapChain, syncInterval, flags);
                }

                this.ImGuiInst.PrepareForPaint(swapChainWrap, new Vector2(backBufferDesc.Width, backBufferDesc.Height));
                ImGuiIOPtr io = ImGui.GetIO();
            
                //Console.WriteLine($"drawdata {drawData.DisplaySize}");
                lock (this.Overlay.TextureMutex)
                {
                    if (this.Overlay.AcquireSync())
                    {
                        this.ImGuiInst.NewFrame();
                        //ImGui.ImGuiImplWin32NewFrame();
                        ImGui.NewFrame();

                        this.Overlay.Paint(static (srv, w, h) =>
                        {
                            ImGuiFullscreenOverlay.Render(srv, w, h);
                        });

                        ImGui.Begin("Fps");
                        ImGui.Text($"{io.Framerate}");
                        ImGui.End();

                        ImGui.ShowDemoWindow();
                        ImGui.Render();

                        ImGui.UpdatePlatformWindows();
                        ImGui.RenderPlatformWindowsDefault(IntPtr.Zero, IntPtr.Zero);
                        var drawData = ImGui.GetDrawData();
                        //ImGuiInst.SetRenderContext(deviceContext);
                        this.ImGuiInst.Render(drawData);
                        this.Overlay.ReleaseSync();
                    }
                }

                return this.PresentHook.OriginalFunction(swapChain, syncInterval, flags);
            }
            finally
            {
                presentLock = false;
            }
        }

        [X64.Function(X64.CallingConventions.Microsoft)]
        [X86.Function(X86.CallingConventions.Stdcall)]
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public unsafe delegate int Present(IDXGISwapChain* swapChain, uint syncInterval, uint flags);

        [X64.Function(X64.CallingConventions.Microsoft)]
        [X86.Function(X86.CallingConventions.Stdcall)]
        public unsafe delegate int ResizeBuffers(IDXGISwapChain* swapChain, uint bufferCount, uint width, uint height, Format newFormat, uint swapChainFlags);

        private static readonly D3DFeatureLevel[] FEATURE_LEVELS = new[] { D3DFeatureLevel.D3DFeatureLevel101, 
            D3DFeatureLevel.D3DFeatureLevel110 };

        public IHook<Present> PresentHook { get; }
        public IHook<ResizeBuffers> ResizeBuffersHook { get; }
        public IngameIpc IngameIpc { get; }

        private unsafe (VirtualFunctionTable deviceVtable, VirtualFunctionTable swapChainVtable) GetDirect3D11VTable()
        {
            using var window = new RenderWindow();

            D3DFeatureLevel outFeatureLevel = 0;
            Span<D3DFeatureLevel> requestFeatureLevels = FEATURE_LEVELS.AsSpan();

            SwapChainDesc swapChainDesc = new()
            {
                BufferDesc = new()
                {
                    Width = 100,
                    Height = 100,
                    RefreshRate = new()
                    {
                        Numerator = 60,
                        Denominator = 1
                    },
                    Format = Format.FormatR8G8B8A8Unorm,    
                    ScanlineOrdering = ModeScanlineOrder.ModeScanlineOrderUnspecified,
                    Scaling = ModeScaling.ModeScalingUnspecified,
                },
                SampleDesc = new()
                {
                    Count = 1,
                    Quality = 0
                },
                Windowed = 1, 
                BufferUsage = DXGI.UsageRenderTargetOutput,
                BufferCount = 1,
                OutputWindow = window.WindowHandle.DangerousGetHandle(),
                SwapEffect = SwapEffect.SwapEffectDiscard,
                Flags = ((uint)SwapChainFlag.SwapChainFlagAllowModeSwitch)
            };

            IDXGISwapChain* swapChain = null;
            ID3D11Device* device = null;
            ID3D11DeviceContext* context = null;
            int? result = 0;
            fixed (D3DFeatureLevel* featureLevels = requestFeatureLevels) {
                result = API.CreateDeviceAndSwapChain(
                null,
                D3DDriverType.D3DDriverTypeHardware,
                0,
                0,
                featureLevels,
                (uint)requestFeatureLevels.Length,
                D3D11.SdkVersion,
                ref swapChainDesc,
                ref swapChain,
                ref device,
                ref outFeatureLevel,
                ref context
                );
            }
            
            if (result == null || result.Value < 0)
            {
                Console.WriteLine("Failed to create D3D11 Device");
                throw new PlatformNotSupportedException("Direct3D11 not supported.");
            }
       
            Console.WriteLine("Succeeded D3D11 Init");

            VirtualFunctionTable deviceVtable = VirtualFunctionTable.FromObject((nint)device, D3D11_DEVICE_METHOD_COUNT);
            VirtualFunctionTable swapChainVtable = VirtualFunctionTable.FromObject((nint)swapChain, D3D11_SWAPCHAIN_METHOD_COUNT);
            
             //order is important!
            swapChain->Release();
            device->Release();
            context->Release();
            return (deviceVtable, swapChainVtable);
        }
    }
}
