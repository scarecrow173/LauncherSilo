using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct2D1;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;

namespace LauncherSilo.GraphicsInterop
{
    public class  GraphicsController : IDisposable, IGraphics2DContext
    {
        private SharpDX.Direct3D11.Device _dx11Device = null;
        private RenderTarget _renderTarget = null;

        public GraphicsController()
        {
            _dx11Device = new SharpDX.Direct3D11.Device(DriverType.Hardware, DeviceCreationFlags.BgraSupport);
        }
        ~GraphicsController()
        {
            Dispose(false);
        }
        public SharpDX.DXGI.SwapChain CreateSwapChain(SharpDX.DXGI.SwapChainDescription desc)
        {
            using (var factory = new SharpDX.DXGI.Factory1())
            {
                return new SharpDX.DXGI.SwapChain(factory, _dx11Device, desc);
            }
        }
        public Texture1D CreateTexture1D(Texture1DDescription desc)
        {
            return new Texture1D(_dx11Device, desc);
        }
        public Texture2D CreateTexture2D(Texture2DDescription desc)
        {
            return new Texture2D(_dx11Device, desc);
        }
        public Texture3D CreateTexture3D(Texture3DDescription desc)
        {
            return new Texture3D(_dx11Device, desc);
        }
        public void SetScissorRectangle(int left, int top, int right, int bottom)
        {
            _dx11Device?.ImmediateContext.Rasterizer.SetScissorRectangle(left, top, right, bottom);
        }
        public void SetViewport(float x, float y, float width, float height, float minZ = 0, float maxZ = 1)
        {
            _dx11Device?.ImmediateContext.Rasterizer.SetViewport(x, y, width, height, minZ, maxZ);
        }
        public void SetViewport(RawViewportF viewport)
        {
            _dx11Device?.ImmediateContext.Rasterizer.SetViewport(viewport);
        }
        public void SetViewports(RawViewportF[] viewports, int count = 0)
        {
            _dx11Device?.ImmediateContext.Rasterizer.SetViewports(viewports, count);
        }
        public unsafe void SetViewports(RawViewportF* viewports, int count = 0)
        {
            _dx11Device?.ImmediateContext.Rasterizer.SetViewports(viewports, count);
        }
        public void SetRenderTarget(RenderTarget renderTarget)
        {
            _renderTarget = renderTarget;
        }
        public RenderTarget GetRenderTarget()
        {
            return _renderTarget;
        }
        public void ContextFlush()
        {
            _dx11Device?.ImmediateContext?.Flush();
        }
        public SharpDX.Direct2D1.SolidColorBrush CreateSolidColorBrush(SharpDX.Mathematics.Interop.RawColor4 color)
        {
            return new SolidColorBrush(_renderTarget, color);
        }
        public SharpDX.Direct2D1.StrokeStyle CreateStrokeStyle(StrokeStyleProperties props)
        {
            return new SharpDX.Direct2D1.StrokeStyle(_renderTarget.Factory, props);
        }

