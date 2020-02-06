using System;
using System.Runtime.InteropServices;

namespace Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RAWINPUTHEADER
    {
        [MarshalAs(UnmanagedType.U4)]
        public uint dwType;
        [MarshalAs(UnmanagedType.U4)]
        public int dwSize;
        public IntPtr hDevice;
        [MarshalAs(UnmanagedType.U4)]
        public int wParam;
    }
}
