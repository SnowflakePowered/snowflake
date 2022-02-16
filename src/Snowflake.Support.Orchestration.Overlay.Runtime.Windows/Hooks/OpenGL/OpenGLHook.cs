using ImGuiNET;
using Reloaded.Hooks;
using Reloaded.Hooks.Definitions;
using Silk.NET.Core.Contexts;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Legacy.Extensions.EXT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Vanara.PInvoke;
using static Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks.WndProc.WndProcHook;
using X64 = Reloaded.Hooks.Definitions.X64;
using X86 = Reloaded.Hooks.Definitions.X86;
using Snowflake.Orchestration.Ingame;
using System.Numerics;
using Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Render;

namespace Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks.OpenGL
{
    internal class OpenGLHook
    {
        GL GlContext { get; }
        Kernel32.SafeHINSTANCE OpenGLHandle { get; }
        HWND ActiveHwnd { get; set; }

        OpenGLImGuiInstance ImGuiInst { get; }

        [X64.Function(X64.CallingConventions.Microsoft)]
        [X86.Function(X86.CallingConventions.Stdcall)]
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public delegate nint WndProcCallback(nint hWnd, uint msg, nint wParam, nint lParam);

        private unsafe delegate* unmanaged<nint> wglGetCurrentContext;
        private unsafe delegate* unmanaged<nint, nint, bool> wglMakeCurrent;
        private unsafe delegate* unmanaged<nint, nint> wglCreateContext;

        private nint ImGuiContext = 0;

        public OpenGLHook(IngameIpc ingameIpc)
        {
            this.IngameIpc = ingameIpc;
            (this.OpenGLHandle, this.GlContext) = GetOpenGLGlobalInstance();

            this.SwapBuffersHook = ReloadedHooks.Instance.CreateHook<SwapBuffers>
                (this.SwapBuffersImpl, this.GlContext.Context.GetProcAddress("wglSwapBuffers"));
            unsafe
            {
                this.wglGetCurrentContext = (delegate* unmanaged<nint>)this.GlContext.Context.GetProcAddress("wglGetCurrentContext");
                this.wglMakeCurrent = (delegate* unmanaged<nint, nint, bool>)this.GlContext.Context.GetProcAddress("wglMakeCurrent");
                this.wglCreateContext = (delegate* unmanaged<nint, nint>)this.GlContext.Context.GetProcAddress("wglCreateContext");
            }

            this.Overlay = new OpenGLOverlayTexture();
            this.IngameIpc.CommandReceived += CommandReceivedHandler;
            this.ImGuiInst = new();
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
            Console.WriteLine("Activated OpenGL Hooks");
            this.SwapBuffersHook.Activate();
        }

        public IngameIpc IngameIpc { get; }

        [X64.Function(X64.CallingConventions.Microsoft)]
        [X86.Function(X86.CallingConventions.Stdcall)]
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public unsafe delegate int SwapBuffers(HDC deviceContext);

        public unsafe int SwapBuffersImpl(HDC deviceContext)
        {
            HWND renderingHwnd = User32.WindowFromDC(deviceContext);
            
            User32.GetClientRect(renderingHwnd, out RECT lpRect);
            int width = lpRect.right - lpRect.left;
            int height = lpRect.bottom - lpRect.top;

            nint originalContext = wglGetCurrentContext();

            if (this.ImGuiContext == 0)
            {
                this.ImGuiContext = wglCreateContext(deviceContext.DangerousGetHandle());
            }

            Vector2 screenDim = new(width, height);

            if (!this.Overlay.SizeMatchesViewport((uint)width, (uint)height))
            {
                Console.WriteLine($"Requesting resize to {width} x {height}");
                this.IngameIpc.SendRequest(new()
                {
                    Magic = GameWindowCommand.GameWindowMagic,
                    Type = GameWindowCommandType.WindowResizeEvent,
                    ResizeEvent = new()
                    {
                        Height = (int)(height),
                        Width = (int)(width),
                    }
                });
            }


            if (!this.Overlay.PrepareForPaint(this.GlContext, renderingHwnd.DangerousGetHandle()))
            {
                Console.WriteLine("Failed to open shared texture");
            }

            this.ImGuiInst.PrepareForPaint(this.GlContext, renderingHwnd.DangerousGetHandle(), screenDim);
            //wglMakeCurrent(deviceContext.DangerousGetHandle(), this.ImGuiContext);

            var io = ImGui.GetIO();
            if (this.Overlay.AcquireSync())
            {
                ImGuiInst.NewFrame();
                ImGui.NewFrame();

                ImGui.Begin("Fps");
                ImGui.Text($"{io.Framerate}");
                ImGui.End();

                this.Overlay.Paint(static (tex, w, h) =>
                {
                    ImGuiFullscreenOverlay.Render(tex, w, h);
                });

                ImGui.ShowDemoWindow();
                ImGui.Render();
                ImGuiInst.Render(ImGui.GetDrawData());
                this.Overlay.ReleaseSync();
            } 
            else
            {
                ;
                Console.WriteLine($"Failed to obtain KMT {this.GlContext.GetError()}");
            }

            //wglMakeCurrent(deviceContext.DangerousGetHandle(), originalContext);
            return this.SwapBuffersHook.OriginalFunction(deviceContext);
        }

        public IHook<SwapBuffers> SwapBuffersHook { get; }
        public IHook<WndProcCallback>? WndProcHook { get; set; }
        public OpenGLOverlayTexture Overlay { get; private set; }

        private unsafe static (Kernel32.SafeHINSTANCE, GL) GetOpenGLGlobalInstance()
        {
            var handle = Kernel32.GetModuleHandle("opengl32");
            if (handle.IsNull)
            {
                throw new PlatformNotSupportedException("OpenGL not supported.");
            }

            return (handle, GL.GetApi(new WglNativeContext(handle)));
        }
    }
}
