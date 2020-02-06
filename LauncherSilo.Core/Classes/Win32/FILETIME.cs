using System;
using System.Runtime.InteropServices;

namespace Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FILETIME
	{
		public uint dwLowDateTime;

		public uint dwHighDateTime;
	}
}
