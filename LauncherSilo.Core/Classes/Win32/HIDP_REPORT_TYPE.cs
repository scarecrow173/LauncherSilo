using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win32
{
    public enum HIDP_REPORT_TYPE : ushort
    {
        HidP_Input = 0,
        HidP_Output,
        HidP_Feature
    }
}
