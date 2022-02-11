using ImGui = DearImguiSharp;

using Silk.NET.OpenGL;
using Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Render;
using DearImguiSharp;
using Vanara.PInvoke;

namespace Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks.OpenGL
{
    internal class OpenGLImGuiInstance
    {
        private bool DevicesReady { get; set; }

        public OpenGLImGuiInstance()
        {
            this.Context = ImGui.ImGui.CreateContext(null);
            this.WndProc = new ImGuiWndProcHandler();
            ImGui.ImGui.StyleColorsDark(null);
        }

        public void InitializeDevices(HDC hDC, nint hWnd)
        {
            this.WndProc.InitializeIO(hWnd);
        }
        public ImGuiContext Context { get; }
        public ImGuiWndProcHandler WndProc { get; }
    }
}
