using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LauncherSilo.GraphicsInterop.WPF
{
    public static class Interop
    {
        public static SharpDX.Color MediaColorToNativeColor(System.Windows.Media.Color from)
        {
            return new SharpDX.Color(from.R, from.G, from.B, from.A);
        }
        public static System.Windows.Media.Color NativeColorToMediaColor(SharpDX.Color from)
        {
            return System.Windows.Media.Color.FromArgb(from.A, from.R, from.G, from.B);
        }
        public static System.Windows.Shapes.Ellipse NativeEllipseToShapesEllipse(SharpDX.Direct2D1.Ellipse from)
        {
            return new System.Windows.Shapes.Ellipse() { Height = from.RadiusY, Width = from.RadiusX };
        }
        public static SharpDX.Direct2D1.Ellipse ShapesEllipseToNativeEllipse(System.Windows.Shapes.Ellipse from)
        {
            return new SharpDX.Direct2D1.Ellipse(new SharpDX.Mathematics.Interop.RawVector2(0,0), (float)(from.Width * 0.5), (float)(from.Height * 0.5));
        }
        public static System.Windows.Shapes.Rectangle NativeRectangleToShapesRectangle(SharpDX.Rectangle from)
        {
            return new System.Windows.Shapes.Rectangle() { Height = from.Height, Width = from.Width };
        }
        public static SharpDX.Rectangle ShapesRectangleToNativeRectangle(System.Windows.Shapes.Rectangle from)
        {
            return new SharpDX.Rectangle(0, 0, (int)from.Width, (int)from.Height);
        }
        public static System.Windows.Shapes.Rectangle NativeRoundedRectangleToShapesRectangle(SharpDX.Direct2D1.RoundedRectangle from)
        {
            return new System.Windows.Shapes.Rectangle() { Height = from.Rect.Bottom - from.Rect.Top, Width = from.Rect.Right - from.Rect.Left, RadiusX = from.RadiusX, RadiusY = from.RadiusY };
        }
        public static SharpDX.Direct2D1.RoundedRectangle ShapesRectangleToNativeRoundedRectangle(System.Windows.Shapes.Rectangle from)
        {
            return new SharpDX.Direct2D1.RoundedRectangle() { Rect = new SharpDX.Mathematics.Interop.RawRectangleF(0, 0, (float)from.Width, (float)from.Height), RadiusX = (float)from.RadiusX, RadiusY = (float)from.RadiusY };
        }

    }
}
