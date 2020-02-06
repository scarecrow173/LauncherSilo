using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Input;

namespace LauncherSilo.Models
{
    [Serializable]
    public class ShrotcutKeySetting
    {
        public bool IsUseAltKey { get; set; } = false;
        public bool IsUseShiftKey { get; set; } = false;
        public bool IsUseCtrlKey { get; set; } = false;
        public System.Windows.Forms.Keys Key { get; set; } = System.Windows.Forms.Keys.None;
    }
}
