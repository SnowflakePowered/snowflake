using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Legacy.Extensions.EXT;
using Snowflake.Orchestration.Ingame;
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
    internal class OpenGLOverlayTexture : IOverlayTexture
    {
        private IntPtr overlayTextureHandle;
        private (uint glTex, uint glMemHandle) overlayTexture;
        private bool ReadyToPaint { get; set; }
        private (uint width, uint height) OverlayTexDim { get; set; }
        public readonly object TextureMutex = new();

        GL? currentContext;
        ExtWin32KeyedMutex kmtExt;
        ExtMemoryObject memObjExt;
        private nint outputWindowHandle;
        private ulong Sz;
        public bool Refresh(OverlayTextureEventParams texParams)
        {
            int owningPid = texParams.SourceProcessId;
            nint textureHandle = texParams.TextureHandle;
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

            Console.WriteLine($"Got owned handle {dupedHandle.ToString("x")} {texParams.Width}x{texParams.Height}");

            // Release old texture
            unsafe
            {
                if (this.overlayTexture.glMemHandle != 0)
                {
                    this.Invalidate();
                }
            }

            // close old the handle
            Kernel32.CloseHandle(this.overlayTextureHandle);

            // new texture will be fetched on next paint.
            this.overlayTextureHandle = dupedHandle;
            this.OverlayTexDim = (texParams.Width, texParams.Height);
            this.Sz = texParams.Size;
            return true;
        }

        private void Invalidate()
        {
            lock (this.TextureMutex)
            {
                if (this.currentContext == null)
                    return;
                this.currentContext.DeleteTexture(this.overlayTexture.glTex);
                Legacy.GL legacyContext = Legacy.GL.GetApi(this.currentContext.Context);
                if (legacyContext.TryGetExtension(out ExtMemoryObject memObjExt))
                {
                    memObjExt.DeleteMemoryObject(this.overlayTexture.glMemHandle);
                }

                this.overlayTexture = (0, 0);
                this.currentContext = null;
                this.ReadyToPaint = false;
            }
        }

        public unsafe bool PrepareForPaint(GL glContext, nint outputWindowHandle)
        {
            if (this.ReadyToPaint && this.outputWindowHandle == outputWindowHandle)
            {
                return true;
            }

            this.Invalidate();

            Legacy.GL legacyContext = Legacy.GL.GetApi(glContext.Context);

            if (!legacyContext.TryGetExtension(out ExtMemoryObjectWin32 memObjWin32Ext))
            {
                return false;
            }

            if (!legacyContext.TryGetExtension(out ExtMemoryObject memObjExt))
            {
                return false;
            }

            if (!legacyContext.TryGetExtension(out ExtWin32KeyedMutex kmtExt))
            {
                return false;
            }

            this.currentContext = glContext;
            glContext.CreateTextures(GLEnum.Texture2D, 1, out uint tex);
            glContext.TextureParameter(tex, TextureParameterName.TextureTilingExt, (uint)EXT.OptimalTilingExt);
            glContext.TextureParameter(tex, TextureParameterName.TextureMinFilter, (uint)GLEnum.Linear);
            glContext.TextureParameter(tex, TextureParameterName.TextureMagFilter, (uint)GLEnum.Linear);
            glContext.TextureParameter(tex, TextureParameterName.TextureWrapS, (uint)GLEnum.ClampToEdge);
            glContext.TextureParameter(tex, TextureParameterName.TextureWrapT, (uint)GLEnum.ClampToEdge);

            Span<int> swizzleMask = stackalloc[] { (int)GLEnum.Blue, (int)GLEnum.Green, (int)GLEnum.Red, (int)GLEnum.Alpha };
            glContext.TextureParameter(tex, TextureParameterName.TextureSwizzleRgba, swizzleMask);

            uint memHandle = memObjExt.CreateMemoryObject();
            // CEF texture is always 4BPP
            memObjWin32Ext.ImportMemoryWin32Handle(memHandle, this.Sz * 2, EXT.HandleTypeD3D11ImageExt, (void*)this.overlayTextureHandle);

            CheckGlError(glContext, "importwin32");
            if (kmtExt.AcquireKeyedMutexWin32(memHandle, 0, unchecked((uint)-1)))
            {
                CheckGlError(glContext, "akmtw32");
                memObjExt.TextureStorageMem2D(tex, 1, EXT.Rgba8Ext, this.OverlayTexDim.width, this.OverlayTexDim.height, memHandle, 0);
                CheckGlError(glContext, "texmem");
                kmtExt.ReleaseKeyedMutexWin32(memHandle, 0);
            } else
            {
                Console.WriteLine("Failed to grab mutex");
            }
            CheckGlError(glContext, "rkmt32");

            this.overlayTexture = (tex, memHandle);
            this.kmtExt = kmtExt;
            this.memObjExt = memObjExt;
            this.outputWindowHandle = outputWindowHandle;
            this.ReadyToPaint = true;
            return true;
        }

        public bool SizeMatchesViewport(uint width, uint height)
        {
            return (width == OverlayTexDim.width) && (height == OverlayTexDim.height);
        }

        public bool AcquireSync()
        {
            return this.kmtExt.AcquireKeyedMutexWin32(this.overlayTexture.glMemHandle, 0, unchecked((uint)-1));
        }

        public void ReleaseSync()
        {
            this.kmtExt.ReleaseKeyedMutexWin32(this.overlayTexture.glMemHandle, 0);
        }

        public void Paint(Action<nint, uint, uint> renderFn)
        {
            lock (this.TextureMutex)
            {
                unsafe
                {
                    renderFn((nint)this.overlayTexture.glTex, OverlayTexDim.width, OverlayTexDim.height);
                }
            }
        }

        public static void CheckGlError(GL gl, string title)
        {
            var error = gl.GetError();
            if (error != GLEnum.NoError)
            {
                Console.WriteLine($"{title}: {error}");
            }
        }
    }
}
