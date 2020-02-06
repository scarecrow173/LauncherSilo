using System;
using System.Runtime.InteropServices;

namespace Win32
{
    public struct DEVICEINFOKEYBOARD
    {
        public uint Type;                       // Type of the keyboard
        public uint SubType;                    // Subtype of the keyboard
        public uint KeyboardMode;               // The scan code mode
        public uint NumberOfFunctionKeys;       // Number of function keys on the keyboard
        public uint NumberOfIndicators;         // Number of LED indicators on the keyboard
        public uint NumberOfKeysTotal;          // Total number of keys on the keyboard
    }
}
