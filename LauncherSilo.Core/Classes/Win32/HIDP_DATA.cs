using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Win32
{
    [StructLayout(LayoutKind.Explicit)]
    public struct HIDP_DATA
    {
        [FieldOffset(0)]
        public short DataIndex;
        [FieldOffset(2)]
        public short Reserved;

        [FieldOffset(4)]
        public int RawValue;
        [FieldOffset(4), MarshalAs(UnmanagedType.U1)]
        public bool On;
    }
}