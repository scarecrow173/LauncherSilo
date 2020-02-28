using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace LauncherSilo.GraphicsInterop.WPF
{
    public class GraphicsInteropLine : GraphicsInteropPrimitiveElement
    {
        public static readonly DependencyProperty Point0Property = DependencyProperty.Register("Point0", typeof(Point), typeof(GraphicsInteropLine), new PropertyMetadata(new Point(0,0), OtherPropertyChanged));
        public Point Point0
        {
            get { return (Point)GetValue(Point0Property); }
            set { SetValue(Point0Property, value); }
        }

        public static readonly DependencyProperty Point1Property = DependencyProperty.Register("Point1", typeof(Point), typeof(GraphicsInteropLine), new PropertyMetadata(new Point(0, 0), OtherPropertyChanged));
        public Point Point1
        {
            get { return (Point)GetValue(Point1Property); }
            set { SetValue(Point1Property, value); }
        }

        private DrawLine2DCommand _drawLine = null;
        private bool _isOtherPropertyChanged = false;
        public override void OnPrepareRender(RenderFrame renderFrame)
        {
            if (_drawLine == null)
            {
                _drawLine = renderFrame.CreateDrawCommnad2D<DrawLine2DCommand>();
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
                _drawLine.strokeStyle = renderFrame.Controller.CreateStrokeStyle(NewStrokeStyleProperties);
                _isStrokeStyleChanged = false;
            }
            if (_isStrokeBrushChanged)
            {
                _drawLine.brush = StrokeBrush.ToNativeBrush(renderFrame);
                _isStrokeBrushChanged = false;
            }
            if (_isStrokeWidthChanged)
            {
                _drawLine.strokeWidth = (float)StrokeWidth;
                _isStrokeWidthChanged = false;
            }
            if (_isOtherPropertyChanged)
            {
                _drawLine.point0 = Point0.ToNativeVector2();
                _drawLine.point1 = Point1.ToNativeVector2();
                _drawLine.strokeWidth = (float)StrokeWidth;
                _isOtherPropertyChanged = false;
            }
        }
        public override void OnRender(RenderFrame renderFrame)
        {
            renderFrame.PushDrawCommand(_drawLine);
        }

        private static void OtherPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GraphicsInteropLine element)
            {
                element._isOtherPropertyChanged = true;
            }
        }
    }
}
