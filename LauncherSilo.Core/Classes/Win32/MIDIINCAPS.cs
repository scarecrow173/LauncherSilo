using System;
using System.Runtime.InteropServices;

namespace Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MIDIINCAPS
	{
		public ushort wMid;

		public ushort wPid;

		public uint vDriverVersion;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szPname;

        public uint dwSupport;
    }
}
