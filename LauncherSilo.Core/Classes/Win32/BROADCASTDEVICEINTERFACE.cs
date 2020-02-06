using System;

namespace Win32
{
    public struct BROADCASTDEVICEINTERFACE
    {
        public Int32 DbccSize;
        public BROADCASTDEVICETYPE BroadcastDeviceType;
        public Int32 DbccReserved;
        public Guid DbccClassguid;
        public char DbccName;
    }
}
