using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LauncherSilo.GraphicsInterop.WPF
{
    public static class Interop
    {
        public static SharpDX.Vector2 ToNativeVector2(this System.Windows.Point vector)
        {
            return new SharpDX.Vector2((float)vector.X, (float)vector.Y);
        }

        public static System.Windows.Point ToPoint(this SharpDX.Vector2 vector)
        {
            return new System.Windows.Point(vector.X, vector.Y);
        }

        public static SharpDX.Color4 ToNativeColor4(this System.Windows.Media.Color color)
        {
            return new SharpDX.Color4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
        }
        public static System.Windows.Media.Color ToMediaColor(this SharpDX.Color4 color)
        {
            return System.Windows.Media.Color.FromArgb((byte)(color.Alpha * 255), (byte)(color.Red * 255), (byte)(color.Green * 255), (byte)(color.Blue * 255));
        }
        public static SharpDX.Color ToNativeColor(this System.Windows.Media.Color from)
        {
            return new SharpDX.Color(from.R, from.G, from.B, from.A);
        }
        public static System.Windows.Media.Color ToMediaColor(this SharpDX.Color from)
        {
            return System.Windows.Media.Color.FromArgb(from.A, from.R, from.G, from.B);
        }
        public static System.Windows.Shapes.Ellipse ToShapesEllipse(this SharpDX.Direct2D1.Ellipse from)
        {
            return new System.Windows.Shapes.Ellipse() { Height = from.RadiusY, Width = from.RadiusX };
        }
        public static SharpDX.Direct2D1.Ellipse ToNativeEllipse(this System.Windows.Shapes.Ellipse from)
        {
            return new SharpDX.Direct2D1.Ellipse(new SharpDX.Mathematics.Interop.RawVector2(0,0), (float)(from.Width * 0.5), (float)(from.Height * 0.5));
        }
        public static System.Windows.Shapes.Rectangle ToShapesRectangle(this SharpDX.Rectangle from)
        {
            return new System.Windows.Shapes.Rectangle() { Height = from.Height, Width = from.Width };
        }
        public static SharpDX.Rectangle ToNativeRectangle(this System.Windows.Shapes.Rectangle from)
        {
            return new SharpDX.Rectangle(0, 0, (int)from.Width, (int)from.Height);
        }
        public static System.Windows.Shapes.Rectangle ToShapesRectangle(this SharpDX.Direct2D1.RoundedRectangle from)
        {
            return new System.Windows.Shapes.Rectangle() { Height = from.Rect.Bottom - from.Rect.Top, Width = from.Rect.Right - from.Rect.Left, RadiusX = from.RadiusX, RadiusY = from.RadiusY };
        }
        public static SharpDX.Direct2D1.RoundedRectangle ToNativeRoundedRectangle(this System.Windows.Shapes.Rectangle from)
        {
            return new SharpDX.Direct2D1.RoundedRectangle() { Rect = new SharpDX.Mathematics.Interop.RawRectangleF(0, 0, (float)from.Width, (float)from.Height), RadiusX = (float)from.RadiusX, RadiusY = (float)from.RadiusY };
        }

        public static SharpDX.Direct2D1.ExtendMode ToNativeExtendMode(this System.Windows.Media.GradientSpreadMethod from)
        {
            switch (from)
            {
                case System.Windows.Media.GradientSpreadMethod.Pad:
                    return SharpDX.Direct2D1.ExtendMode.Clamp;
                case System.Windows.Media.GradientSpreadMethod.Reflect:
                    return SharpDX.Direct2D1.ExtendMode.Mirror;
                case System.Windows.Media.GradientSpreadMethod.Repeat:
                    return SharpDX.Direct2D1.ExtendMode.Wrap;
                default:
                    return SharpDX.Direct2D1.ExtendMode.Wrap;
            }
        }

        public static SharpDX.Direct2D1.Gamma ToNativeColorInterpolationMode(this System.Windows.Media.ColorInterpolationMode from)
        {
            switch (from)
            {
                case System.Windows.Media.ColorInterpolationMode.ScRgbLinearInterpolation:
                    return SharpDX.Direct2D1.Gamma.Linear;
                case System.Windows.Media.ColorInterpolationMode.SRgbLinearInterpolation:
                    return SharpDX.Direct2D1.Gamma.StandardRgb;
                default:
                    return SharpDX.Direct2D1.Gamma.Linear;
            }
        }
        public static SharpDX.Direct2D1.Brush ToNativeBrush(this System.Windows.Media.Brush from, RenderFrame target)
        {
            return ToNativeBrush(from, target.Controller);
        }
        public static SharpDX.Direct2D1.Brush ToNativeBrush(this System.Windows.Media.Brush from, GraphicsController target)
        {
            return ToNativeBrush(from, target.GetRenderTarget());
        }

        public static SharpDX.Direct2D1.Brush ToNativeBrush(this System.Windows.Media.Brush from, SharpDX.Direct2D1.RenderTarget target)
        {
            if (from is System.Windows.Media.SolidColorBrush solid)
            {
                return new global::SharpDX.Direct2D1.SolidColorBrush(target, solid.Color.ToNativeColor4());
            }
            else if (from is System.Windows.Media.LinearGradientBrush linear)
            {
                return new SharpDX.Direct2D1.LinearGradientBrush(target,
                    new SharpDX.Direct2D1.LinearGradientBrushProperties() { StartPoint = linear.StartPoint.ToNativeVector2(), EndPoint = linear.EndPoint.ToNativeVector2() },
                    new SharpDX.Direct2D1.GradientStopCollection
                    (
                        target,
                        linear.GradientStops.Select(x => new SharpDX.Direct2D1.GradientStop() { Color = x.Color.ToNativeColor4(), Position = (float)x.Offset }).ToArray(),
                        linear.ColorInterpolationMode.ToNativeColorInterpolationMode(),
                        linear.SpreadMethod.ToNativeExtendMode()
                    )
                    );
            }
            else if (from is System.Windows.Media.RadialGradientBrush radial)
            {
                return new SharpDX.Direct2D1.RadialGradientBrush(target,
                    new SharpDX.Direct2D1.RadialGradientBrushProperties()
                    {
                        Center = radial.Center.ToNativeVector2(),
                        GradientOriginOffset = radial.GradientOrigin.ToNativeVector2(),
                        RadiusX = (float)radial.RadiusX,
                        RadiusY = (float)radial.RadiusY
                    },
                    new SharpDX.Direct2D1.GradientStopCollection
                    (
                        target,
                        radial.GradientStops.Select(x => new SharpDX.Direct2D1.GradientStop() { Color = x.Color.ToNativeColor4(), Position = (float)x.Offset }).ToArray(),
                        radial.ColorInterpolationMode.ToNativeColorInterpolationMode(),
                        radial.SpreadMethod.ToNativeExtendMode()
                    ));
            }
            else
            {
                throw new NotImplementedException("Brush does not support yet.");
            }
        }

        public static SharpDX.Direct2D1.CapStyle ToNativeCapStyle(this System.Windows.Media.PenLineCap from)
        {
            switch (from)
            {
                case System.Windows.Media.PenLineCap.Flat:
                    return SharpDX.Direct2D1.CapStyle.Flat;
                case System.Windows.Media.PenLineCap.Round:
                    return SharpDX.Direct2D1.CapStyle.Round;
                case System.Windows.Media.PenLineCap.Square:
                    return SharpDX.Direct2D1.CapStyle.Square;
                case System.Windows.Media.PenLineCap.Triangle:
                    return SharpDX.Direct2D1.CapStyle.Triangle;
                default:
                    return SharpDX.Direct2D1.CapStyle.Flat;
            }
        }

        public static SharpDX.Direct2D1.LineJoin ToNativeLineJoin(this System.Windows.Media.PenLineJoin from)
        {
            switch (from)
            {
                case System.Windows.Media.PenLineJoin.Bevel:
                    return SharpDX.Direct2D1.LineJoin.Bevel;
                case System.Windows.Media.PenLineJoin.Miter:
                    return SharpDX.Direct2D1.LineJoin.Miter;
                case System.Windows.Media.PenLineJoin.Round:
                    return SharpDX.Direct2D1.LineJoin.Round;
                default:
                    return SharpDX.Direct2D1.LineJoin.Bevel;
            }
        }
        public static SharpDX.Direct2D1.DashStyle ToNativeDashStyle(this System.Windows.Media.DashStyle from)
        {
            if (from == System.Windows.Media.DashStyles.Dash)
            {
                return SharpDX.Direct2D1.DashStyle.Dash;
            }
            else if (from == System.Windows.Media.DashStyles.DashDot)
            {
                return SharpDX.Direct2D1.DashStyle.DashDot;
            }
            else if (from == System.Windows.Media.DashStyles.DashDotDot)
            {
                return SharpDX.Direct2D1.DashStyle.DashDotDot;
            }
            else if (from == System.Windows.Media.DashStyles.Dot)
            {
                return SharpDX.Direct2D1.DashStyle.Dot;
            }
            else
            {
                return SharpDX.Direct2D1.DashStyle.Solid;
            }
        }
    }
}
