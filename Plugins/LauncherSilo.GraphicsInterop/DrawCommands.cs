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
    public abstract class DrawCommand2D : IDrawCommand
    {
        public IGraphics2DContext Context2D;
        public abstract void Draw();
    }

    public class Clear2DCommand : DrawCommand2D
    {
        public RawColor4? clearColor { get; set; } = null;
        public override void Draw()
        {
            Context2D.Clear(clearColor);
        }
    }
    public class DrawBitmap2DCommand : DrawCommand2D
    {
        public Bitmap bitmap { get; set; }
        public RawRectangleF? destinationRectangle { get; set; }
        public float opacity { get; set; }
        public BitmapInterpolationMode interpolationMode { get; set; }
        public RawRectangleF? sourceRectangle { get; set; }
        public override void Draw()
        {
            Context2D.DrawBitmap(bitmap, destinationRectangle, opacity, interpolationMode, sourceRectangle);
        }
    }
    public class DrawEllipse2DCommand : DrawCommand2D
    {
        public Ellipse ellipse { get; set; }
        public Brush brush { get; set; }
        public float strokeWidth { get; set; }
        public StrokeStyle strokeStyle { get; set; }
        public override void Draw()
        {
            Context2D.DrawEllipse(ellipse, brush, strokeWidth, strokeStyle);
        }
    }
    public class DrawGeometry2DCommand : DrawCommand2D
    {
        public Geometry geometry { get; set; }
        public Brush brush { get; set; }
        public float strokeWidth { get; set; }
        public StrokeStyle strokeStyle { get; set; }
        public override void Draw()
        {
            Context2D.DrawGeometry(geometry, brush, strokeWidth, strokeStyle);
        }
    }
    public class DrawGlyphRun2DCommand : DrawCommand2D
    {
        public RawVector2 baselineOrigin { get; set; }
        public GlyphRun glyphRun { get; set; }
        public Brush foregroundBrush { get; set; }
        public MeasuringMode measuringMode { get; set; }
        public override void Draw()
        {
            Context2D.DrawGlyphRun(baselineOrigin, glyphRun, foregroundBrush, measuringMode);
        }
    }
    public class DrawLine2DCommand : DrawCommand2D
    {
        public RawVector2 point0 { get; set; }
        public RawVector2 point1 { get; set; }
        public Brush brush { get; set; }
        public float strokeWidth { get; set; }
        public StrokeStyle strokeStyle { get; set; }
        public override void Draw()
        {
            Context2D.DrawLine(point0, point1, brush, strokeWidth, strokeStyle);
        }
    }
    public class DrawRectangle2DCommand : DrawCommand2D
    {
        public RawRectangleF rect { get; set; }
        public Brush brush { get; set; }
        public float strokeWidth { get; set; }
        public StrokeStyle strokeStyle { get; set; }
        public override void Draw()
        {
            Context2D.DrawRectangle(rect, brush, strokeWidth, strokeStyle);
        }
    }
    public class DrawRoundedRectangle2DCommand : DrawCommand2D
    {
        public RoundedRectangle roundedRect { get; set; }
        public Brush brush { get; set; }
        public float strokeWidth { get; set; }
        public StrokeStyle strokeStyle { get; set; }
        public override void Draw()
        {
            Context2D.DrawRoundedRectangle(roundedRect, brush, strokeWidth, strokeStyle);
        }
    }
    public class DrawText2DCommand : DrawCommand2D
    {
        public string text { get; set; }
        public int stringLength { get; set; }
        public TextFormat textFormat { get; set; }
        public RawRectangleF layoutRect { get; set; }
        public Brush defaultFillBrush { get; set; }
        public DrawTextOptions options { get; set; }
        public MeasuringMode measuringMode { get; set; }
        public override void Draw()
        {
            Context2D.DrawText(text, stringLength, textFormat, layoutRect, defaultFillBrush, options, measuringMode);
        }
    }
    public class DrawTextLayout2DCommand : DrawCommand2D
    {
        public RawVector2 origin { get; set; }
        public TextLayout textLayout { get; set; }
        public Brush defaultFillBrush { get; set; }
        public DrawTextOptions options { get; set; }
        public override void Draw()
        {
            Context2D.DrawTextLayout(origin, textLayout, defaultFillBrush, options);
        }
    }
    public class FillEllipse2DCommand : DrawCommand2D
    {
        public Ellipse ellipse { get; set; }
        public Brush brush { get; set; }
        public override void Draw()
        {
            Context2D.FillEllipse(ellipse, brush);
        }
    }
    public class FillGeometry2DCommand : DrawCommand2D
    {
        public Geometry geometry { get; set; }
        public Brush brush { get; set; }
        public Brush opacityBrush { get; set; }
        public override void Draw()
        {
            Context2D.FillGeometry(geometry, brush, opacityBrush);
        }
    }
    public class FillMesh2DCommand : DrawCommand2D
    {
        public Mesh mesh { get; set; }
        public Brush brush { get; set; }
        public override void Draw()
        {
            Context2D.FillMesh(mesh, brush);
        }
    }
    public class FillOpacityMask2DCommand : DrawCommand2D
    {
        public Bitmap opacityMask { get; set; }
        public Brush brush { get; set; }
        public OpacityMaskContent content { get; set; }
        public RawRectangleF? destinationRectangle { get; set; }
        public RawRectangleF? sourceRectangle { get; set; }
        public override void Draw()
        {
            Context2D.FillOpacityMask(opacityMask, brush, content, destinationRectangle, sourceRectangle);
        }
    }
    public class FillRectangle2DCommand : DrawCommand2D
    {
        public RawRectangleF rect { get; set; }
        public Brush brush { get; set; }

        public override void Draw()
        {
            Context2D.FillRectangle(rect, brush);
        }
    }
    public class FillRoundedRectangle2DCommand : DrawCommand2D
    {
        public RoundedRectangle roundedRect { get; set; }
        public Brush brush { get; set; }
        public override void Draw()
        {
            Context2D.FillRoundedRectangle(roundedRect, brush);
        }
    }
}
