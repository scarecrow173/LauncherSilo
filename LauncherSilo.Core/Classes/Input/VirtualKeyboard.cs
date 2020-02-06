using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

namespace Input
{
    public class VirtualKeyboard
    {
        private struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public int mouseData;
            public int dwFlags;
            public int time;
            public int dwExtraInfo;
        }
        private struct KEYBDINPUT
        {
            public short wVk;
            public short wScan;
            public int dwFlags;
            public int time;
            public int dwExtraInfo;
        }
        private struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }
        [StructLayout(LayoutKind.Explicit)]
        private struct INPUT
        {
            [FieldOffset(0)]
            public int type;
            [FieldOffset(4)]
            public VirtualKeyboard.MOUSEINPUT mi;
            [FieldOffset(4)]
            public VirtualKeyboard.KEYBDINPUT ki;
            [FieldOffset(4)]
            public VirtualKeyboard.HARDWAREINPUT hi;
        }
        private const int INPUT_MOUSE = 0;
        private const int INPUT_KEYBOARD = 1;
        private const int INPUT_HARDWARE = 2;
        private const int MOUSEEVENTF_MOVE = 1;
        private const int MOUSEEVENTF_ABSOLUTE = 32768;
        private const int MOUSEEVENTF_LEFTDOWN = 2;
        private const int MOUSEEVENTF_LEFTUP = 4;
        private const int MOUSEEVENTF_RIGHTDOWN = 8;
        private const int MOUSEEVENTF_RIGHTUP = 16;
        private const int MOUSEEVENTF_MIDDLEDOWN = 32;
        private const int MOUSEEVENTF_MIDDLEUP = 64;
        private const int MOUSEEVENTF_WHEEL = 2048;
        private const int WHEEL_DELTA = 120;
        private const int KEYEVENTF_KEYDOWN = 0;
        private const int KEYEVENTF_KEYUP = 2;
        private const int KEYEVENTF_EXTENDEDKEY = 1;
        private const int VK_SHIFT = 16;
        [DllImport("user32.dll")]
        private static extern void SendInput(int nInputs, ref VirtualKeyboard.INPUT pInputs, int cbsize);
        [DllImport("user32.dll", EntryPoint = "MapVirtualKeyA")]
        private static extern int MapVirtualKey(int wCode, int wMapType);
        public static void SendKeyEnter(int KeyInterval = 20)
        {
            VirtualKeyboard.INPUT[] array = new VirtualKeyboard.INPUT[]
            {
                VirtualKeyboard.CreateInput(Keys.Return, true),
                VirtualKeyboard.CreateInput(Keys.Return, false)
            };
            for (int i = 0; i < array.Length; i++)
            {
                VirtualKeyboard.SendInput(1, ref array[i], Marshal.SizeOf(array[i]));
                Thread.Sleep(KeyInterval);
            }
        }
        public static bool SendTextByCopyPaste(string TextString, int KeyInterval = 20)
        {
            if (string.IsNullOrEmpty(TextString))
            {
                return false;
            }
            try
            {
                Clipboard.SetText(TextString);
            }
            catch
            {
                return false;
            }
            List<VirtualKeyboard.INPUT> list = new List<VirtualKeyboard.INPUT>();
            list.Add(VirtualKeyboard.CreateInput(Keys.LControlKey, true));
            list.AddRange(VirtualKeyboard.CreateInputMomentary(Keys.V));
            list.Add(VirtualKeyboard.CreateInput(Keys.LControlKey, false));
            VirtualKeyboard.INPUT[] array = list.ToArray();
            for (int i = 0; i < array.Length; i++)
            {
                VirtualKeyboard.SendInput(1, ref array[i], Marshal.SizeOf(array[i]));
                Thread.Sleep(KeyInterval);
            }
            return true;
        }
        public static bool SendText(Process TargetProcess, string TextString, int KeyInterval = 20)
        {
            if (string.IsNullOrEmpty(TextString))
            {
                return false;
            }

            foreach (var ch in TextString)
            {
                if (GetInputFromChar(ch, out Keys Key, out bool Shift))
                {
                    Win32.User32.SendMessage(TargetProcess.MainWindowHandle, 256, (short)Key, 0);
                    Thread.Sleep(KeyInterval);
                    Win32.User32.SendMessage(TargetProcess.MainWindowHandle, 257, (short)Key, 0);
                    Thread.Sleep(KeyInterval);
                }
            }
            return true;
        }
        private static VirtualKeyboard.INPUT CreateInput(Keys key, bool bDown)
        {
            VirtualKeyboard.INPUT result = default(VirtualKeyboard.INPUT);
            result.type = 1;
            result.ki.wVk = (short)key;
            result.ki.wScan = (short)VirtualKeyboard.MapVirtualKey((int)result.ki.wVk, 0);
            result.ki.dwFlags = (1 | (bDown ? 0 : 2));
            result.ki.dwExtraInfo = 0;
            result.ki.time = 0;
            return result;
        }
        private static VirtualKeyboard.INPUT[] CreateInputMomentary(Keys key)
        {
            return new VirtualKeyboard.INPUT[]
            {
                VirtualKeyboard.CreateInput(key, true),
                VirtualKeyboard.CreateInput(key, false)
            };
        }
        public static bool GetInputFromChar(char c, out Keys outKey, out bool outShift)
        {
            outKey = Keys.None;
            outShift = false;
            Keys keys;
            bool flag;
            switch (c)
            {
                case ' ':
                    keys = Keys.Space;
                    flag = false;
                    break;
                case '!':
                    keys = Keys.D1;
                    flag = true;
                    break;
                case '"':
                    keys = Keys.D2;
                    flag = true;
                    break;
                case '#':
                    keys = Keys.D3;
                    flag = true;
                    break;
                case '$':
                    keys = Keys.D4;
                    flag = true;
                    break;
                case '%':
                    keys = Keys.D5;
                    flag = true;
                    break;
                case '&':
                    keys = Keys.D6;
                    flag = true;
                    break;
                case '\'':
                    keys = Keys.D7;
                    flag = true;
                    break;
                case '(':
                    keys = Keys.D8;
                    flag = true;
                    break;
                case ')':
                    keys = Keys.D9;
                    flag = true;
                    break;
                case '*':
                    keys = Keys.Multiply;
                    flag = true;
                    break;
                case '+':
                    keys = Keys.Add;
                    flag = false;
                    break;
                case ',':
                    keys = Keys.Oemcomma;
                    flag = false;
                    break;
                case '-':
                    keys = Keys.OemMinus;
                    flag = false;
                    break;
                case '.':
                    keys = Keys.OemPeriod;
                    flag = false;
                    break;
                case '/':
                    keys = Keys.OemQuestion;
                    flag = false;
                    break;
                case '0':
                    keys = Keys.NumPad0;
                    flag = false;
                    break;
                case '1':
                    keys = Keys.NumPad1;
                    flag = false;
                    break;
                case '2':
                    keys = Keys.NumPad2;
                    flag = false;
                    break;
                case '3':
                    keys = Keys.NumPad3;
                    flag = false;
                    break;
                case '4':
                    keys = Keys.NumPad4;
                    flag = false;
                    break;
                case '5':
                    keys = Keys.NumPad5;
                    flag = false;
                    break;
                case '6':
                    keys = Keys.NumPad6;
                    flag = false;
                    break;
                case '7':
                    keys = Keys.NumPad7;
                    flag = false;
                    break;
                case '8':
                    keys = Keys.NumPad8;
                    flag = false;
                    break;
                case '9':
                    keys = Keys.NumPad9;
                    flag = false;
                    break;
                case ':':
                    keys = Keys.OemSemicolon;
                    flag = false;
                    break;
                case ';':
                    keys = Keys.Oemplus;
                    flag = false;
                    break;
                case '<':
                    keys = Keys.Oemcomma;
                    flag = true;
                    break;
                case '=':
                    keys = Keys.OemMinus;
                    flag = true;
                    break;
                case '>':
                    keys = Keys.OemPeriod;
                    flag = true;
                    break;
                case '?':
                    keys = Keys.OemQuestion;
                    flag = true;
                    break;
                case '@':
                    keys = Keys.Oemtilde;
                    flag = false;
                    break;
                case 'A':
                    keys = Keys.A;
                    flag = true;
                    break;
                case 'B':
                    keys = Keys.B;
                    flag = true;
                    break;
                case 'C':
                    keys = Keys.C;
                    flag = true;
                    break;
                case 'D':
                    keys = Keys.D;
                    flag = true;
                    break;
                case 'E':
                    keys = Keys.E;
                    flag = true;
                    break;
                case 'F':
                    keys = Keys.F;
                    flag = true;
                    break;
                case 'G':
                    keys = Keys.G;
                    flag = true;
                    break;
                case 'H':
                    keys = Keys.H;
                    flag = true;
                    break;
                case 'I':
                    keys = Keys.I;
                    flag = true;
                    break;
                case 'J':
                    keys = Keys.J;
                    flag = true;
                    break;
                case 'K':
                    keys = Keys.K;
                    flag = true;
                    break;
                case 'L':
                    keys = Keys.L;
                    flag = true;
                    break;
                case 'M':
                    keys = Keys.M;
                    flag = true;
                    break;
                case 'N':
                    keys = Keys.N;
                    flag = true;
                    break;
                case 'O':
                    keys = Keys.O;
                    flag = true;
                    break;
                case 'P':
                    keys = Keys.P;
                    flag = true;
                    break;
                case 'Q':
                    keys = Keys.Q;
                    flag = true;
                    break;
                case 'R':
                    keys = Keys.R;
                    flag = true;
                    break;
                case 'S':
                    keys = Keys.S;
                    flag = true;
                    break;
                case 'T':
                    keys = Keys.T;
                    flag = true;
                    break;
                case 'U':
                    keys = Keys.U;
                    flag = true;
                    break;
                case 'V':
                    keys = Keys.V;
                    flag = true;
                    break;
                case 'W':
                    keys = Keys.W;
                    flag = true;
                    break;
                case 'X':
                    keys = Keys.X;
                    flag = true;
                    break;
                case 'Y':
                    keys = Keys.Y;
                    flag = true;
                    break;
                case 'Z':
                    keys = Keys.Z;
                    flag = true;
                    break;
                case '[':
                    keys = Keys.OemOpenBrackets;
                    flag = false;
                    break;
                case '\\':
                    keys = Keys.OemPipe;
                    flag = false;
                    break;
                case ']':
                    keys = Keys.OemCloseBrackets;
                    flag = false;
                    break;
                case '^':
                    keys = Keys.OemQuotes;
                    flag = false;
                    break;
                case '_':
                    keys = Keys.OemBackslash;
                    flag = true;
                    break;
                case '`':
                    keys = Keys.Oemtilde;
                    flag = true;
                    break;
                case 'a':
                    keys = Keys.A;
                    flag = false;
                    break;
                case 'b':
                    keys = Keys.B;
                    flag = false;
                    break;
                case 'c':
                    keys = Keys.C;
                    flag = false;
                    break;
                case 'd':
                    keys = Keys.D;
                    flag = false;
                    break;
                case 'e':
                    keys = Keys.E;
                    flag = false;
                    break;
                case 'f':
                    keys = Keys.F;
                    flag = false;
                    break;
                case 'g':
                    keys = Keys.G;
                    flag = false;
                    break;
                case 'h':
                    keys = Keys.H;
                    flag = false;
                    break;
                case 'i':
                    keys = Keys.I;
                    flag = false;
                    break;
                case 'j':
                    keys = Keys.J;
                    flag = false;
                    break;
                case 'k':
                    keys = Keys.K;
                    flag = false;
                    break;
                case 'l':
                    keys = Keys.L;
                    flag = false;
                    break;
                case 'm':
                    keys = Keys.M;
                    flag = false;
                    break;
                case 'n':
                    keys = Keys.N;
                    flag = false;
                    break;
                case 'o':
                    keys = Keys.O;
                    flag = false;
                    break;
                case 'p':
                    keys = Keys.P;
                    flag = false;
                    break;
                case 'q':
                    keys = Keys.Q;
                    flag = false;
                    break;
                case 'r':
                    keys = Keys.R;
                    flag = false;
                    break;
                case 's':
                    keys = Keys.S;
                    flag = false;
                    break;
                case 't':
                    keys = Keys.T;
                    flag = false;
                    break;
                case 'u':
                    keys = Keys.U;
                    flag = false;
                    break;
                case 'v':
                    keys = Keys.V;
                    flag = false;
                    break;
                case 'w':
                    keys = Keys.W;
                    flag = false;
                    break;
                case 'x':
                    keys = Keys.X;
                    flag = false;
                    break;
                case 'y':
                    keys = Keys.Y;
                    flag = false;
                    break;
                case 'z':
                    keys = Keys.Z;
                    flag = false;
                    break;
                default:
                    return false;
            }
            outKey = keys;
            outShift = flag;
            return true;
        }
    }
}
