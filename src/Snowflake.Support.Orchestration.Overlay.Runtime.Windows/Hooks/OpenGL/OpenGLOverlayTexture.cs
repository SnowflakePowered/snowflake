using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Legacy.Extensions.EXT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Vanara.PInvoke;
using Legacy = Silk.NET.OpenGL.Legacy;

namespace Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks.OpenGL
{
    internal class OpenGLOverlayTexture
    {
        private IntPtr overlayTextureHandle;

        public bool Refresh(int owningPid, nint textureHandle)
        {
            // todo: unify this with d3d11
            var process = Kernel32.OpenProcess(new(Kernel32.ProcessAccess.PROCESS_DUP_HANDLE), false, (uint)owningPid);
            if (process.IsNull)
            {
                Console.WriteLine("unable to open source process...");
                return false;
            }

            if (!Kernel32.DuplicateHandle(process, textureHandle, Kernel32.GetCurrentProcess(), out IntPtr dupedHandle,
                0, false, Kernel32.DUPLICATE_HANDLE_OPTIONS.DUPLICATE_SAME_ACCESS))
            {
                Console.WriteLine("unable to dupe handle");
                return false;
            };

            Console.WriteLine($"Got owned handle {dupedHandle.ToString("x")}");

            // Release old texture
            unsafe
            {
                //if (this.overlayTexture != null)
                //{
                //    this.Invalidate();
                //}
            }

            // close old the handle
            Kernel32.CloseHandle(this.overlayTextureHandle);

            // new texture will be fetched on next paint.
            this.overlayTextureHandle = dupedHandle;
            return true;
        }

        public unsafe bool PrepareForPaint(GL glContext, Vector2 screenDim)
        {
            Legacy.GL legacyContext = Legacy.GL.GetApi(glContext.Context);

            if (!legacyContext.TryGetExtension<ExtMemoryObjectWin32>(out ExtMemoryObjectWin32 memObjExt))
            {
                return false;
            }

            memObjExt.
            return true;
        }
    }
}
