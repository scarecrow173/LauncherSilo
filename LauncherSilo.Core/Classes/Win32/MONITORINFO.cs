using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Win32 {
    [StructLayout(LayoutKind.Sequential)]
    public struct MONITORINFO {
        public uint cbSize;

        public RECT rcMonitor;

        public RECT rcWork;

        public uint dwFlags;
    }
}
