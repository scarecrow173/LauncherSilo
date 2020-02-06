using System;
using System.Runtime.InteropServices;
using System.Text;


namespace Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct HIDP_BUTTON_CAPS_RANGE
    {
        public ushort UsageMin;
        public ushort UsageMax;
        public ushort StringMin;
        public ushort StringMax;
        public ushort DesignatorMin;
        public ushort DesignatorMax;
        public ushort DataIndexMin;
        public ushort DataIndexMax;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HIDP_BUTTON_CAPS_NOT_RANGE
    {
        public ushort Usage;
        public ushort Reserved1;
        public ushort StringIndex;
        public ushort Reserved2;
        public ushort DesignatorIndex;
        public ushort Reserved3;
        public ushort DataIndex;
        public ushort Reserved4;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct HIDP_BUTTON_CAPS
    {
        [FieldOffset(0)]
        public ushort UsagePage;
        [FieldOffset(2)]
        public byte ReportID;
        [FieldOffset(3)]
        [MarshalAs(UnmanagedType.U1)]
        public bool IsAlias;
        [FieldOffset(4)]
        public ushort BitField;
        [FieldOffset(6)]
        public ushort LinkCollection;
        [FieldOffset(8)]
        public ushort LinkUsage;
        [FieldOffset(10)]
        public ushort LinkUsagePage;
        [FieldOffset(12)]
        [MarshalAs(UnmanagedType.U1)]
        public bool IsRange;
        [FieldOffset(13)]
        [MarshalAs(UnmanagedType.U1)]
        public bool IsStringRange;
        [FieldOffset(14)]
        [MarshalAs(UnmanagedType.U1)]
        public bool IsDesignatorRange;
        [FieldOffset(15)]
        [MarshalAs(UnmanagedType.U1)]
        public bool IsAbsolute;
        [FieldOffset(16)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.U4)]
        public uint[] Reserved;
        [FieldOffset(56)]
        public HIDP_BUTTON_CAPS_RANGE Range;
        [FieldOffset(56)]
        public HIDP_BUTTON_CAPS_NOT_RANGE NotRange;
    }
}