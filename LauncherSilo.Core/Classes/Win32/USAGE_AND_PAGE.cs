using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct USAGE_AND_PAGE
    {
        public ushort Usage;
        public ushort UsagePage;
    }
}