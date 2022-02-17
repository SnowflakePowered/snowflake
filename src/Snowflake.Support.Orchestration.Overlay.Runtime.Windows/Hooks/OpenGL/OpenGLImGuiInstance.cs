
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
        public ImGuiBackendOpenGL Backend { get; set; }

        IntPtr context;

        private nint outputWindowHandle;

        public void InitializeDevices(GL gl, nint hWnd)
        {
            // OpenGL backend does not support reusable contexts.
            Console.WriteLine("reinitializing imgui context");

            context = ImGui.CreateContext();
            ImGui.SetCurrentContext(context);
            ImGui.StyleColorsDark();
            this.Backend = new ImGuiBackendOpenGL();

            this.Backend.Init(gl);
            this.Backend.CreateDeviceObjects();

            this.outputWindowHandle = hWnd;
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

        private void DiscardContext()
        {
            this.Backend?.Shutdown();

            // OpenGL backend does not support reusable contexts.
            if (context != IntPtr.Zero)
                ImGui.DestroyContext(context);

            this.DevicesReady = false;
        }

        public unsafe bool PrepareForPaint(GL gl, nint hWnd, Vector2 screenDim)
        {
            if (hWnd != this.outputWindowHandle)
            {
                Console.WriteLine($"GL context outdated and discarded was {this.outputWindowHandle} now {hWnd}.");
                this.DiscardContext();
            }

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
