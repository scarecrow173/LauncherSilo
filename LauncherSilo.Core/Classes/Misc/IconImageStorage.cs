using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

using Win32;

namespace Misc
{
    public class IconImageStorage
    {

        static public ImageSource FindSystemIconImage(string path)
        {
            SHFILEINFO SHFileInfo = new SHFILEINFO();
            int hSuccess = Shell32.SHGetFileInfo(path, 0, ref SHFileInfo, Marshal.SizeOf(SHFileInfo), Shell32.SHGFI_SYSICONINDEX | Shell32.SHGFI_ICON | Shell32.SHGFI_LARGEICON);
            if (hSuccess == 0)
            {
                return null;
            }
            if (SystemIconImageDictionary.ContainsKey(SHFileInfo.iIcon))
            {
                return SystemIconImageDictionary[SHFileInfo.iIcon];
            }
            else
            {
                
                Icon appIcon = Icon.FromHandle(SHFileInfo.hIcon);
                ImageSource IconImage = IconToImageSource(appIcon);
                lock (SystemIconImageDictionary) { SystemIconImageDictionary.Add(SHFileInfo.iIcon, IconImage); }
                return IconImage;
            }
        }
        static public ImageSource FindIconImage(string path, int number)
        {
            string IconKey = CreateIconKeyInfo(path, number);
            if (IconImageDictionary.ContainsKey(IconKey))
            {
                return IconImageDictionary[IconKey];
            }
            IntPtr large = IntPtr.Zero;
            IntPtr small = IntPtr.Zero;
            int hSuccess = Shell32.ExtractIconEx(path, number, out large, out small, 1);
            if (hSuccess == 0)
            {
                return null;
            }
            else
            {

                Icon appIcon = Icon.FromHandle(large);
                ImageSource IconImage = IconToImageSource(appIcon);
                IconImageDictionary.Add(IconKey, IconImage);
                return IconImage;
            }
        }
        static private string CreateIconKeyInfo(string path, int number)
        {
            return path + ',' + number.ToString();
        }

        static private Dictionary<int, ImageSource> SystemIconImageDictionary = new Dictionary<int, ImageSource>();
        static private Dictionary<string, ImageSource> IconImageDictionary = new Dictionary<string, ImageSource>();


        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeleteObject([In] IntPtr hObject);
        static private ImageSource IconToImageSource(Icon ico)
        {
            var handle = ico.Handle;
            try
            {
                return Imaging.CreateBitmapSourceFromHIcon(handle, Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            }
            finally { DeleteObject(handle); }
        }

        
    }
}
