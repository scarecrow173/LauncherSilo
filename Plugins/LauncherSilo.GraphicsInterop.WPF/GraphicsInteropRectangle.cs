using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace LauncherSilo.GraphicsInterop.WPF
{
    public class GraphicsInteropRectangle : GraphicsInteropElement
    {
        public static readonly DependencyProperty RectangleBrushProperty = DependencyProperty.Register("RectangleBrush", typeof(Brush), typeof(GraphicsInteropRectangle), new PropertyMetadata(new SolidColorBrush(Colors.Black), BrushChanged));
        public Brush RectangleBrush
        {
            get { return (Brush)GetValue(RectangleBrushProperty); }
            set { SetValue(RectangleBrushProperty, value); }
        }
        public static readonly DependencyProperty StrokeWidthProperty = DependencyProperty.Register("StrokeWidth", typeof(double), typeof(GraphicsInteropRectangle), new PropertyMetadata(1.0, OtherPropertyChanged));
        public double StrokeWidth
        {
            get { return (double)GetValue(StrokeWidthProperty); }
            set { SetValue(StrokeWidthProperty, value); }
        }
        public static readonly DependencyProperty LeftProperty = DependencyProperty.Register("Left", typeof(double), typeof(GraphicsInteropEllipse), new PropertyMetadata(0.0, OtherPropertyChanged));
        public double Left
        {
            get { return (double)GetValue(LeftProperty); }
            set { SetValue(LeftProperty, value); }
        }

        public static readonly DependencyProperty TopProperty = DependencyProperty.Register("Top", typeof(double), typeof(GraphicsInteropEllipse), new PropertyMetadata(0.0, OtherPropertyChanged));
        public double Top
        {
            get { return (double)GetValue(TopProperty); }
            set { SetValue(TopProperty, value); }
        }

        public static readonly DependencyProperty RightProperty = DependencyProperty.Register("Right", typeof(double), typeof(GraphicsInteropEllipse), new PropertyMetadata(0.0, OtherPropertyChanged));
        public double Right
        {
            get { return (double)GetValue(RightProperty); }
            set { SetValue(RightProperty, value); }
        }

        public static readonly DependencyProperty BottomProperty = DependencyProperty.Register("Bottom", typeof(double), typeof(GraphicsInteropEllipse), new PropertyMetadata(0.0, OtherPropertyChanged));
        public double Bottom
        {
            get { return (double)GetValue(BottomProperty); }
            set { SetValue(BottomProperty, value); }
        }

        #region StrokeStyle Props
        public static DependencyProperty StrokeStartLineCapProperty = DependencyProperty.Register("StrokeStartLineCap", typeof(PenLineCap), typeof(GraphicsInteropRectangle), new PropertyMetadata(PenLineCap.Flat, StrokeStyleChanged));
        public PenLineCap StrokeStartLineCap
        {
            get { return (PenLineCap)GetValue(StrokeStartLineCapProperty); }
            set { SetValue(StrokeStartLineCapProperty, value); }
        }

        public static DependencyProperty StrokeEndLineCapProperty = DependencyProperty.Register("StrokeEndLineCap", typeof(PenLineCap), typeof(GraphicsInteropRectangle), new PropertyMetadata(PenLineCap.Flat, StrokeStyleChanged));
        public PenLineCap StrokeEndLineCap
        {
            get { return (PenLineCap)GetValue(StrokeEndLineCapProperty); }
            set { SetValue(StrokeEndLineCapProperty, value); }
        }

        public static DependencyProperty StrokeDashCapProperty = DependencyProperty.Register("StrokeDashCap", typeof(PenLineCap), typeof(GraphicsInteropRectangle), new PropertyMetadata(PenLineCap.Flat, StrokeStyleChanged));
        public PenLineCap StrokeDashCap
        {
            get { return (PenLineCap)GetValue(StrokeDashCapProperty); }
            set { SetValue(StrokeDashCapProperty, value); }
        }

        public static DependencyProperty StrokeDashStyleProperty = DependencyProperty.Register("StrokeDashStyle", typeof(DashStyle), typeof(GraphicsInteropRectangle), new PropertyMetadata(DashStyles.Solid, StrokeStyleChanged));
        public DashStyle StrokeDashStyle
        {
            get { return (DashStyle)GetValue(StrokeDashStyleProperty); }
            set { SetValue(StrokeDashStyleProperty, value); }
        }

        public static DependencyProperty StrokeDashOffsetProperty = DependencyProperty.Register("StrokeDashOffset", typeof(double), typeof(GraphicsInteropRectangle), new PropertyMetadata(0.0, StrokeStyleChanged));
        public double StrokeDashOffset
        {
            get { return (double)GetValue(StrokeDashOffsetProperty); }
            set { SetValue(StrokeDashOffsetProperty, value); }
        }

        public static DependencyProperty StrokeLineJoinProperty = DependencyProperty.Register("StrokeLineJoin", typeof(PenLineJoin), typeof(GraphicsInteropRectangle), new PropertyMetadata(PenLineJoin.Miter, StrokeStyleChanged));
        public PenLineJoin StrokeLineJoin
        {
            get { return (PenLineJoin)GetValue(StrokeLineJoinProperty); }
            set { SetValue(StrokeLineJoinProperty, value); }
        }

        public static DependencyProperty StrokeMiterLimitProperty = DependencyProperty.Register("StrokeMiterLimit", typeof(double), typeof(GraphicsInteropRectangle), new PropertyMetadata(1.0, StrokeStyleChanged));
        public double StrokeMiterLimit
        {
            get { return (double)GetValue(StrokeMiterLimitProperty); }
            set { SetValue(StrokeMiterLimitProperty, value); }
        }
        private bool _isStrokeStyleChanged = false;
        private static void StrokeStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GraphicsInteropRectangle element)
            {
                element._isStrokeStyleChanged = true;
            }
        }
        #endregion

        private DrawRectangle2DCommand _drawRectangle = null;
        public override void OnRender(RenderFrame renderFrame)
        {
            if (_drawRectangle == null)
            {
                _drawRectangle = renderFrame.CreateDrawCommnad2D<DrawRectangle2DCommand>();
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
                _drawRectangle.strokeStyle = renderFrame.Controller.CreateStrokeStyle(NewStrokeStyleProperties);
                _isStrokeStyleChanged = false;
            }
            if (_isBrushChanged)
            {
                _drawRectangle.brush = RectangleBrush.ToNativeBrush(renderFrame);
                _isBrushChanged = false;
            }
            if (_isOtherPropertyChanged)
            {
                _drawRectangle.strokeWidth = (float)StrokeWidth;
                _drawRectangle.rect = new SharpDX.Mathematics.Interop.RawRectangleF((float)Left, (float)Top, (float)Right, (float)Bottom);
                _isOtherPropertyChanged = false;
            }
            renderFrame.PushDrawCommand(_drawRectangle);
        }

        private bool _isBrushChanged = false;
        private static void BrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GraphicsInteropRectangle element)
            {
                element._isBrushChanged = true;
            }
        }

        private bool _isOtherPropertyChanged = false;
        private static void OtherPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GraphicsInteropRectangle element)
            {
                element._isOtherPropertyChanged = true;
            }
        }
    }
}
