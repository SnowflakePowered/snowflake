using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;
using Snowflake.Input;
using Snowflake.Input.Device;
using Snowflake.Service;

namespace Snowflake.Input
{
    public class WiimoteInputEnumerator : InputEnumerator
    {
        private readonly IInputManager inputManager;

        public WiimoteInputEnumerator(ICoreService coreService) : base(coreService)
        {
            this.inputManager = coreService.Get<IInputManager>();
        }

        [DllImport("hid.dll", SetLastError = true)]
        internal extern static bool HidD_SetOutputReport(
            IntPtr HidDeviceObject,
            byte[] lpReportBuffer,
            uint ReportBufferLength);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern SafeFileHandle CreateFile(string fileName,
          [MarshalAs(UnmanagedType.U4)] FileAccess fileAccess,
          [MarshalAs(UnmanagedType.U4)] FileShare fileShare,
          IntPtr securityAttributes,
          [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
          [MarshalAs(UnmanagedType.U4)] uint flags,
          IntPtr template);

        public static SafeHandle CreateFile(string devicePath)
        {
            return WiimoteInputEnumerator.CreateFile(devicePath, FileAccess.ReadWrite, FileShare.ReadWrite, IntPtr.Zero, FileMode.Open,
                0x40000000, IntPtr.Zero);
        }

        //todo factor this out to wiimote specific implementation
        private static bool IsWiimoteConnected(SafeHandle wiimoteHandle)
        {
            var _mBuff = new byte[22];
            _mBuff[0] = 0x15;
            bool good = WiimoteInputEnumerator.HidD_SetOutputReport(wiimoteHandle.DangerousGetHandle(), _mBuff, (uint)_mBuff.Length);
            int error = Marshal.GetLastWin32Error();
            return error != 0x1F && good;
        }

        public override IEnumerable<IInputDevice> GetConnectedDevices()
        {
            var guid = new Guid("0306057e-0000-0000-0000-504944564944");
            return (from device in this.inputManager.GetAllDevices()
                    where device.DI_ProductGUID == guid
                    let wiimoteHandle = WiimoteInputEnumerator.CreateFile(device.DI_InterfacePath)
                    let deviceId = "DEVICE_WII_REMOTE"
                    where WiimoteInputEnumerator.IsWiimoteConnected(wiimoteHandle)
                    select new InputDevice(deviceId, "Wii Remote", InputApi.Other, device, 
                    this.ControllerLayouts[deviceId]));
            //todo get deviceindex from wiimtoe

        }
    }
}
