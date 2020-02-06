using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct HIDD_ATTRIBUTES
    {
        public Int32 Size;
        public Int16 VendorID;
        public Int16 ProductID;
        public Int16 VersionNumber;
    }
}