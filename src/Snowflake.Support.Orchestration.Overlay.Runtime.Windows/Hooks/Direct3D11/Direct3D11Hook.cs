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

using Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks.Direct3D9;

using X64 = Reloaded.Hooks.Definitions.X64;
using X86 = Reloaded.Hooks.Definitions.X86;

using ImGui = DearImguiSharp.ImGui;
using Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Render;
using Silk.NET.DXGI;
using Snowflake.Orchestration.Ingame;
using Vanara.PInvoke;

using static Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks.GuidHelpers;
using Evergine.Bindings.RenderDoc;

namespace Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks.Direct3D11
{
    internal class Direct3D11Hook
    {
        const int D3D11_DEVICE_METHOD_COUNT = 43;
        const int D3D11_SWAPCHAIN_METHOD_COUNT = 18;

        private bool resizeBuffersLock = false;
        private bool presentLock = false;

        private unsafe IDXGIKeyedMutex* overlayTextureMutex = null;
        private unsafe ID3D11ShaderResourceView* overlayShaderResourceView = null;
        private unsafe ID3D11Texture2D* overlayTexture = null;
        private Texture2DDesc overlayTextureDesc = new();
        private IntPtr overlayTextureHandle = IntPtr.Zero;

        private unsafe ID3D11RenderTargetView* renderTargetView = null;
        private bool imguiInitialized = false;
        protected RenderDoc renderDoc;

        D3D11 API { get; }
        public Direct3D11Hook(IngameIpc ipc)
        {
            this.IngameIpc = ipc;
            this.API = D3D11.GetApi();
            (VirtualFunctionTable deviceVtable, VirtualFunctionTable swapChainVtable) = GetDirect3D11VTable();
            unsafe
            {
                this.PresentHook = swapChainVtable.CreateFunctionHook<Present>((int)DXGISwapChainOrdinals.Present, this.PresentImpl);
                this.ResizeBuffersHook = swapChainVtable.CreateFunctionHook<ResizeBuffers>((int)DXGISwapChainOrdinals.ResizeBuffers, this.ResizeBuffersImpl);
            }
            //this.ImGuiInstance = new ImGuiInstance(Open);
            this.Context = ImGui.CreateContext(null);
            ImGui.StyleColorsDark(null);
            this.IngameIpc.CommandReceived += CommandReceivedHandler;
            RenderDoc.Load(out this.renderDoc);
            if (this.renderDoc == null)
            {
                Console.WriteLine("NODOC");
            }
        }

        private void CommandReceivedHandler(GameWindowCommand command)
        {
            if (command.Type == GameWindowCommandType.OverlayTextureEvent)
            {
                Console.WriteLine($"Got texhandle {command.TextureEvent.TextureHandle.ToString("x")} from PID {command.TextureEvent.SourceProcessId}");
                var process = Kernel32.OpenProcess(new(Kernel32.ProcessAccess.PROCESS_DUP_HANDLE), false, (uint)command.TextureEvent.SourceProcessId);
                if (process.IsNull)
                {
                    Console.WriteLine("unable to open source process...");
                    return;
                }

                if (!Kernel32.DuplicateHandle(process, command.TextureEvent.TextureHandle, Kernel32.GetCurrentProcess(), out IntPtr dupedHandle,
                    0, false, Kernel32.DUPLICATE_HANDLE_OPTIONS.DUPLICATE_SAME_ACCESS))
                {
                    Console.WriteLine("unable to dupe handle");
                    return;
                };

                Console.WriteLine($"Got owned handle {dupedHandle.ToString("x")}");

                // Release old texture
                unsafe
                {
                    if (this.overlayTexture != null)
                    {
                        this.overlayShaderResourceView->Release();
                        this.overlayTextureMutex->Release();
                        this.overlayTexture->Release();

                        this.overlayTexture = null;
                        this.overlayTextureMutex = null;
                        this.overlayShaderResourceView = null;
                    }
                }
                // close the handle
                Kernel32.CloseHandle(this.overlayTextureHandle);

                // new texture will be fetched on next paint.
                this.overlayTextureHandle = dupedHandle;
            }
        }

