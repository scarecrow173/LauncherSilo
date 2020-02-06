using System;
using System.Runtime.InteropServices;

namespace Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct OVERLAPPED
	{
		public int Internal;

		public int InternalHigh;

		public UInt64 offset;

		public IntPtr hEvent;
	}
}
