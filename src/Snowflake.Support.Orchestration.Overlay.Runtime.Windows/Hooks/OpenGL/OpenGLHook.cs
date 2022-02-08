using Reloaded.Hooks;
using Reloaded.Hooks.Definitions;
using Silk.NET.Core.Contexts;
using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Vanara.PInvoke;
using X64 = Reloaded.Hooks.Definitions.X64;
using X86 = Reloaded.Hooks.Definitions.X86;
namespace Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks.OpenGL
{
    internal class OpenGLHook
    {
        GL NativeOpenGLApi { get; }
        Kernel32.SafeHINSTANCE OpenGLHandle { get; }
        public OpenGLHook(IngameIpc ingameIpc)
        {
            this.IngameIpc = ingameIpc;
            (this.OpenGLHandle, this.NativeOpenGLApi) = GetOpenGLGlobalInstance();

            this.SwapBuffersHook = ReloadedHooks.Instance.CreateHook<SwapBuffers>
                (this.SwapBuffersImpl, this.NativeOpenGLApi.Context.GetProcAddress("wglSwapBuffers"));
        }

        public void Activate()
        {
            Console.WriteLine("Activated OpenGL Hooks");
            this.SwapBuffersHook.Activate();
        }

        public IngameIpc IngameIpc { get; }

        [X64.Function(X64.CallingConventions.Microsoft)]
        [X86.Function(X86.CallingConventions.Stdcall)]
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public unsafe delegate int SwapBuffers(HDC deviceContext);

        public int SwapBuffersImpl(HDC deviceContext)
        {
            return this.SwapBuffersHook.OriginalFunction(deviceContext);
        }

        public IHook<SwapBuffers> SwapBuffersHook { get; }

        private static (Kernel32.SafeHINSTANCE, GL) GetOpenGLGlobalInstance()
        {
            var handle = Kernel32.GetModuleHandle("opengl32");
            if (handle.IsNull)
            {
                throw new PlatformNotSupportedException("OpenGL not supported.");
            }

            return (handle, GL.GetApi(handle.GetProcAddress));
        }
    }
}
