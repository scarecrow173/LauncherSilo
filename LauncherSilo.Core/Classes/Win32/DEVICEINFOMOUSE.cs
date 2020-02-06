using System;
using System.Runtime.InteropServices;

namespace Win32
{
    public struct DEVICEINFOMOUSE
    {
        public uint Id;                         // Identifier of the mouse device
        public uint NumberOfButtons;            // Number of buttons for the mouse
        public uint SampleRate;                 // Number of data points per second.
        public bool HasHorizontalWheel;         // True is mouse has wheel for horizontal scrolling else false.
    }
}
