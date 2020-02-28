using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace LauncherSilo.GraphicsInterop.WPF
{
    public class GraphicsInteropEllipse : GraphicsInteropFillableElement
    {
        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register("Center", typeof(Point), typeof(GraphicsInteropEllipse), new PropertyMetadata(new Point(0, 0), OtherPropertyChanged));
        public Point Center
        {
            get { return (Point)GetValue(CenterProperty); }
            set { SetValue(CenterProperty, value); }
        }
        public static readonly DependencyProperty RadiusXProperty = DependencyProperty.Register("RadiusX", typeof(double), typeof(GraphicsInteropEllipse), new PropertyMetadata(0.0, OtherPropertyChanged));
        public double RadiusX
        {
            get { return (double)GetValue(RadiusXProperty); }
            set { SetValue(RadiusXProperty, value); }
        }
        public static readonly DependencyProperty RadiusYProperty = DependencyProperty.Register("RadiusY", typeof(double), typeof(GraphicsInteropEllipse), new PropertyMetadata(0.0, OtherPropertyChanged));
        public double RadiusY
        {
            get { return (double)GetValue(RadiusYProperty); }
            set { SetValue(RadiusYProperty, value); }
        }

        private DrawEllipse2DCommand _drawEllipse = null;
        private FillEllipse2DCommand _fillEllipse = null;
        public override void OnPrepareRender(RenderFrame renderFrame)
        {
            if (_drawEllipse == null)
            {
                _drawEllipse = renderFrame.CreateDrawCommnad2D<DrawEllipse2DCommand>();
                _isStrokeBrushChanged = true;
                _isStrokeWidthChanged = true;
                _isStrokeStyleChanged = true;
                _isOtherPropertyChanged = true;
            }
            if (_isStrokeStyleChanged)
            {
                SharpDX.Direct2D1.StrokeStyleProperties NewStrokeStyleProperties = new SharpDX.Direct2D1.StrokeStyleProperties()
                {
                    StartCap = StrokeStartLineCap.ToNativeCapStyle(),
                    EndCap = StrokeEndLineCap.ToNativeCapStyle(),
                    DashCap = StrokeDashCap.ToNativeCapStyle(),
                    DashStyle = StrokeDashStyle.ToNativeDashStyle(),
                    DashOffset = (float)StrokeDashOffset,
                    LineJoin = StrokeLineJoin.ToNativeLineJoin(),
                    MiterLimit = (float)StrokeMiterLimit,
                };
                _drawEllipse.strokeStyle = renderFrame.Controller.CreateStrokeStyle(NewStrokeStyleProperties);
                _isStrokeStyleChanged = false;
            }
            if (_isStrokeBrushChanged)
            {
                _drawEllipse.brush = StrokeBrush.ToNativeBrush(renderFrame);
                _isStrokeBrushChanged = false;
            }
            if (_isStrokeWidthChanged)
            {
                _drawEllipse.strokeWidth = (float)StrokeWidth;
                _isStrokeWidthChanged = false;
            }
            if (_fillEllipse == null && FillBrush != null)
            {
                _fillEllipse = renderFrame.CreateDrawCommnad2D<FillEllipse2DCommand>();
                _isFillBrushChanged = true;
            }
            if (_isFillBrushChanged)
            {
                if (_fillEllipse != null)
                {
                    if (FillBrush != null)
                    {
                        _fillEllipse.brush = FillBrush.ToNativeBrush(renderFrame);
                    }
                    else
                    {
                        _fillEllipse = null;
                    }
                }
                _isFillBrushChanged = false;
            }
            if (_isOtherPropertyChanged)
            {
                _drawEllipse.ellipse = new SharpDX.Direct2D1.Ellipse(Center.ToNativeVector2(), (float)RadiusX, (float)RadiusY);
                if (_fillEllipse != null)
                {
                    _fillEllipse.ellipse = _drawEllipse.ellipse;
                }
                _isOtherPropertyChanged = false;
            }
        }
        public override void OnRender(RenderFrame renderFrame)
        {
            if (_fillEllipse != null)
            {
                renderFrame.PushDrawCommand(_fillEllipse);
            }
            renderFrame.PushDrawCommand(_drawEllipse);
        }

        private bool _isOtherPropertyChanged = false;
        private static void OtherPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GraphicsInteropEllipse element)
            {
                element._isOtherPropertyChanged = true;
            }
        }
    }
}
