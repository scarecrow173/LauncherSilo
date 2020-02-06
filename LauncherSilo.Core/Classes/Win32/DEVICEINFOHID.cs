using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DEVICEINFOHID
    {
        public uint VendorID;       // Vendor identifier for the HID
        public uint ProductID;      // Product identifier for the HID
        public uint VersionNumber;  // Version number for the device
        public ushort UsagePage;    // Top-level collection Usage page for the device
        public ushort Usage;        // Top-level collection Usage for the device
    }
}
