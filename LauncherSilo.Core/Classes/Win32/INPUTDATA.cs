using System;
using System.Runtime.InteropServices;

namespace Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct INPUTDATA
    {
        public RAWINPUTHEADER header;           // 64 bit header size: 24  32 bit the header size: 16
        public RAWDATA data;                    // Creating the rest in a struct allows the header size to align correctly for 32/64 bit
    }
}
