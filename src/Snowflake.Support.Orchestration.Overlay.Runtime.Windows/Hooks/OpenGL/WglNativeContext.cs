using Silk.NET.Core.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Vanara.PInvoke;

namespace Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks.OpenGL
{
    internal class WglNativeContext : INativeContext
    {
        public WglNativeContext(Kernel32.SafeHINSTANCE opengl32)
        {
            this.LibraryInstance = opengl32;
            unsafe
            {
                nint wglGPA = this.LibraryInstance.GetProcAddress("wglGetProcAddress");
                this.WglGetProcAddress = (delegate* unmanaged<string, nint>)wglGPA;

                if (this.WglGetProcAddress == null)
                {
                    throw new PlatformNotSupportedException("Unable to find wglGetProcAddress in instance handle.");
                }
            }
        }

        private bool disposedValue;

        unsafe readonly delegate* unmanaged<string, nint> WglGetProcAddress;

        public Kernel32.SafeHINSTANCE LibraryInstance { get; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public nint GetProcAddress(string proc, int? slot = null)
        {
            nint procAddr;
            unsafe
            {
                procAddr = this.WglGetProcAddress(proc);
            }

            if (procAddr == 0)
            {
                procAddr = Kernel32.GetProcAddress(this.LibraryInstance, proc);
            }

            return procAddr;
        }

        public bool TryGetProcAddress(string proc, out nint addr, int? slot = null)
        {
            addr = this.GetProcAddress(proc, slot);
            return addr != 0;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.LibraryInstance.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
