﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win32
{
    public enum HIDUSAGEPAGE : ushort
    {
        UNDEFINED = 0x00,   // Unknown usage page
        GENERIC = 0x01,     // Generic desktop controls
        SIMULATION = 0x02,  // Simulation controls
        VR = 0x03,          // Virtual reality controls
        SPORT = 0x04,       // Sports controls
        GAME = 0x05,        // Games controls
        KEYBOARD = 0x07,    // Keyboard controls
    }
}
