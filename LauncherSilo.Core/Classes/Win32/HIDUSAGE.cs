using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win32
{
    public enum HIDUSAGE : ushort
    {
        Undefined = 0x00,       // Unknown usage
        Pointer = 0x01,         // Pointer
        Mouse = 0x02,           // Mouse
        Joystick = 0x04,        // Joystick
        Gamepad = 0x05,         // Game Pad
        Keyboard = 0x06,        // Keyboard
        Keypad = 0x07,          // Keypad
        SystemControl = 0x80,   // Muilt-axis Controller
        Consumer = 0x0C,        // Consumer
    }
}
