using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;

namespace LauncherSilo.GraphicsInterop
{
    public interface IGraphics2DContext
    {
        void BeginDraw();
        void Clear(RawColor4? clearColor);
        void DrawBitmap(Bitmap bitmap, float opacity, BitmapInterpolationMode interpolationMode, RawRectangleF sourceRectangle);
        void DrawBitmap(Bitmap bitmap, RawRectangleF? destinationRectangle, float opacity, BitmapInterpolationMode interpolationMode, RawRectangleF? sourceRectangle);
        void DrawBitmap(Bitmap bitmap, float opacity, BitmapInterpolationMode interpolationMode);
        void DrawBitmap(Bitmap bitmap, RawRectangleF destinationRectangle, float opacity, BitmapInterpolationMode interpolationMode);
        void DrawEllipse(Ellipse ellipse, Brush brush);
        void DrawEllipse(Ellipse ellipse, Brush brush, float strokeWidth);
        void DrawEllipse(Ellipse ellipse, Brush brush, float strokeWidth, StrokeStyle strokeStyle);
        void DrawGeometry(Geometry geometry, Brush brush);
        void DrawGeometry(Geometry geometry, Brush brush, float strokeWidth);
        void DrawGeometry(Geometry geometry, Brush brush, float strokeWidth, StrokeStyle strokeStyle);
        void DrawGlyphRun(RawVector2 baselineOrigin, GlyphRun glyphRun, Brush foregroundBrush, MeasuringMode measuringMode);
        void DrawLine(RawVector2 point0, RawVector2 point1, Brush brush);
        void DrawLine(RawVector2 point0, RawVector2 point1, Brush brush, float strokeWidth);
        void DrawLine(RawVector2 point0, RawVector2 point1, Brush brush, float strokeWidth, StrokeStyle strokeStyle);
        void DrawRectangle(RawRectangleF rect, Brush brush);
        void DrawRectangle(RawRectangleF rect, Brush brush, float strokeWidth);
        void DrawRectangle(RawRectangleF rect, Brush brush, float strokeWidth, StrokeStyle strokeStyle);
        void DrawRoundedRectangle(RoundedRectangle roundedRect, Brush brush);
        void DrawRoundedRectangle(RoundedRectangle roundedRect, Brush brush, float strokeWidth);
        void DrawRoundedRectangle(RoundedRectangle roundedRect, Brush brush, float strokeWidth, StrokeStyle strokeStyle);
        void DrawRoundedRectangle(ref RoundedRectangle roundedRect, Brush brush, float strokeWidth, StrokeStyle strokeStyle);
        void DrawText(string text, int stringLength, TextFormat textFormat, RawRectangleF layoutRect, Brush defaultFillBrush, DrawTextOptions options, MeasuringMode measuringMode);
        void DrawText(string text, TextFormat textFormat, RawRectangleF layoutRect, Brush defaultForegroundBrush);
        void DrawText(string text, TextFormat textFormat, RawRectangleF layoutRect, Brush defaultForegroundBrush, DrawTextOptions options);
        void DrawText(string text, TextFormat textFormat, RawRectangleF layoutRect, Brush defaultForegroundBrush, DrawTextOptions options, MeasuringMode measuringMode);
        void DrawTextLayout(RawVector2 origin, TextLayout textLayout, Brush defaultFillBrush, DrawTextOptions options);
        void DrawTextLayout(RawVector2 origin, TextLayout textLayout, Brush defaultForegroundBrush);
        void EndDraw();
        void FillEllipse(Ellipse ellipse, Brush brush);
        void FillGeometry(Geometry geometry, Brush brush, Brush opacityBrush);
        void FillGeometry(Geometry geometry, Brush brush);
        void FillMesh(Mesh mesh, Brush brush);
        void FillOpacityMask(Bitmap opacityMask, Brush brush, OpacityMaskContent content);
        void FillOpacityMask(Bitmap opacityMask, Brush brush, OpacityMaskContent content, RawRectangleF? destinationRectangle, RawRectangleF? sourceRectangle);
        void FillRectangle(RawRectangleF rect, Brush brush);
        void FillRoundedRectangle(ref RoundedRectangle roundedRect, Brush brush);
        void FillRoundedRectangle(RoundedRectangle roundedRect, Brush brush);
    }
}
