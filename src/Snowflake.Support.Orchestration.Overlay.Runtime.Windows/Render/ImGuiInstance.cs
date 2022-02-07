using DearImguiSharp;
using Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks.WndProc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Vanara.PInvoke.User32;

namespace Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Render
{
    internal class ImGuiInstance
    {
        public bool Initialized { get; private set; }
        public nint WindowHandle { get; private set; }
        public ImGuiContext Context { get; private set; }
        private Action DoRender { get; set; }
        private WndProcHook WndProcHook { get; set; }

        public ImGuiInstance(Action render)
        {
            this.DoRender = render;
            this.Context = ImGui.CreateContext(null);
            ImGui.StyleColorsDark(null);
        }

        public void Activate(nint hWnd)
        {
            if (this.Initialized)
                return;
            if (hWnd == 0)
                throw new ArgumentException("hWnd can not be 0");
            this.WindowHandle = hWnd;
            Console.WriteLine($"[ImguiHook] Init with Window Handle {(long)WindowHandle:X}");
            ImGui.ImGuiImplWin32Init(this.WindowHandle);
            this.WndProcHook = new WndProcHook(hWnd, WndProcHandler);
            this.WndProcHook.Hook.Activate();
            this.Initialized = true;
        }

        /// <summary>
        /// [Internal] Called from renderer implementation, renders a new frame.
        /// </summary>
        public unsafe void DrawFrame()
        {
            ImGui.ImGuiImplWin32NewFrame();
            ImGui.NewFrame();
            this.DoRender();
            ImGui.EndFrame();
            ImGui.Render();

            // dont do this.
            if ((ImGui.GetIO().ConfigFlags & (int)ImGuiConfigFlags.ImGuiConfigFlagsViewportsEnable) > 0)
            {
                ImGui.UpdatePlatformWindows();
                ImGui.RenderPlatformWindowsDefault(IntPtr.Zero, IntPtr.Zero);
            }
        }

        /// <summary>
        /// [Internal] Checks if the provided window handle matches the window handle associated with this context.
        /// If not initialised, accepts only IntPtr.Zero
        /// </summary>
        /// <param name="windowHandle">The window handle.</param>
        public bool CheckWindowHandle(nint windowHandle)
        {
            // Check for exact handle.
            if (windowHandle != 0)
                return windowHandle == WindowHandle || !Initialized;

            return false;
        }


        /// <summary>
        /// Hooks WndProc to allow for input for ImGui
        /// </summary>
        //[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
        private unsafe nint WndProcHandler(nint hWnd, uint msg, nint wParam, nint lParam)
        {
            var io = ImGui.GetIO();
            ImGui.ImplWin32_WndProcHandler((void*)hWnd, msg, wParam, lParam);

            return this.WndProcHook.Hook.OriginalFunction.Invoke(hWnd, msg, wParam, lParam);
        }

    }
}
