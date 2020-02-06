using System;
using System.Runtime.InteropServices;

namespace Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MIDIPROPTEMPO
	{
		public int cbStruct;

		public int dwTempo;
	}
}
