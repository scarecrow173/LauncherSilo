using System.Runtime.InteropServices;

namespace Win32
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct SP_DEVICE_INTERFACE_DETAIL_DATA
    {
        public int Size;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string DevicePath;
    }
}