        private void Open()
        {
            bool open = true;
            DearImguiSharp.ImGui.ShowDemoWindow(ref open);
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
                if (this.imguiInitialized)
                {
                    // Destroy existing ImGui 
                    if (renderTargetView != null)
                        renderTargetView->Release();
                    renderTargetView = null;

                    ImGui.ImGuiImplDX11InvalidateDeviceObjects();
                }

                var bufferResult =  this.ResizeBuffersHook.OriginalFunction(swapChain, bufferCount, width, height, newFormat, swapChainFlags);

                // Request new overlay texture
                ID3D11Device* device = null;
                ID3D11Texture2D* backBuffer = null;
                Texture2DDesc desc = new();
                
                int res = swapChain->GetBuffer(0, RiidOf(ID3D11Texture2D.Guid), (void**)&backBuffer);
                swapChain->GetDevice(RiidOf(ID3D11Device.Guid), (void**)&device);
                backBuffer->GetDesc(ref desc);

                Console.WriteLine($"Hook Resize ({desc.Width}, {desc.Height})");

                this.IngameIpc.SendRequest(new()
                {
                    Magic = GameWindowCommand.GameWindowMagic,
                    Type = GameWindowCommandType.WindowResizeEvent,
                    ResizeEvent = new()
                    {
                        Height = (int)(desc.Height),
                        Width = (int)(desc.Width),
                    }
                });

                // Recreate ImGui objects
                if (this.imguiInitialized)
                {
                    ID3D11Resource* backBufferResource = null;
                    ID3D11RenderTargetView* newRenderTargetView = null;

                    backBuffer->QueryInterface(RiidOf(ID3D11Resource.Guid), (void**)&backBufferResource);
                    device->CreateRenderTargetView(backBufferResource, null, &newRenderTargetView);

                    renderTargetView = newRenderTargetView;
                    backBufferResource->Release();
                    ImGui.ImGuiImplDX11CreateDeviceObjects();
                }

                // release backbuffer
                backBuffer->Release();

                return bufferResult;
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
                ID3D11Device* device = null;
                ID3D11Device1* device1 = null;
                ID3D11DeviceContext* deviceContext = null;
                SwapChainDesc swapChainDesc = new();

                swapChain->GetDevice(RiidOf(ID3D11Device.Guid), (void**)&device);
                swapChain->GetDevice(RiidOf(ID3D11Device1.Guid), (void**)&device1);
                swapChain->GetDesc(ref swapChainDesc);

                device->GetImmediateContext(ref deviceContext);

                // Haven't received texture handle yet
                if (this.overlayTextureHandle == IntPtr.Zero)
                {
                    ID3D11Texture2D* backBuffer = null;
                    Texture2DDesc desc = new();
                    int res = swapChain->GetBuffer(0, RiidOf(ID3D11Texture2D.Guid), (void**)&backBuffer);

                    if (desc.Height != 0 && desc.Width != 0)
                    {
                        Console.WriteLine("Requesting resize");
                        this.IngameIpc.SendRequest(new()
                        {
                            Magic = GameWindowCommand.GameWindowMagic,
                            Type = GameWindowCommandType.WindowResizeEvent,
                            ResizeEvent = new()
                            {
                                Height = (int)(desc.Height),
                                Width = (int)(desc.Width),
                            }
                        });
                    }

                    backBuffer->Release();
                    deviceContext->Release();
                    device1->Release();
                    device->Release();
                    return this.PresentHook.OriginalFunction(swapChain, syncInterval, flags);
                }

                // need to refresh texture
                if (this.overlayTextureHandle != IntPtr.Zero && this.overlayTexture == null)
                {
                    ID3D11Texture2D* tex2D = null; // moved to thhis.overlayTexture
                    Texture2DDesc tex2dDesc = new(); // dropped 
                    ID3D11ShaderResourceView* texSRV = null; // moved to this.overlayShaderResourceView
                    IDXGIKeyedMutex* texMtx = null; // moved to this.overlayTextureMutex
                    ID3D11Resource* texRsrc = null; // dropped
                    // todo: error check
                    int res;
                    if ((res = device1->OpenSharedResource1(this.overlayTextureHandle.ToPointer(), RiidOf(ID3D11Texture2D.Guid), (void**)&tex2D)) != 0)
                    {
                        Console.WriteLine($"Unable to open shared texture handle {this.overlayTextureHandle}: {res.ToString("x")}.");
                        device1->Release();
                        return this.PresentHook.OriginalFunction(swapChain, syncInterval, flags);
                    }

                    Console.WriteLine("Opened shared texture.");
                    tex2D->QueryInterface(RiidOf(IDXGIKeyedMutex.Guid), (void**)&texMtx);
                    tex2D->QueryInterface(RiidOf(ID3D11Resource.Guid), (void**)&texRsrc);

                    tex2D->GetDesc(ref tex2dDesc);
                    this.overlayTextureMutex = texMtx;
                    this.overlayTexture = tex2D;
                    this.overlayTextureDesc = tex2dDesc;

                    // Acquire lock on mutex for SRV creation, not sure if this is needed.
                    this.overlayTextureMutex->AcquireSync(0, unchecked((uint)-1));
                    ShaderResourceViewDesc srvDesc = new()
                    {
                        Format = tex2dDesc.Format,
                        ViewDimension = D3DSrvDimension.D3D101SrvDimensionTexture2D,
                        Anonymous = new(texture2D: new()
                        {
                            MipLevels = tex2dDesc.MipLevels,
                            MostDetailedMip = 0,
                        })
                    };
                    device->CreateShaderResourceView(texRsrc, ref srvDesc, ref texSRV);
                    this.overlayTextureMutex->ReleaseSync(0);

                    // Get text src
                    this.overlayShaderResourceView = texSRV;
                    texRsrc->Release();

                }

                if (!this.imguiInitialized)
                {
                    ID3D11Texture2D* backBuffer = null;
                    ID3D11Resource* backBufferResource = null;

                    ImGui.ImGuiImplWin32Init(swapChainDesc.OutputWindow);

                    // todo: init wndproc

                    ImGui.ImGuiImplDX11Init(device, deviceContext);
                    swapChain->GetBuffer(0, RiidOf(ID3D11Texture2D.Guid), (void**)&backBuffer);

                    ID3D11RenderTargetView* newRenderTargetView = null;

                    backBuffer->QueryInterface(RiidOf(ID3D11Resource.Guid), (void**)&backBufferResource);
                    device->CreateRenderTargetView(backBufferResource, null, &newRenderTargetView);

                    if (this.renderTargetView != null)
                        this.renderTargetView->Release();

                    this.renderTargetView = newRenderTargetView;
                    backBufferResource->Release();
                    backBuffer->Release();

                    this.imguiInitialized = true;
                }

                this.overlayTextureMutex->AcquireSync(0, unchecked((uint)-1));
                ImGui.ImGuiImplDX11NewFrame();
                ImGui.ImGuiImplWin32NewFrame();
                ImGui.NewFrame();
                // -- render function
                bool x = true;
                //ImGui.ShowDemoWindow(ref x);
                // bruh i need to make a shaderesourceview???

                var vec = new DearImguiSharp.ImVec2.__Internal() { x = this.overlayTextureDesc.Width, y = this.overlayTextureDesc.Height };
                var uv_min = new DearImguiSharp.ImVec2.__Internal() { x = 0f, y = 0f };         // Top-left
                var uv_max = new DearImguiSharp.ImVec2.__Internal() { x = 1f, y = 1f };
                var tint_col = new DearImguiSharp.ImVec4.__Internal() { w = 1.0f, x = 1.0f, y = 1.0f, z = 1.0f, };
                var border_col = new DearImguiSharp.ImVec4.__Internal() { x = 0.0f, y = 0.0f, w = 0.0f, z = 0.0f };
                var wndowpad = new DearImguiSharp.ImVec2.__Internal() { x = 0.0f, y = 0.0f };

                var viewPort = ImGui.GetMainViewport();
                ImGui.SetNextWindowPos(viewPort.Pos, 1, new()
                {
                    X = 0f,
                    Y = 0f,
                });
                ImGui.SetNextWindowSize(viewPort.Size, 1);
                ImGui.SetNextWindowFocus();
                //ImGui.PushStyleColorU32((int)DearImguiSharp.ImGuiCol.ImGuiColWindowBg, unchecked((uint)-1));
                ImGui.__Internal.PushStyleVarVec2((int)DearImguiSharp.ImGuiStyleVar.ImGuiStyleVarWindowPadding, wndowpad);
                ImGui.__Internal.PushStyleVarVec2((int)DearImguiSharp.ImGuiStyleVar.ImGuiStyleVarWindowBorderSize, wndowpad);

                ImGui.Begin("DirectX11 Texture Test", ref x, 
                    (int)(0
                    | DearImguiSharp.ImGuiWindowFlags.ImGuiWindowFlagsNoDecoration
                    | DearImguiSharp.ImGuiWindowFlags.ImGuiWindowFlagsNoMove
                    | DearImguiSharp.ImGuiWindowFlags.ImGuiWindowFlagsNoResize
                    | DearImguiSharp.ImGuiWindowFlags.ImGuiWindowFlagsNoBackground
                    //| DearImguiSharp.ImGuiWindowFlags.ImGuiWindowFlagsNoSavedSettings
                    ));

                ImGui.__Internal.Image((nint)this.overlayShaderResourceView, vec, uv_min, uv_max, tint_col, border_col);

                ImGui.PopStyleVar(1);
                ImGui.End();

                // -- render function
                ImGui.EndFrame();
                ImGui.Render();

                ImGui.UpdatePlatformWindows();
                ImGui.RenderPlatformWindowsDefault(IntPtr.Zero, IntPtr.Zero);

                // set rendertargetview
                deviceContext->OMSetRenderTargets(1, ref renderTargetView, null);

                using var drawData = ImGui.GetDrawData();
                ImGui.ImGuiImplDX11RenderDrawData(drawData);
                this.overlayTextureMutex->ReleaseSync(0);

                deviceContext->Release();
                device1->Release();
                device->Release();
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
        public ImGuiInstance ImGuiInstance { get; }
        public IngameIpc IngameIpc { get; }
        public DearImguiSharp.ImGuiContext Context { get; }

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
