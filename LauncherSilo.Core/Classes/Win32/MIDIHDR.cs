using System;
using System.Runtime.InteropServices;

namespace Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MIDIHDR
	{
		public string lpData;

		public int dwBufferLength;

		public int dwBytesRecorded;

		public int dwUser;

		public int dwFlags;

		public int lpNext;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public int Reserved;
	}
}
