using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Snowball.CLI
{
    //http://stackoverflow.com/a/23947777
    public static class ConsoleEx
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetConsoleHistoryInfo(CONSOLE_HISTORY_INFO consoleHistoryInfo);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GetConsoleHistoryInfo(CONSOLE_HISTORY_INFO consoleHistoryInfo);

        [StructLayout(LayoutKind.Sequential)]
        private class CONSOLE_HISTORY_INFO
        {
            public uint cbSize;
            public uint BufferSize;
            public uint BufferCount;
            public uint TrimDuplicates;
        }

        public static void ClearConsoleHistory()
        {
            var chi = new CONSOLE_HISTORY_INFO
            {
                cbSize = (uint) Marshal.SizeOf(typeof (CONSOLE_HISTORY_INFO))
            };

            if (!ConsoleEx.GetConsoleHistoryInfo(chi))
            {
                return;
            }

            uint originalBufferSize = chi.BufferSize;
            chi.BufferSize = 0;

            if (!ConsoleEx.SetConsoleHistoryInfo(chi))
            {
                return;
            }

            chi.BufferSize = originalBufferSize;

            if (!ConsoleEx.SetConsoleHistoryInfo(chi))
            {
                return;
            }
        }
    }
}
