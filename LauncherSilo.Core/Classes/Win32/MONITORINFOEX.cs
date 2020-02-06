using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Win32 {
    [StructLayout(LayoutKind.Sequential)]
    public struct MONITORINFOEX {
        public uint cbSize;

        public RECT rcMonitor;

        public RECT rcWork;

        public uint dwFlags;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szDevice;
    }
}
