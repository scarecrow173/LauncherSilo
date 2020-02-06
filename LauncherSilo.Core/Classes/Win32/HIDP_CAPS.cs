using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct HIDP_CAPS
    {
        public UInt16 Usage;
        public UInt16 UsagePage;
        public UInt16 InputReportByteLength;
        public UInt16 OutputReportByteLength;
        public UInt16 FeatureReportByteLength;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
        public UInt16[] Reserved;
        public UInt16 NumberLinkCollectionNodes;
        public UInt16 NumberInputButtonCaps;
        public UInt16 NumberInputValueCaps;
        public UInt16 NumberInputDataIndices;
        public UInt16 NumberOutputButtonCaps;
        public UInt16 NumberOutputValueCaps;
        public UInt16 NumberOutputDataIndices;
        public UInt16 NumberFeatureButtonCaps;
        public UInt16 NumberFeatureValueCaps;
        public UInt16 NumberFeatureDataIndices;
    }
}