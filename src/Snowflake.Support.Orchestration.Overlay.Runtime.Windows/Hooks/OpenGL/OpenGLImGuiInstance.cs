
using ImGuiBackends.OpenGL3;
using ImGuiNET;
using Silk.NET.OpenGL;
using Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Render;
using System.Numerics;
using Vanara.PInvoke;

namespace Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks.OpenGL
{
    internal class OpenGLImGuiInstance
    {
        private bool DevicesReady { get; set; }
        public ImGuiBackendOpenGL Backend { get; }

        IntPtr context;
        private object outputWindowHandle;

        public OpenGLImGuiInstance()
        {
            context = ImGui.CreateContext();
            ImGui.SetCurrentContext(context);
            ImGui.StyleColorsDark();

            this.Backend = new ImGuiBackendOpenGL();
        }

        public void InitializeDevices(GL gl, nint hWnd)
        {
            this.Backend.Init(gl);
            this.Backend.CreateDeviceObjects();

            this.DevicesReady = true;
            //this.WndProc.InitializeIO(hWnd);
        }

        public void NewFrame()
        {
            this.Backend.NewFrame();
        }

        public void Render(ImDrawDataPtr drawData)
        {
            this.Backend.RenderDrawData(drawData);
        }

        public unsafe bool PrepareForPaint(GL gl, nint hWnd, Vector2 screenDim)
        {
            //if (desc.OutputWindow != this.outputWindowHandle)
            //{
            //    Console.WriteLine("Swapchain outdated and so discarded.");
            //    this.DiscardSwapchain();
            //    this.InvalidateDevices();
            //}

            if (!this.DevicesReady)
            {
                this.InitializeDevices(gl, hWnd);
            }

            ImGuiIOPtr io = ImGui.GetIO();
            io.DisplaySize = screenDim;
            this.outputWindowHandle = hWnd;
            return true;
        }

        //public ImGuiContext Context { get; }
        //public ImGuiWndProcHandler WndProc { get; }
    }
}
