using DearImguiSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Render
{
    internal static class ImGuiFullscreenOverlay
    {
        public static readonly DearImguiSharp.ImVec2 Zero2H = new() { X = 0f, Y = 0f };
        public static readonly DearImguiSharp.ImVec2.__Internal Zero2 = new() { x = 0f, y = 0f };
        public static readonly DearImguiSharp.ImVec2.__Internal One2 = new() { x = 1f, y = 1f };
        public static readonly DearImguiSharp.ImVec4.__Internal One4 = new() { w = 1.0f, x = 1.0f, y = 1.0f, z = 1.0f };
        public static readonly DearImguiSharp.ImVec4.__Internal Zero4 = new() { x = 0.0f, y = 0.0f, w = 0.0f, z = 0.0f };

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void Render(nint imageHandle, float width, float height)
        {
            var browserDim = new ImVec2.__Internal() { x = width, y = height };

            var viewPort = ImGui.GetMainViewport();
            ImGui.SetNextWindowPos(viewPort.Pos, 1, Zero2H);
            ImGui.SetNextWindowSize(viewPort.Size, 1);
            //ImGui.SetNextWindowFocus();

            ImGui.__Internal.PushStyleVarVec2((int)ImGuiStyleVar.ImGuiStyleVarWindowPadding, Zero2);
            ImGui.__Internal.PushStyleVarVec2((int)ImGuiStyleVar.ImGuiStyleVarWindowBorderSize, Zero2);

            unsafe
            {
                ImGui.__Internal.Begin("Browser Window", null,
                    (int)(0
                    | ImGuiWindowFlags.ImGuiWindowFlagsNoDecoration
                    | ImGuiWindowFlags.ImGuiWindowFlagsNoMove
                    | ImGuiWindowFlags.ImGuiWindowFlagsNoResize
                    | ImGuiWindowFlags.ImGuiWindowFlagsNoBackground
                    //| DearImguiSharp.ImGuiWindowFlags.ImGuiWindowFlagsNoSavedSettings
                    ));
            }
            ImGui.__Internal.Image(imageHandle, browserDim, Zero2, One2, One4, Zero4);

            ImGui.__Internal.PopStyleVar(1);
            ImGui.__Internal.End();
        }
    }
}
