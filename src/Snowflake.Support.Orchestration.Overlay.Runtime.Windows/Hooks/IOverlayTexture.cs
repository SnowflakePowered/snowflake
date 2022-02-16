using Snowflake.Orchestration.Ingame;

namespace Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks
{
    internal interface IOverlayTexture
    {
        bool AcquireSync();
        void ReleaseSync();
        bool SizeMatchesViewport(uint width, uint height);
        bool Refresh(OverlayTextureEventParams texParams);
        void Paint(Action<nint, uint, uint> renderFn);

    }
}