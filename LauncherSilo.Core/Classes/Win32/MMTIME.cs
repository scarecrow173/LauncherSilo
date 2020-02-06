using System;
using System.Runtime.InteropServices;

namespace Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MMTIME
	{
		public int wType;

		public int u;
	}
}
