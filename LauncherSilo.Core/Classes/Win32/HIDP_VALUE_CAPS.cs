using System;
using System.Runtime.InteropServices;
using System.Text;


namespace Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct HIDP_VALUE_CAPS_RANGE
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

    /// <summary>
    /// Type created in place of an anonymous struct
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct HIDP_VALUE_CAPS_NOT_RANGE
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
    public struct HIDP_VALUE_CAPS
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
        [MarshalAs(UnmanagedType.U1)]
        public bool HasNull;
        [FieldOffset(17)]
        public byte Reserved;
        [FieldOffset(18)]
        public ushort BitSize;
        [FieldOffset(20)]
        public ushort ReportCount;
        [FieldOffset(22)]
        public ushort Reserved21;
        [FieldOffset(24)]
        public ushort Reserved22;
        [FieldOffset(26)]
        public ushort Reserved23;
        [FieldOffset(28)]
        public ushort Reserved24;
        [FieldOffset(30)]
        public ushort Reserved25;
        [FieldOffset(32)]
        public uint UnitsExp;
        [FieldOffset(36)]
        public uint Units;
        [FieldOffset(40)]
        public int LogicalMin;
        [FieldOffset(44)]
        public int LogicalMax;
        [FieldOffset(48)]
        public int PhysicalMin;
        [FieldOffset(52)]
        public int PhysicalMax;
        [FieldOffset(56)]
        public HIDP_VALUE_CAPS_RANGE Range;
        [FieldOffset(56)]
        public HIDP_VALUE_CAPS_NOT_RANGE NotRange;
    }
}
