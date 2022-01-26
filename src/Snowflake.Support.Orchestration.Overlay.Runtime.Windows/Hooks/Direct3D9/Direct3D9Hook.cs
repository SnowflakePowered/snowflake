using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Reloaded.Hooks;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Tools;
using Silk.NET.Direct3D9;
using Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks.Direct3D9;
using static Vanara.PInvoke.Gdi32;

namespace Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks
{
    internal class Direct3D9Hook
    {
        const int D3D9_DEVICE_METHOD_COUNT = 119;
        const int D3D9Ex_DEVICE_METHOD_COUNT = 15;

        [Reloaded.Hooks.Definitions.X64.Function(Reloaded.Hooks.Definitions.X64.CallingConventions.Microsoft)]
        [Reloaded.Hooks.Definitions.X86.Function(Reloaded.Hooks.Definitions.X86.CallingConventions.Stdcall)]
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public unsafe delegate int EndScene(IDirect3DDevice9* device);

        private IHook<EndScene>? EndSceneHook = null;

        private unsafe int EndSceneImpl(IDirect3DDevice9* device)
        {
            Console.WriteLine("Hooked Endscene");
            return EndSceneHook.OriginalFunction(device);
        }

        public unsafe void Init()
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
                // todo: throw exception
                Console.WriteLine("Failed to create D3D9 Device");
                d3d9->Release();
                return;
            }
            Console.WriteLine("Succeeded D3D9 Init");
            
            var vtable = VirtualFunctionTable.FromObject((nint)device, D3D9_DEVICE_METHOD_COUNT);

            Console.WriteLine("Found EndScene at " + vtable.TableEntries[(int)Direct3DDevice9Ordinals.EndScene].FunctionPointer);

            this.EndSceneHook = vtable.CreateFunctionHook<EndScene>((int)Direct3DDevice9Ordinals.EndScene, this.EndSceneImpl).Activate();

            device->Release();
            d3d9->Release();

        }
    }
}
