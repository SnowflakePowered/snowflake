using DearImguiSharp;
using Reloaded.Hooks;
using Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks.WndProc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Render
{
    // No need to update windows because we don't support viewports.
    internal abstract class ImGuiWndProcHandler
    {
        protected nint outputWindowHandle = 0;

        private WndProcHook? WndProcHook = null;

        private unsafe nint WndProcHandler(nint hWnd, uint msg, nint wParam, nint lParam)
        {
            var io = ImGui.GetIO();
            ImGui.ImplWin32_WndProcHandler((void*)hWnd, msg, wParam, lParam);
            return this.WndProcHook?.Hook.OriginalFunction.Invoke(hWnd, msg, wParam, lParam) ?? -1;
        }

        protected void InvalidateIO()
        {
            ImGui.ImGuiImplWin32Shutdown();
            this.WndProcHook?.Hook.Disable();
            this.WndProcHook = null;
        }

        protected bool PrepareForIO(nint outputWindowHandle)
        {
            if (this.WndProcHook != null && this.outputWindowHandle == outputWindowHandle)
                return true;
            this.InvalidateIO();

            ImGui.ImGuiImplWin32Init(outputWindowHandle);
            this.WndProcHook = new WndProcHook(outputWindowHandle, this.WndProcHandler);
            this.WndProcHook.Hook.Activate();
            return true;
        }
    }
}
