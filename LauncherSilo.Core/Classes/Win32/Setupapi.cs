using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Win32
{
    public abstract class Setupapi
    {
        public const int INVALID_HANDLE_VALUE = -1;
        public const int MAX_DEV_LEN = 1000;
        public const int SPDRP_ADDRESS = 0x1c;
        public const int SPDRP_BUSNUMBER = 0x15;
        public const int SPDRP_BUSTYPEGUID = 0x13;
        public const int SPDRP_CAPABILITIES = 0xf;
        public const int SPDRP_CHARACTERISTICS = 0x1b;
        public const int SPDRP_CLASS = 7;
        public const int SPDRP_CLASSGUID = 8;
        public const int SPDRP_COMPATIBLEIDS = 2;
        public const int SPDRP_CONFIGFLAGS = 0xa;
        public const int SPDRP_DEVICE_POWER_DATA = 0x1e;
        public const int SPDRP_DEVICEDESC = 0;
        public const int SPDRP_DEVTYPE = 0x19;
        public const int SPDRP_DRIVER = 9;
        public const int SPDRP_ENUMERATOR_NAME = 0x16;
        public const int SPDRP_EXCLUSIVE = 0x1a;
        public const int SPDRP_FRIENDLYNAME = 0xc;
        public const int SPDRP_HARDWAREID = 1;
        public const int SPDRP_LEGACYBUSTYPE = 0x14;
        public const int SPDRP_LOCATION_INFORMATION = 0xd;
        public const int SPDRP_LOWERFILTERS = 0x12;
        public const int SPDRP_MFG = 0xb;
        public const int SPDRP_PHYSICAL_DEVICE_OBJECT_NAME = 0xe;
        public const int SPDRP_REMOVAL_POLICY = 0x1f;
        public const int SPDRP_REMOVAL_POLICY_HW_DEFAULT = 0x20;
        public const int SPDRP_REMOVAL_POLICY_OVERRIDE = 0x21;
        public const int SPDRP_SECURITY = 0x17;
        public const int SPDRP_SECURITY_SDS = 0x18;
        public const int SPDRP_SERVICE = 4;
        public const int SPDRP_UI_NUMBER = 0x10;
        public const int SPDRP_UI_NUMBER_DESC_FORMAT = 0x1d;

        public const int SPDRP_UPPERFILTERS = 0x11;

        [DllImport("setupapi.dll")]
        public static extern bool SetupDiGetDeviceRegistryProperty(IntPtr deviceInfoSet, ref SP_DEVINFO_DATA deviceInfoData, int propertyVal, ref int propertyRegDataType, byte[] propertyBuffer, int propertyBufferSize, ref int requiredSize);

        [DllImport("setupapi.dll")]
        public static extern bool SetupDiGetDeviceProperty(IntPtr deviceInfo, ref SP_DEVINFO_DATA deviceInfoData, ref DEVPROPKEY propkey, ref ulong propertyDataType, byte[] propertyBuffer, int propertyBufferSize, ref int requiredSize, uint flags);

        [DllImport("setupapi.dll")]
        public static extern bool SetupDiEnumDeviceInfo(IntPtr deviceInfoSet, int memberIndex, ref SP_DEVINFO_DATA deviceInfoData);

        [DllImport("setupapi.dll")]
        public static extern int SetupDiCreateDeviceInfoList(ref Guid classGuid, int hwndParent);

        [DllImport("setupapi.dll")]
        public static extern int SetupDiDestroyDeviceInfoList(IntPtr deviceInfoSet);

        [DllImport("setupapi.dll")]
        public static extern bool SetupDiEnumDeviceInterfaces(IntPtr deviceInfoSet, ref SP_DEVINFO_DATA deviceInfoData, ref Guid interfaceClassGuid, int memberIndex, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

        [DllImport("setupapi.dll")]
        public static extern IntPtr SetupDiGetClassDevs(ref Guid classGuid, string enumerator, int hwndParent, int flags);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, EntryPoint = "SetupDiGetDeviceInterfaceDetail")]
        public static extern bool SetupDiGetDeviceInterfaceDetailBuffer(IntPtr deviceInfoSet, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData, IntPtr deviceInterfaceDetailData, int deviceInterfaceDetailDataSize, ref int requiredSize, IntPtr deviceInfoData);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        public static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr deviceInfoSet, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData, ref SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData, int deviceInterfaceDetailDataSize, ref int requiredSize, IntPtr deviceInfoData);
    }
}