        public void BeginDraw() => _renderTarget?.BeginDraw();
        public void Clear(RawColor4? clearColor) => _renderTarget?.Clear(clearColor);
        public void DrawBitmap(Bitmap bitmap, float opacity, BitmapInterpolationMode interpolationMode, RawRectangleF sourceRectangle) => _renderTarget?.DrawBitmap(bitmap, opacity, interpolationMode, sourceRectangle);
        public void DrawBitmap(Bitmap bitmap, RawRectangleF? destinationRectangle, float opacity, BitmapInterpolationMode interpolationMode, RawRectangleF? sourceRectangle) => _renderTarget?.DrawBitmap(bitmap, destinationRectangle, opacity, interpolationMode, sourceRectangle);
        public void DrawBitmap(Bitmap bitmap, float opacity, BitmapInterpolationMode interpolationMode) => _renderTarget?.DrawBitmap(bitmap, opacity, interpolationMode);
        public void DrawBitmap(Bitmap bitmap, RawRectangleF destinationRectangle, float opacity, BitmapInterpolationMode interpolationMode) => _renderTarget?.DrawBitmap(bitmap, destinationRectangle, opacity, interpolationMode);
        public void DrawEllipse(Ellipse ellipse, Brush brush) => _renderTarget?.DrawEllipse(ellipse, brush);
        public void DrawEllipse(Ellipse ellipse, Brush brush, float strokeWidth) => _renderTarget?.DrawEllipse(ellipse, brush, strokeWidth);
        public void DrawEllipse(Ellipse ellipse, Brush brush, float strokeWidth, StrokeStyle strokeStyle) => _renderTarget?.DrawEllipse(ellipse, brush, strokeWidth, strokeStyle);
        public void DrawGeometry(Geometry geometry, Brush brush) => _renderTarget?.DrawGeometry(geometry, brush);
        public void DrawGeometry(Geometry geometry, Brush brush, float strokeWidth) => _renderTarget?.DrawGeometry(geometry, brush, strokeWidth);
        public void DrawGeometry(Geometry geometry, Brush brush, float strokeWidth, StrokeStyle strokeStyle) => _renderTarget?.DrawGeometry(geometry, brush, strokeWidth, strokeStyle);
        public void DrawGlyphRun(RawVector2 baselineOrigin, GlyphRun glyphRun, Brush foregroundBrush, MeasuringMode measuringMode) => _renderTarget?.DrawGlyphRun(baselineOrigin, glyphRun, foregroundBrush, measuringMode);
        public void DrawLine(RawVector2 point0, RawVector2 point1, Brush brush) => _renderTarget?.DrawLine(point0, point1, brush);
        public void DrawLine(RawVector2 point0, RawVector2 point1, Brush brush, float strokeWidth) => _renderTarget?.DrawLine(point0, point1, brush, strokeWidth);
        public void DrawLine(RawVector2 point0, RawVector2 point1, Brush brush, float strokeWidth, StrokeStyle strokeStyle) => _renderTarget?.DrawLine(point0, point1, brush, strokeWidth, strokeStyle);
        public void DrawRectangle(RawRectangleF rect, Brush brush) => _renderTarget?.DrawRectangle(rect, brush);
        public void DrawRectangle(RawRectangleF rect, Brush brush, float strokeWidth) => _renderTarget?.DrawRectangle(rect, brush, strokeWidth);
        public void DrawRectangle(RawRectangleF rect, Brush brush, float strokeWidth, StrokeStyle strokeStyle) => _renderTarget?.DrawRectangle(rect, brush, strokeWidth, strokeStyle);
        public void DrawRoundedRectangle(RoundedRectangle roundedRect, Brush brush) => _renderTarget?.DrawRoundedRectangle(roundedRect, brush);
        public void DrawRoundedRectangle(RoundedRectangle roundedRect, Brush brush, float strokeWidth) => _renderTarget?.DrawRoundedRectangle(roundedRect, brush, strokeWidth);
        public void DrawRoundedRectangle(RoundedRectangle roundedRect, Brush brush, float strokeWidth, StrokeStyle strokeStyle) => _renderTarget?.DrawRoundedRectangle(roundedRect, brush, strokeWidth, strokeStyle);
        public void DrawRoundedRectangle(ref RoundedRectangle roundedRect, Brush brush, float strokeWidth, StrokeStyle strokeStyle) => _renderTarget?.DrawRoundedRectangle(ref roundedRect, brush, strokeWidth, strokeStyle);
        public void DrawText(string text, int stringLength, TextFormat textFormat, RawRectangleF layoutRect, Brush defaultFillBrush, DrawTextOptions options, MeasuringMode measuringMode) => _renderTarget?.DrawText(text, stringLength, textFormat, layoutRect, defaultFillBrush, options, measuringMode);
        public void DrawText(string text, TextFormat textFormat, RawRectangleF layoutRect, Brush defaultForegroundBrush) => _renderTarget?.DrawText(text, textFormat, layoutRect, defaultForegroundBrush);
        public void DrawText(string text, TextFormat textFormat, RawRectangleF layoutRect, Brush defaultForegroundBrush, DrawTextOptions options) => _renderTarget?.DrawText(text, textFormat, layoutRect, defaultForegroundBrush, options);
        public void DrawText(string text, TextFormat textFormat, RawRectangleF layoutRect, Brush defaultForegroundBrush, DrawTextOptions options, MeasuringMode measuringMode) => _renderTarget?.DrawText(text, textFormat, layoutRect, defaultForegroundBrush, options, measuringMode);
        public void DrawTextLayout(RawVector2 origin, TextLayout textLayout, Brush defaultFillBrush, DrawTextOptions options) => _renderTarget?.DrawTextLayout(origin, textLayout, defaultFillBrush, options);
        public void DrawTextLayout(RawVector2 origin, TextLayout textLayout, Brush defaultForegroundBrush) => _renderTarget?.DrawTextLayout(origin, textLayout, defaultForegroundBrush);
        public void EndDraw() => _renderTarget?.EndDraw();
        public void FillEllipse(Ellipse ellipse, Brush brush) => _renderTarget?.FillEllipse(ellipse, brush);
        public void FillGeometry(Geometry geometry, Brush brush, Brush opacityBrush) => _renderTarget?.FillGeometry(geometry, brush, opacityBrush);
        public void FillGeometry(Geometry geometry, Brush brush) => _renderTarget?.FillGeometry(geometry, brush);
        public void FillMesh(Mesh mesh, Brush brush) => _renderTarget?.FillMesh(mesh, brush);
        public void FillOpacityMask(Bitmap opacityMask, Brush brush, OpacityMaskContent content) => _renderTarget?.FillOpacityMask(opacityMask, brush, content);
        public void FillOpacityMask(Bitmap opacityMask, Brush brush, OpacityMaskContent content, RawRectangleF? destinationRectangle, RawRectangleF? sourceRectangle) => _renderTarget?.FillOpacityMask(opacityMask, brush, content, destinationRectangle, sourceRectangle);
        public void FillRectangle(RawRectangleF rect, Brush brush) => _renderTarget?.FillRectangle(rect, brush);
        public void FillRoundedRectangle(ref RoundedRectangle roundedRect, Brush brush) => _renderTarget?.FillRoundedRectangle(ref roundedRect, brush);
        public void FillRoundedRectangle(RoundedRectangle roundedRect, Brush brush) => _renderTarget?.FillRoundedRectangle(roundedRect, brush);
        public void Flush() => _renderTarget?.Flush();

        public bool IsDisposed { get; private set; }
        public void Dispose()
        {
            if (IsDisposed) return;
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            Utils.SafeDispose(ref _dx11Device);
            IsDisposed = true;
        }


    }
}
