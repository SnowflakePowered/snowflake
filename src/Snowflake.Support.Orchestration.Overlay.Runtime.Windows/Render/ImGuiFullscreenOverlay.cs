using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Render
{
    internal static class ImGuiFullscreenOverlay
    {
        public static readonly Vector2 Zero2 = new(0, 0);
        public static readonly Vector2 One2 = new(1, 1);
        public static readonly Vector4 One4 = new(1, 1, 1, 1);
        public static readonly Vector4 Zero4 = new(0, 0, 0, 0);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void Render(nint imageHandle, float width, float height)
        {
            var browserDim = new Vector2 { X = width, Y = height };

            var viewPort = ImGui.GetMainViewport();
            ImGui.SetNextWindowPos(viewPort.Pos, ImGuiCond.Always, Zero2);
            ImGui.SetNextWindowSize(viewPort.Size, ImGuiCond.Always);
            //ImGui.SetNextWindowFocus();

            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, Zero2);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, Zero2);

            unsafe
            {
                ImGui.Begin("Browser Window",
                      ImGuiWindowFlags.NoDecoration
                    | ImGuiWindowFlags.NoMove
                    | ImGuiWindowFlags.NoResize
                    | ImGuiWindowFlags.NoBackground
                    //| DearImguiSharp.ImGuiWindowFlags.ImGuiWindowFlagsNoSavedSettings
                    );
            }
            ImGui.Image(imageHandle, browserDim, Zero2, One2, One4, Zero4);

            ImGui.PopStyleVar(1);
            ImGui.End();
        }
    }
}
