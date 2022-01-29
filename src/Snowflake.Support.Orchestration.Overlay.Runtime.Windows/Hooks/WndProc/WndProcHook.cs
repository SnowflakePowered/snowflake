using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X64 = Reloaded.Hooks.Definitions.X64;
using X86 = Reloaded.Hooks.Definitions.X86;
using System.Runtime.InteropServices;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks;

namespace Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks.WndProc
{
    internal class WndProcHook
    {
        [X64.Function(X64.CallingConventions.Microsoft)]
        [X86.Function(X86.CallingConventions.Stdcall)]
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]

        public delegate nint WndProcCallback(nint hWnd, uint msg, nint wParam, nint lParam);

        private WndProcCallback Callback { get; }

        /// The hook created for the WndProc function.
        /// Can be used to call the original WndProc.
        /// </summary>
        public IHook<WndProcCallback> Hook { get; private set; }

        public WndProcHook(nint hWnd, WndProcCallback callback)
        {
            var wndProcAddr = Native.GetWindowLong(hWnd, Native.GWL.GWL_WNDPROC);
            this.Callback = callback;
            this.Hook = ReloadedHooks.Instance.CreateHook(this.Callback, (long)wndProcAddr);
        }

        static class Native
        {
            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr GetModuleHandle(string lpModuleName);

            public static IntPtr GetWindowLong(IntPtr hWnd, GWL nIndex)
            {
                if (IsWindowUnicode(hWnd))
                    return GetWindowLongW(hWnd, nIndex);

                return GetWindowLongA(hWnd, nIndex);
            }

            private static IntPtr GetWindowLongA(IntPtr hWnd, GWL nIndex)
            {
                var is64Bit = Environment.Is64BitProcess;
                return is64Bit ? GetWindowLongPtr64(hWnd, (int)nIndex) : GetWindowLongPtr32(hWnd, (int)nIndex);
            }

            private static IntPtr GetWindowLongW(IntPtr hWnd, GWL nIndex)
            {
                var is64Bit = Environment.Is64BitProcess;
                return is64Bit ? GetWindowLongPtr64W(hWnd, (int)nIndex) : GetWindowLongPtr32W(hWnd, (int)nIndex);
            }

            public enum GWL
            {
                GWL_WNDPROC = (-4),
                GWL_HINSTANCE = (-6),
                GWL_HWNDPARENT = (-8),
                GWL_STYLE = (-16),
                GWL_EXSTYLE = (-20),
                GWL_USERDATA = (-21),
                GWL_ID = (-12)
            }

            [DllImport("user32.dll")]
            private static extern bool IsWindowUnicode(IntPtr hWnd);

            [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
            private static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

            [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
            private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

            [DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetWindowLongW")]
            private static extern IntPtr GetWindowLongPtr32W(IntPtr hWnd, int nIndex);

            [DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetWindowLongPtrW")]
            private static extern IntPtr GetWindowLongPtr64W(IntPtr hWnd, int nIndex);
        }
    }
}
