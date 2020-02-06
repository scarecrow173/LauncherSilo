using System;
using System.Runtime.InteropServices;

namespace Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RAWHID
    {
        public uint dwSizHid;
        public uint dwCount;
        public byte bRawData;
    }
}
