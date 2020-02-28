using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace LauncherSilo.GraphicsInterop.WPF
{
    public class GraphicsInteropLine : GraphicsInteropElement
    {
        public static readonly DependencyProperty LineBrushProperty = DependencyProperty.Register("LineBrush", typeof(Brush), typeof(GraphicsInteropLine), new PropertyMetadata(new SolidColorBrush(Colors.Black), BrushChanged));
        public Brush LineBrush
        {
            get { return (Brush)GetValue(LineBrushProperty); }
            set { SetValue(LineBrushProperty, value); }
        }
        public static readonly DependencyProperty StrokeWidthProperty = DependencyProperty.Register("StrokeWidth", typeof(double), typeof(GraphicsInteropLine), new PropertyMetadata(1.0, OtherPropertyChanged));
        public double StrokeWidth
        {
            get { return (double)GetValue(StrokeWidthProperty); }
            set { SetValue(StrokeWidthProperty, value); }
        }

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

        #region StrokeStyle Props
        public static DependencyProperty StrokeStartLineCapProperty = DependencyProperty.Register("StrokeStartLineCap", typeof(PenLineCap), typeof(GraphicsInteropLine), new PropertyMetadata(PenLineCap.Flat, StrokeStyleChanged));
        public PenLineCap StrokeStartLineCap
        {
            get { return (PenLineCap)GetValue(StrokeStartLineCapProperty); }
            set { SetValue(StrokeStartLineCapProperty, value); }
        }

        public static DependencyProperty StrokeEndLineCapProperty = DependencyProperty.Register("StrokeEndLineCap", typeof(PenLineCap), typeof(GraphicsInteropLine), new PropertyMetadata(PenLineCap.Flat, StrokeStyleChanged));
        public PenLineCap StrokeEndLineCap
        {
            get { return (PenLineCap)GetValue(StrokeEndLineCapProperty); }
            set { SetValue(StrokeEndLineCapProperty, value); }
        }

        public static DependencyProperty StrokeDashCapProperty = DependencyProperty.Register("StrokeDashCap", typeof(PenLineCap), typeof(GraphicsInteropLine), new PropertyMetadata(PenLineCap.Flat, StrokeStyleChanged));
        public PenLineCap StrokeDashCap
        {
            get { return (PenLineCap)GetValue(StrokeDashCapProperty); }
            set { SetValue(StrokeDashCapProperty, value); }
        }

        public static DependencyProperty StrokeDashStyleProperty = DependencyProperty.Register("StrokeDashStyle", typeof(DashStyle), typeof(GraphicsInteropLine), new PropertyMetadata(DashStyles.Solid, StrokeStyleChanged));
        public DashStyle StrokeDashStyle
        {
            get { return (DashStyle)GetValue(StrokeDashStyleProperty); }
            set { SetValue(StrokeDashStyleProperty, value); }
        }

        public static DependencyProperty StrokeDashOffsetProperty = DependencyProperty.Register("StrokeDashOffset", typeof(double), typeof(GraphicsInteropLine), new PropertyMetadata(0.0, StrokeStyleChanged));
        public double StrokeDashOffset
        {
            get { return (double)GetValue(StrokeDashOffsetProperty); }
            set { SetValue(StrokeDashOffsetProperty, value); }
        }

        public static DependencyProperty StrokeLineJoinProperty = DependencyProperty.Register("StrokeLineJoin", typeof(PenLineJoin), typeof(GraphicsInteropLine), new PropertyMetadata(PenLineJoin.Miter, StrokeStyleChanged));
        public PenLineJoin StrokeLineJoin
        {
            get { return (PenLineJoin)GetValue(StrokeLineJoinProperty); }
            set { SetValue(StrokeLineJoinProperty, value); }
        }

        public static DependencyProperty StrokeMiterLimitProperty = DependencyProperty.Register("StrokeMiterLimit", typeof(double), typeof(GraphicsInteropLine), new PropertyMetadata(1.0, StrokeStyleChanged));
        public double StrokeMiterLimit
        {
            get { return (double)GetValue(StrokeMiterLimitProperty); }
            set { SetValue(StrokeMiterLimitProperty, value); }
        }
        private bool _isStrokeStyleChanged = false;
        private static void StrokeStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GraphicsInteropLine element)
            {
                element._isStrokeStyleChanged = true;
            }
        }
        #endregion

        private DrawLine2DCommand _drawLine = null;
        private bool _isBrushChanged = false;
        private bool _isOtherPropertyChanged = false;
        public override void OnRender(RenderFrame renderFrame)
        {
            if (_drawLine == null)
            {
                _drawLine = renderFrame.CreateDrawCommnad2D<DrawLine2DCommand>();
                _isBrushChanged = true;
                _isOtherPropertyChanged = true;
                _isStrokeStyleChanged = true;
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
            if (_isBrushChanged)
            {
                _drawLine.brush = LineBrush.ToNativeBrush(renderFrame);
                _isBrushChanged = false;
            }
            if (_isOtherPropertyChanged)
            {
                _drawLine.point0 = Point0.ToNativeVector2();
                _drawLine.point1 = Point1.ToNativeVector2();
                _drawLine.strokeWidth = (float)StrokeWidth;
                _isOtherPropertyChanged = false;
            }
            renderFrame.PushDrawCommand(_drawLine);
        }
        private static void BrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GraphicsInteropLine element)
            {
                element._isBrushChanged = true;
            }
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
