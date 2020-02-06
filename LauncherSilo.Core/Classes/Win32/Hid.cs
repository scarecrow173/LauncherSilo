using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Win32
{
    public abstract class Hid
    {
        [DllImport("hid")]
        public static extern bool HidD_FlushQueue(IntPtr hidDeviceObject);

        [DllImport("hid")]
        public static extern bool HidD_GetAttributes(IntPtr hidDeviceObject, ref HIDD_ATTRIBUTES attributes);

        [DllImport("hid")]
        public static extern bool HidD_GetFeature(IntPtr hidDeviceObject, byte[] lpReportBuffer, int reportBufferLength);

        [DllImport("hid")]
        public static extern bool HidD_GetInputReport(IntPtr hidDeviceObject, byte[] lpReportBuffer, int reportBufferLength);

        [DllImport("hid")]
        public static extern void HidD_GetHidGuid(ref Guid hidGuid);

        [DllImport("hid")]
        public static extern bool HidD_GetNumInputBuffers(IntPtr hidDeviceObject, ref int numberBuffers);

        [DllImport("hid")]
        public static extern bool HidD_GetPreparsedData(IntPtr hidDeviceObject, ref IntPtr preparsedData);

        [DllImport("hid")]
        public static extern bool HidD_FreePreparsedData(IntPtr preparsedData);

        [DllImport("hid")]
        public static extern bool HidD_SetFeature(IntPtr hidDeviceObject, byte[] lpReportBuffer, int reportBufferLength);

        [DllImport("hid")]
        public static extern bool HidD_SetNumInputBuffers(IntPtr hidDeviceObject, int numberBuffers);

        [DllImport("hid")]
        public static extern bool HidD_SetOutputReport(IntPtr hidDeviceObject, byte[] lpReportBuffer, int reportBufferLength);


        [DllImport("hid", CharSet = CharSet.Unicode)]
        public static extern bool HidD_GetProductString(IntPtr hidDeviceObject, ref byte lpReportBuffer, int reportBufferLength);

        [DllImport("hid", CharSet = CharSet.Unicode)]
        public static extern bool HidD_GetManufacturerString(IntPtr hidDeviceObject, ref byte lpReportBuffer, int reportBufferLength);

        [DllImport("hid", CharSet = CharSet.Unicode)]
        public static extern bool HidD_GetSerialNumberString(IntPtr hidDeviceObject, ref byte lpReportBuffer, int reportBufferLength);

        [DllImport("hid", CharSet = CharSet.Unicode)]
        public static extern bool HidD_GetIndexedString(IntPtr hidDeviceObject, int StringIndex, ref byte lpReportBuffer, int reportBufferLength);

        [DllImport("hid")]
        public static extern HIDP_ERROR_CODES HidP_GetCaps(IntPtr preparsedData, ref HIDP_CAPS capabilities);

        [DllImport("hid")]
        public static extern HIDP_ERROR_CODES HidP_GetUsages(HIDP_REPORT_TYPE ReportType, ushort UsagePage, ushort LinkCollection, [In, Out] HIDP_DATA[] UsageList, ref uint UsageLength, IntPtr PreparsedData, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 7)] byte[] Report, uint ReportLength);

        [DllImport("hid.dll")]
        public static extern HIDP_ERROR_CODES HidP_GetUsagesEx(HIDP_REPORT_TYPE ReportType, ushort LinkCollection, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] USAGE_AND_PAGE[] ButtonList, ref uint UsageLength, IntPtr PreparsedData, [MarshalAs(UnmanagedType.LPArray)] byte[] Report, uint ReportLength);

        [DllImport("hid")]
        public static extern HIDP_ERROR_CODES HidP_GetButtonCaps(HIDP_REPORT_TYPE reportType, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] HIDP_BUTTON_CAPS[] buttonCaps, ref ushort buttonCapsLength, System.IntPtr preparsedData);

        [DllImport("hid")]
        public static extern HIDP_ERROR_CODES HidP_GetValueCaps(HIDP_REPORT_TYPE reportType, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] HIDP_VALUE_CAPS[] valueCaps, ref ushort valueCapsLength, System.IntPtr preparsedData);

        [DllImport("hid.dll")]
        public static extern HIDP_ERROR_CODES HidP_GetUsageValue(HIDP_REPORT_TYPE reportType, ushort UsagePage, ushort LinkCollection, ushort Usage, ref uint UsageValue, IntPtr PreparsedData, [MarshalAs(UnmanagedType.LPArray)] byte[] Report, uint ReportLength);

        [DllImport("hid.dll")]
        public static extern HIDP_ERROR_CODES HidP_GetScaledUsageValue(HIDP_REPORT_TYPE reportType, ushort UsagePage, ushort LinkCollection, ushort Usage, ref uint UsageValue, IntPtr PreparsedData, [MarshalAs(UnmanagedType.LPArray)] byte[] Report, uint ReportLength);

        [DllImport("hid.dll")]
        public static extern HIDP_ERROR_CODES HidP_GetUsageValueArray(HIDP_REPORT_TYPE ReportType, ushort UsagePage, ushort LinkCollection, ushort Usage, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5)] byte[] UsageValue, short UsageValueByteLength, IntPtr PreparsedData, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 8)] byte[] Report, int ReportLength);
        /* HidD
         * HidD_GetConfiguration
         * HidD_GetMsGenreDescriptor
         * HidD_GetPhysicalDescriptor
         * HidD_Hello
         * HidD_SetConfiguration
         */
        /* HidP
         * HidP_GetData
         * HidP_GetExtendedAttributes
         * HidP_GetLinkCollectionNodes
         * HidP_GetSpecificButtonCaps
         * HidP_GetSpecificValueCaps
         * HidP_GetUsageValueArray
         * HidP_InitializeReportForID
         * HidP_MaxDataListLength
         * HidP_MaxUsageListLength
         * HidP_SetData
         * HidP_SetScaledUsageValue
         * HidP_SetUsages
         * HidP_SetUsageValue
         * HidP_SetUsageValueArray
         * HidP_TranslateUsagesToI8042ScanCodes
         * HidP_UnsetUsages
         * HidP_UsageListDifference
         */
    }
}
