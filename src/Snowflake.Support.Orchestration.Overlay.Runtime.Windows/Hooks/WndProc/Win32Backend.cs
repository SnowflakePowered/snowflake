using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Vanara.PInvoke;

namespace Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks.WndProc
{
    // For now taken from
    // https://github.com/ff-meli/ImGuiScene/blob/master/ImGuiScene/ImGui_Impl/Input/ImGui_Input_Impl_Direct.cs 
    // We will eventually not actually need this at all, since we will be installing our own hooks and UI driver is really
    // the CEF texture and not ImGui, but whatever
    //
    // largely a port of https://github.com/ocornut/imgui/blob/master/examples/imgui_impl_User32.cpp, though some changes
    // and wndproc hooking
#if DEBUG_INPUT
    public class Win32Backend
    {
        [DllImport("user32.dll")]
        public static extern long CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, ulong wParam, long lParam);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort HIWORD(ulong val)
        {
            // #define HIWORD(l)  ((WORD)((((DWORD_PTR)(l)) >> 16) & 0xffff))
            return (ushort)((val >> 16) & 0xFFFF);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort LOWORD(ulong val)
        {
            // #define LOWORD(l)  ((WORD)(((DWORD_PTR)(l)) & 0xffff))
            return (ushort)(val & 0xFFFF);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort GET_XBUTTON_WPARAM(ulong val)
        {
            // #define GET_XBUTTON_WPARAM(wParam)  (HIWORD(wParam))
            return HIWORD(val);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short GET_WHEEL_DELTA_WPARAM(ulong val)
        {
            // #define GET_WHEEL_DELTA_WPARAM(wParam)  ((short)HIWORD(wParam))
            return (short)HIWORD(val);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GET_X_LPARAM(ulong val)
        {
            // #define GET_X_LPARAM(lp)  ((int)(short)LOWORD(lp))
            return (int)(short)LOWORD(val);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GET_Y_LPARAM(ulong val)
        {
            // #define GET_Y_LPARAM(lp)  ((int)(short)HIWORD(lp))
            return (int)(short)HIWORD(val);
        }

        delegate long WndProcDelegate(IntPtr hWnd, uint msg, ulong wParam, long lParam);

        private long _lastTime;
        private IntPtr _platformNamePtr;
        private IntPtr _iniPathPtr;
        private IntPtr _hWnd;
        private WndProcDelegate _wndProcDelegate;
        private IntPtr _wndProcPtr;
        private IntPtr _oldWndProcPtr;
        private ImGuiMouseCursor _oldCursor = ImGuiMouseCursor.None;
        private User32.SafeHCURSOR[] _cursors;

        public Win32Backend(IntPtr hWnd)
        {
            _hWnd = hWnd;

            // hook wndproc
            // have to hold onto the delegate to keep it in memory for unmanaged code
            _wndProcDelegate = WndProcDetour;
            _wndProcPtr = Marshal.GetFunctionPointerForDelegate(_wndProcDelegate);
            _oldWndProcPtr = User32.SetWindowLong(_hWnd, User32.WindowLongFlags.GWL_WNDPROC, _wndProcPtr);

            var io = ImGui.GetIO();

            io.BackendFlags = io.BackendFlags | (ImGuiBackendFlags.HasMouseCursors | ImGuiBackendFlags.HasSetMousePos);

            _platformNamePtr = Marshal.StringToHGlobalAnsi("imgui_impl_User32_c#");
            unsafe
            {
                io.NativePtr->BackendPlatformName = (byte*)_platformNamePtr.ToPointer();
            }


            io.KeyMap[(int)ImGuiKey.Tab] = (int)User32.VK.VK_TAB;
            io.KeyMap[(int)ImGuiKey.LeftArrow] = (int)User32.VK.VK_LEFT;
            io.KeyMap[(int)ImGuiKey.RightArrow] = (int)User32.VK.VK_RIGHT;
            io.KeyMap[(int)ImGuiKey.UpArrow] = (int)User32.VK.VK_UP;
            io.KeyMap[(int)ImGuiKey.DownArrow] = (int)User32.VK.VK_DOWN;
            io.KeyMap[(int)ImGuiKey.PageUp] = (int)User32.VK.VK_PRIOR;
            io.KeyMap[(int)ImGuiKey.PageDown] = (int)User32.VK.VK_NEXT;
            io.KeyMap[(int)ImGuiKey.Home] = (int)User32.VK.VK_HOME;
            io.KeyMap[(int)ImGuiKey.End] = (int)User32.VK.VK_END;
            io.KeyMap[(int)ImGuiKey.Insert] = (int)User32.VK.VK_INSERT;
            io.KeyMap[(int)ImGuiKey.Delete] = (int)User32.VK.VK_DELETE;
            io.KeyMap[(int)ImGuiKey.Backspace] = (int)User32.VK.VK_BACK;
            io.KeyMap[(int)ImGuiKey.Space] = (int)User32.VK.VK_SPACE;
            io.KeyMap[(int)ImGuiKey.Enter] = (int)User32.VK.VK_RETURN;
            io.KeyMap[(int)ImGuiKey.Escape] = (int)User32.VK.VK_ESCAPE;
            io.KeyMap[(int)ImGuiKey.KeyPadEnter] = (int)User32.VK.VK_RETURN; // same keycode, lparam is different.  Not sure if this will cause dupe events or not
            io.KeyMap[(int)ImGuiKey.A] = (int)User32.VK.VK_A;
            io.KeyMap[(int)ImGuiKey.C] = (int)User32.VK.VK_C;
            io.KeyMap[(int)ImGuiKey.V] = (int)User32.VK.VK_V;
            io.KeyMap[(int)ImGuiKey.X] = (int)User32.VK.VK_X;
            io.KeyMap[(int)ImGuiKey.Y] = (int)User32.VK.VK_Y;
            io.KeyMap[(int)ImGuiKey.Z] = (int)User32.VK.VK_Z;

            _cursors = new User32.SafeHCURSOR[8];
            _cursors[(int)ImGuiMouseCursor.Arrow] = User32.LoadCursor(IntPtr.Zero, User32.IDC_ARROW);
            _cursors[(int)ImGuiMouseCursor.TextInput] = User32.LoadCursor(IntPtr.Zero, User32.IDC_IBEAM);
            _cursors[(int)ImGuiMouseCursor.ResizeAll] = User32.LoadCursor(IntPtr.Zero, User32.IDC_SIZEALL);
            _cursors[(int)ImGuiMouseCursor.ResizeEW] = User32.LoadCursor(IntPtr.Zero, User32.IDC_SIZEWE);
            _cursors[(int)ImGuiMouseCursor.ResizeNS] = User32.LoadCursor(IntPtr.Zero, User32.IDC_SIZENS);
            _cursors[(int)ImGuiMouseCursor.ResizeNESW] = User32.LoadCursor(IntPtr.Zero, User32.IDC_SIZENESW);
            _cursors[(int)ImGuiMouseCursor.ResizeNWSE] = User32.LoadCursor(IntPtr.Zero, User32.IDC_SIZENWSE);
            _cursors[(int)ImGuiMouseCursor.Hand] = User32.LoadCursor(IntPtr.Zero, User32.IDC_HAND);
        }

        //public bool IsImGuiCursor(IntPtr hCursor)
        //{
        //    return _cursors.Contains(hCursor) ?? false;
        //}

        public void NewFrame(int targetWidth, int targetHeight)
        {
            var io = ImGui.GetIO();

            io.DisplaySize.X = targetWidth;
            io.DisplaySize.Y = targetHeight;
            io.DisplayFramebufferScale.X = 1f;
            io.DisplayFramebufferScale.Y = 1f;

            var frequency = Stopwatch.Frequency;
            var currentTime = Stopwatch.GetTimestamp();
            io.DeltaTime = _lastTime > 0 ? (float)((double)(currentTime - _lastTime) / frequency) : 1f / 60;
            _lastTime = currentTime;

            io.KeyCtrl = (User32.GetKeyState((int)User32.VK.VK_CONTROL) & 0x8000) != 0;
            io.KeyShift = (User32.GetKeyState((int)User32.VK.VK_SHIFT) & 0x8000) != 0;
            io.KeyAlt = (User32.GetKeyState((int)User32.VK.VK_MENU) & 0x8000) != 0;
            io.KeySuper = false;

            UpdateMousePos();

            // TODO: need to figure out some way to unify all this
            // The bottom case works(?) if the caller hooks SetCursor, but otherwise causes fps issues
            // The top case more or less works if we use ImGui's software cursor (and ideally hide the
            // game's hardware cursor)
            // It would be nice if hooking WM_SETCURSOR worked as it 'should' so that external hooking
            // wasn't necessary

            // this is what imgui's example does, but it doesn't seem to work for us
            // this could be a timing issue.. or their logic could just be wrong for many applications
            var cursor = io.MouseDrawCursor ? ImGuiMouseCursor.None : ImGui.GetMouseCursor();
            if (_oldCursor != cursor)
            {
                _oldCursor = cursor;
                UpdateMouseCursor();
            }
            
            // this mouse logic makes the window scroll feel bad.

            // hacky attempt to make cursors work how I think they 'should'
            //if (io.WantCaptureMouse || io.MouseDrawCursor)
            //{
            //    UpdateMouseCursor();
            //}
        }

        public void SetIniPath(string iniPath)
        {
            // TODO: error/messaging when trying to set after first render?
            if (iniPath != null)
            {
                if (_iniPathPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(_iniPathPtr);
                }

                _iniPathPtr = Marshal.StringToHGlobalAnsi(iniPath);
                unsafe
                {
                    ImGui.GetIO().NativePtr->IniFilename = (byte*)_iniPathPtr.ToPointer();
                }
            }
        }

        private void UpdateMousePos()
        {
            var io = ImGui.GetIO();

            if (io.WantSetMousePos)
            {
                var pos = new Point { X = (int)io.MousePos.X, Y = (int)io.MousePos.Y };
                User32.ClientToScreen(_hWnd, ref pos);
                User32.SetCursorPos(pos.X, pos.Y);
            }

            //if (HWND active_window = ::GetForegroundWindow())
            //    if (active_window == g_hWnd || ::IsChild(active_window, g_hWnd))
            if (User32.GetCursorPos(out Point pt) && User32.ScreenToClient(_hWnd, ref pt))
            {
                io.MousePos.X = pt.X;
                io.MousePos.Y = pt.Y;
            }
            else
            {
                io.MousePos.X = float.MinValue;
                io.MousePos.Y = float.MinValue;
            }
        }

        private bool UpdateMouseCursor()
        {
            var io = ImGui.GetIO();
            if (io.ConfigFlags.HasFlag(ImGuiConfigFlags.NoMouseCursorChange))
            {
                return false;
            }

            var cur = ImGui.GetMouseCursor();
            if (cur == ImGuiMouseCursor.None || io.MouseDrawCursor)
            {
                User32.SetCursor(null);
            }
            else
            {
                User32.SetCursor(_cursors[(int)cur]);
            }

            return true;
        }

        private long WndProcDetour(IntPtr hWnd, uint msg, ulong wParam, long lParam)
        {
            if (hWnd == _hWnd && ImGui.GetCurrentContext() != IntPtr.Zero && (ImGui.GetIO().WantCaptureMouse || ImGui.GetIO().WantCaptureKeyboard))
            {
                var io = ImGui.GetIO();
                var wmsg = (User32.WindowMessage)msg;

                switch (wmsg)
                {
                    case User32.WindowMessage.WM_LBUTTONDOWN:
                    case User32.WindowMessage.WM_LBUTTONDBLCLK:
                    case User32.WindowMessage.WM_RBUTTONDOWN:
                    case User32.WindowMessage.WM_RBUTTONDBLCLK:
                    case User32.WindowMessage.WM_MBUTTONDOWN:
                    case User32.WindowMessage.WM_MBUTTONDBLCLK:
                    case User32.WindowMessage.WM_XBUTTONDOWN:
                    case User32.WindowMessage.WM_XBUTTONDBLCLK:
                        if (io.WantCaptureMouse)
                        {
                            var button = 0;
                            if (wmsg == User32.WindowMessage.WM_LBUTTONDOWN || wmsg == User32.WindowMessage.WM_LBUTTONDBLCLK)
                            {
                                button = 0;
                            }
                            else if (wmsg == User32.WindowMessage.WM_RBUTTONDOWN || wmsg == User32.WindowMessage.WM_RBUTTONDBLCLK)
                            {
                                button = 1;
                            }
                            else if (wmsg == User32.WindowMessage.WM_MBUTTONDOWN || wmsg == User32.WindowMessage.WM_MBUTTONDBLCLK)
                            {
                                button = 2;
                            }
                            else if (wmsg == User32.WindowMessage.WM_XBUTTONDOWN || wmsg == User32.WindowMessage.WM_XBUTTONDBLCLK)
                            {
                                button = GET_XBUTTON_WPARAM(wParam) == 1 /* XBUTTON1 */ ? 3 : 4;
                            }

                            if (!ImGui.IsAnyMouseDown() && User32.GetCapture() == IntPtr.Zero)
                            {
                                User32.SetCapture(hWnd);
                            }
                            io.MouseDown[button] = true;
                            return 0;
                        }
                        break;

                    case User32.WindowMessage.WM_LBUTTONUP:
                    case User32.WindowMessage.WM_RBUTTONUP:
                    case User32.WindowMessage.WM_MBUTTONUP:
                    case User32.WindowMessage.WM_XBUTTONUP:
                        if (io.WantCaptureMouse)
                        {
                            var button = 0;
                            if (wmsg == User32.WindowMessage.WM_LBUTTONUP)
                            {
                                button = 0;
                            }
                            else if (wmsg == User32.WindowMessage.WM_RBUTTONUP)
                            {
                                button = 1;
                            }
                            else if (wmsg == User32.WindowMessage.WM_MBUTTONUP)
                            {
                                button = 2;
                            }
                            else if (wmsg == User32.WindowMessage.WM_XBUTTONUP)
                            {
                                button = GET_XBUTTON_WPARAM(wParam) == 1 /* XBUTTON1 */ ? 3 : 4;
                            }

                            if (!ImGui.IsAnyMouseDown() && User32.GetCapture() == hWnd)
                            {
                                User32.ReleaseCapture();
                            }
                            io.MouseDown[button] = false;
                            return 0;
                        }
                        break;

                    case User32.WindowMessage.WM_MOUSEWHEEL:
                        if (io.WantCaptureMouse)
                        {
                            io.MouseWheel += (float)GET_WHEEL_DELTA_WPARAM(wParam) / (float)120 /* WHEEL_DELTA */;
                            return 0;
                        }
                        break;

                    case User32.WindowMessage.WM_MOUSEHWHEEL:
                        if (io.WantCaptureMouse)
                        {
                            io.MouseWheelH += (float)GET_WHEEL_DELTA_WPARAM(wParam) / (float)120 /* WHEEL_DELTA */;
                            return 0;
                        }
                        break;

                    case User32.WindowMessage.WM_KEYDOWN:
                    case User32.WindowMessage.WM_SYSKEYDOWN:
                        if (io.WantCaptureKeyboard)
                        {
                            if (wParam < 256)
                            {
                                io.KeysDown[(int)wParam] = true;
                            }
                            return 0;
                        }
                        break;

                    case User32.WindowMessage.WM_KEYUP:
                    case User32.WindowMessage.WM_SYSKEYUP:
                        if (io.WantCaptureKeyboard)
                        {
                            if (wParam < 256)
                            {
                                io.KeysDown[(int)wParam] = false;
                            }
                            return 0;
                        }
                        break;

                    case User32.WindowMessage.WM_CHAR:
                        if (io.WantCaptureKeyboard)
                        {
                            io.AddInputCharacter((uint)wParam);
                            return 0;
                        }
                        break;

                    // this never seemed to work reasonably, but I'll leave it for now
                    case User32.WindowMessage.WM_SETCURSOR:
                        if (io.WantCaptureMouse)
                        {
                            if (LOWORD((ulong)lParam) == 1 /* HTCLIENT*/ && UpdateMouseCursor())
                            {
                                // this message returns 1 to block further processing
                                // because consistency is no fun
                                return 1;
                            }
                        }
                        break;

                    default:
                        break;
                }
            }
            return CallWindowProc(_oldWndProcPtr, hWnd, msg, wParam, lParam);
        }

#region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _cursors = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                if (_oldWndProcPtr != IntPtr.Zero)
                {
                    User32.SetWindowLong(_hWnd, User32.WindowLongFlags.GWL_WNDPROC, _oldWndProcPtr);
                    _oldWndProcPtr = IntPtr.Zero;
                }

                if (_platformNamePtr != IntPtr.Zero)
                {
                    unsafe
                    {
                        ImGui.GetIO().NativePtr->BackendPlatformName = null;
                    }

                    Marshal.FreeHGlobal(_platformNamePtr);
                    _platformNamePtr = IntPtr.Zero;
                }

                if (_iniPathPtr != IntPtr.Zero)
                {
                    unsafe
                    {
                        ImGui.GetIO().NativePtr->IniFilename = null;
                    }

                    Marshal.FreeHGlobal(_iniPathPtr);
                    _iniPathPtr = IntPtr.Zero;
                }

                disposedValue = true;
            }
        }

        ~Win32Backend()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
#endregion
    }
#endif
}
