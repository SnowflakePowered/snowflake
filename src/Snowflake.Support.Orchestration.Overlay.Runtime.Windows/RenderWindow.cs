using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vanara.PInvoke;
namespace Snowflake.Support.Orchestration.Overlay.Runtime.Windows
{
    internal class RenderWindow : IDisposable
    {
        private bool disposedValue;

        private WindowClass WindowClass { get; }
        public HWND WindowHandle { get; }
        
        public RenderWindow() 
        {
            this.WindowClass = new WindowClass(
                "SnowflakeOverlay",
                Vanara.PInvoke.Kernel32.GetModuleHandle(),
                User32.DefWindowProc,
                User32.WindowClassStyles.CS_HREDRAW | User32.WindowClassStyles.CS_VREDRAW
                );
            User32.RegisterClassEx(this.WindowClass.wc);
            this.WindowHandle = User32.CreateWindowEx(0, "SnowflakeOverlay", "SnowflakeOverlayInitWindow",
                User32.WindowStyles.WS_OVERLAPPEDWINDOW, 0, 0, 100, 100, (nint)0, (nint)0, this.WindowClass.wc.hInstance);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    User32.DestroyWindow(this.WindowHandle);
                    this.WindowClass.Unregister();
                }
                disposedValue = true;
            }
        }

        ~RenderWindow()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
