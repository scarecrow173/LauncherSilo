using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LauncherSilo.GraphicsInterop
{
    public static class Utils
    {
        public static void SafeDispose<T>(ref T Disposable) where T : class
        {
            if (Disposable == null)
            {
                return;
            }
            var disposer = Disposable as IDisposable;
            disposer?.Dispose();
            Disposable = null;
        }
    }
}
