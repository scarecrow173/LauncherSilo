using System;
using System.Runtime.InteropServices;

namespace Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RAWINPUTDEVICE
    {
        public HIDUSAGEPAGE UsagePage;
        public HIDUSAGE Usage;
        public RAWINPUTDEVICEFLAGS Flags;
        public IntPtr Target;
    }
}
