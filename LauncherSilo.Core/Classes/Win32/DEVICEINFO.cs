using System;
using System.Runtime.InteropServices;

namespace Win32
{
    [StructLayout(LayoutKind.Explicit)]
    public struct DeviceInfo
    {
        [FieldOffset(0)]
        public int Size;
        [FieldOffset(4)]
        public int Type;

        [FieldOffset(8)]
        public DEVICEINFOMOUSE MouseInfo;
        [FieldOffset(8)]
        public DEVICEINFOKEYBOARD KeyboardInfo;
        [FieldOffset(8)]
        public DEVICEINFOHID HIDInfo;

    }
}
