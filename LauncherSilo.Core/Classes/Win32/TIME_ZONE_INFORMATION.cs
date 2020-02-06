using System;
using System.Runtime.InteropServices;

namespace Win32
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct TIME_ZONE_INFORMATION
	{
		public int Bias;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]        
		public string StandardName;

		public SYSTEMTIME StandardDate;

		public int StandardBias;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string DaylightName;

		public SYSTEMTIME DaylightDate;

		public int DaylightBias;
	}
}
