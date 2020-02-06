using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Win32;

namespace Input
{
    public enum MOD_KEY : int
    {
        MOD_NONE = 0x0000,

        MOD_ALT = 0x0001,

        MOD_CONTROL = 0x0002,

        MOD_SHIFT = 0x0004,

        MOD_WIN = 0x0008
    }
    public class Hotkey : IDisposable
    {
        HotkeyForm Form;

        public event EventHandler HotkeyPush;

        public Hotkey(MOD_KEY ModKey, Keys Key)
        {
            Form = new HotkeyForm(ModKey, Key, RaiseHotkeyPush);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            Form.Dispose();
        }
        private void RaiseHotkeyPush()
        {
            if (HotkeyPush != null)
            {
                HotkeyPush(this, EventArgs.Empty);
            }
        }

        private class HotkeyForm : Form
        {
            const int WM_HOTKEY = 0x0312;
            int Id;
            ThreadStart Proc;

            public HotkeyForm(MOD_KEY ModKey, Keys Key, ThreadStart proc)
            {
                this.Proc = proc;
                for (int i = 0x0000; i <= 0xbfff; i++)
                {
                    if (User32.RegisterHotKey(this.Handle, i, (int)ModKey, (int)Key) != 0)
                    {
                        Id = i;
                        break;
                    }
                }
            }

            protected override void WndProc(ref Message Msg)
            {
                base.WndProc(ref Msg);

                if (Msg.Msg == WM_HOTKEY)
                {
                    if ((int)Msg.WParam == Id)
                    {
                        Proc();
                    }
                }
            }

            protected override void Dispose(bool disposing)
            {
                User32.UnregisterHotKey(this.Handle, Id);
                base.Dispose(disposing);
            }
        }
    }
}
