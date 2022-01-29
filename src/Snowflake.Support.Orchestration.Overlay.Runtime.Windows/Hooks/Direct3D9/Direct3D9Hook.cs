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
using Silk.NET.Direct3D9;
using Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks.Direct3D9;

using X64 = Reloaded.Hooks.Definitions.X64;
using X86 = Reloaded.Hooks.Definitions.X86;

using ImGui = DearImguiSharp.ImGui;
using Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Render;

namespace Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks
{
    internal class Direct3D9Hook
    {
        const int D3D9_DEVICE_METHOD_COUNT = 119;
        const int D3D9Ex_DEVICE_METHOD_COUNT = 15;

        [X64.Function(X64.CallingConventions.Microsoft)]
        [X86.Function(X86.CallingConventions.Stdcall)]
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public unsafe delegate int EndScene(IDirect3DDevice9* device);

        [X64.Function(X64.CallingConventions.Microsoft)]
        [X86.Function(X86.CallingConventions.Stdcall)]
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public unsafe delegate int Reset(IDirect3DDevice9* device, PresentParameters* presentParams);

        [X64.Function(X64.CallingConventions.Microsoft)]
        [X86.Function(X86.CallingConventions.Stdcall)]
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public unsafe delegate int Release(IDirect3DDevice9* device);

        private IHook<EndScene> EndSceneHook { get; }
        private IHook<Reset> ResetHook { get; }
        private bool Initialized { get; set; }
        public IntPtr WindowHandle { get; private set; }

        private ImGuiInstance ImGuiInstance { get; }
        private bool _endSceneLock;
        private bool _resetLock;

        public Direct3D9Hook()
        {
            var vtable = GetDirect3Device9VTable();
            unsafe
            {
                this.EndSceneHook = vtable.CreateFunctionHook<EndScene>((int)Direct3DDevice9Ordinals.EndScene, this.EndSceneImpl);
                this.ResetHook = vtable.CreateFunctionHook<Reset>((int)Direct3DDevice9Ordinals.Reset, this.ResetImpl);
            }
            this.ImGuiInstance = new ImGuiInstance(Open);
        }

        private void Open()
        {
            bool open = true;
            DearImguiSharp.ImGui.ShowDemoWindow(ref open);
        }
        public void Activate()
        {
            Console.WriteLine("Activated D3D9 Hooks");
            this.EndSceneHook.Activate();
            this.ResetHook.Activate();
        }

        private unsafe int EndSceneImpl(IDirect3DDevice9* device)
        {
            if (_endSceneLock)
            {
                return EndSceneHook.OriginalFunction(device);
            }
            _endSceneLock = true;
            IDirect3DSwapChain9* swapChain = null;

            try
            {
                device->GetSwapChain(0, ref swapChain);
                var createParams = new DeviceCreationParameters();
                var presentParams = new PresentParameters();
                device->GetCreationParameters(ref createParams);
                swapChain->GetPresentParameters(ref presentParams);


                nint windowHandle = createParams.HFocusWindow == 0 ? presentParams.HDeviceWindow : createParams.HFocusWindow;

                if (!this.Initialized)
                {
                   // do init code
                   if (windowHandle == 0)
                        return this.EndSceneHook.OriginalFunction(device);
                    Console.WriteLine($"[DX9 EndScene] Init, Window Handle {(long)windowHandle:X}");
                    ImGuiInstance.Activate(windowHandle);
                    ImGui.ImGuiImplDX9Init((void*)device);
                    this.Initialized = true;
                }

                if (!this.ImGuiInstance.CheckWindowHandle(windowHandle))
                {
                    Console.WriteLine($"[DX9 EndScene] Discarding Window Handle {(long)windowHandle:X}");
                    return this.EndSceneHook.OriginalFunction(device);
                }

                ImGui.ImGuiImplDX9NewFrame();
                this.ImGuiInstance.DrawFrame();
                using var drawData = ImGui.GetDrawData();
                ImGui.ImGuiImplDX9RenderDrawData(drawData);
      
                return EndSceneHook.OriginalFunction(device);
            }
            finally
            {
                if (swapChain != null) 
                    swapChain->Release();
                _endSceneLock = false;
            }
        }

        private unsafe int ResetImpl(IDirect3DDevice9* device, PresentParameters* presentParams)
        {
            Console.WriteLine("reset");
            if (_resetLock)
            {
                ResetHook.OriginalFunction.Invoke(device, presentParams);
            }
            _resetLock = true;
            try
            {
                
                if (!this.ImGuiInstance.CheckWindowHandle(presentParams->HDeviceWindow))
                {
                    Console.WriteLine($"[DX9 EndScene] Discarding Window Handle {(long)presentParams->HDeviceWindow:X}");
                    return this.ResetHook.OriginalFunction(device, presentParams);
                }

                Console.WriteLine($"[DX9 Reset] Reset with Handle {(long)presentParams->HDeviceWindow:X}");
                ImGui.ImGuiImplDX9InvalidateDeviceObjects();
                var result = ResetHook.OriginalFunction.Invoke(device, presentParams);
                ImGui.ImGuiImplDX9CreateDeviceObjects();
                return result;
            } 
            finally
            {
                _resetLock = false;
            }
        }

        private unsafe VirtualFunctionTable GetDirect3Device9VTable()
        {
            using var window = new RenderWindow();
            var d3d9Api = D3D9.GetApi();
            IDirect3D9* d3d9 = d3d9Api.Direct3DCreate9(D3D9.SdkVersion);

            var d3d9Params = new PresentParameters()
            {
                BackBufferWidth = 0,
                BackBufferHeight = 0,
                BackBufferFormat = Format.FmtUnknown,
                BackBufferCount = 0,
                MultiSampleType = MultisampleType.MultisampleNone,
                MultiSampleQuality = 0,
                SwapEffect = Swapeffect.SwapeffectDiscard,
                Windowed = 1,
                EnableAutoDepthStencil = 0,
                AutoDepthStencilFormat = Format.FmtUnknown,
                Flags = 0,
                FullScreenRefreshRateInHz = 0,
                PresentationInterval = 0,
                HDeviceWindow = window.WindowHandle.DangerousGetHandle()
            };

            IDirect3DDevice9* device = null;

            var result = d3d9->CreateDevice(D3D9.AdapterDefault, Devtype.DevtypeNullref, window.WindowHandle.DangerousGetHandle(),
                D3D9.CreateSoftwareVertexprocessing | D3D9.CreateDisableDriverManagement, ref d3d9Params, ref device);

            if (result < 0)
            {
                Console.WriteLine("Failed to create D3D9 Device");
                d3d9->Release();
                throw new PlatformNotSupportedException("Direct3D 9 not supported.");
            }
            Console.WriteLine("Succeeded D3D9 Init");

            VirtualFunctionTable vtable = VirtualFunctionTable.FromObject((nint)device, D3D9_DEVICE_METHOD_COUNT);
            device->Release();
            d3d9->Release();
            return vtable;
        }
    }
}
