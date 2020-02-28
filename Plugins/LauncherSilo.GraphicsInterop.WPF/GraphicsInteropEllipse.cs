using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace LauncherSilo.GraphicsInterop.WPF
{
    public class GraphicsInteropEllipse : GraphicsInteropElement
    {
        public static readonly DependencyProperty EllipseBrushProperty = DependencyProperty.Register("EllipseBrush", typeof(Brush), typeof(GraphicsInteropEllipse), new PropertyMetadata(new SolidColorBrush(Colors.Black), BrushChanged));
        public Brush EllipseBrush
        {
            get { return (Brush)GetValue(EllipseBrushProperty); }
            set { SetValue(EllipseBrushProperty, value); }
        }
        public static readonly DependencyProperty StrokeWidthProperty = DependencyProperty.Register("StrokeWidth", typeof(double), typeof(GraphicsInteropEllipse), new PropertyMetadata(1.0, OtherPropertyChanged));
        public double StrokeWidth
        {
            get { return (double)GetValue(StrokeWidthProperty); }
            set { SetValue(StrokeWidthProperty, value); }
        }
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
        #region StrokeStyle Props
        public static DependencyProperty StrokeStartLineCapProperty = DependencyProperty.Register("StrokeStartLineCap", typeof(PenLineCap), typeof(GraphicsInteropEllipse), new PropertyMetadata(PenLineCap.Flat, StrokeStyleChanged));
        public PenLineCap StrokeStartLineCap
        {
            get { return (PenLineCap)GetValue(StrokeStartLineCapProperty); }
            set { SetValue(StrokeStartLineCapProperty, value); }
        }

        public static DependencyProperty StrokeEndLineCapProperty = DependencyProperty.Register("StrokeEndLineCap", typeof(PenLineCap), typeof(GraphicsInteropEllipse), new PropertyMetadata(PenLineCap.Flat, StrokeStyleChanged));
        public PenLineCap StrokeEndLineCap
        {
            get { return (PenLineCap)GetValue(StrokeEndLineCapProperty); }
            set { SetValue(StrokeEndLineCapProperty, value); }
        }

        public static DependencyProperty StrokeDashCapProperty = DependencyProperty.Register("StrokeDashCap", typeof(PenLineCap), typeof(GraphicsInteropEllipse), new PropertyMetadata(PenLineCap.Flat, StrokeStyleChanged));
        public PenLineCap StrokeDashCap
        {
            get { return (PenLineCap)GetValue(StrokeDashCapProperty); }
            set { SetValue(StrokeDashCapProperty, value); }
        }

        public static DependencyProperty StrokeDashStyleProperty = DependencyProperty.Register("StrokeDashStyle", typeof(DashStyle), typeof(GraphicsInteropEllipse), new PropertyMetadata(DashStyles.Solid, StrokeStyleChanged));
        public DashStyle StrokeDashStyle
        {
            get { return (DashStyle)GetValue(StrokeDashStyleProperty); }
            set { SetValue(StrokeDashStyleProperty, value); }
        }

        public static DependencyProperty StrokeDashOffsetProperty = DependencyProperty.Register("StrokeDashOffset", typeof(double), typeof(GraphicsInteropEllipse), new PropertyMetadata(0.0, StrokeStyleChanged));
        public double StrokeDashOffset
        {
            get { return (double)GetValue(StrokeDashOffsetProperty); }
            set { SetValue(StrokeDashOffsetProperty, value); }
        }

        public static DependencyProperty StrokeLineJoinProperty = DependencyProperty.Register("StrokeLineJoin", typeof(PenLineJoin), typeof(GraphicsInteropEllipse), new PropertyMetadata(PenLineJoin.Miter, StrokeStyleChanged));
        public PenLineJoin StrokeLineJoin
        {
            get { return (PenLineJoin)GetValue(StrokeLineJoinProperty); }
            set { SetValue(StrokeLineJoinProperty, value); }
        }

        public static DependencyProperty StrokeMiterLimitProperty = DependencyProperty.Register("StrokeMiterLimit", typeof(double), typeof(GraphicsInteropEllipse), new PropertyMetadata(1.0, StrokeStyleChanged));
        public double StrokeMiterLimit
        {
            get { return (double)GetValue(StrokeMiterLimitProperty); }
            set { SetValue(StrokeMiterLimitProperty, value); }
        }
        private bool _isStrokeStyleChanged = false;
        private static void StrokeStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GraphicsInteropEllipse element)
            {
                element._isStrokeStyleChanged = true;
            }
        }
        #endregion

        private DrawEllipse2DCommand _drawEllipse = null;

        public override void OnRender(RenderFrame renderFrame)
        {
            if (_drawEllipse == null)
            {
                _drawEllipse = renderFrame.CreateDrawCommnad2D<DrawEllipse2DCommand>();
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
                _drawEllipse.strokeStyle = renderFrame.Controller.CreateStrokeStyle(NewStrokeStyleProperties);
                _isStrokeStyleChanged = false;
            }
            if (_isBrushChanged)
            {
                _drawEllipse.brush = EllipseBrush.ToNativeBrush(renderFrame);
                _isBrushChanged = false;
            }
            if (_isOtherPropertyChanged)
            {
                _drawEllipse.ellipse = new SharpDX.Direct2D1.Ellipse(Center.ToNativeVector2(), (float)RadiusX, (float)RadiusY);
                _drawEllipse.strokeWidth = (float)StrokeWidth;
                _isOtherPropertyChanged = false;
            }
            renderFrame.PushDrawCommand(_drawEllipse);
        }

        private bool _isBrushChanged = false;
        private static void BrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GraphicsInteropEllipse element)
            {
                element._isBrushChanged = true;
            }
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
