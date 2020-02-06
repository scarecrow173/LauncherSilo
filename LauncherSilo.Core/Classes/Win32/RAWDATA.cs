using System;
using System.Runtime.InteropServices;

namespace Win32
{
    [StructLayout(LayoutKind.Explicit)]
    public struct RAWDATA
    {
        [FieldOffset(0)]
        public RAWMOUSE mouse;
        [FieldOffset(0)]
        public RAWKEYBOARD keyboard;
        [FieldOffset(0)]
        public RAWHID hid;
    }
}
