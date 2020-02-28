using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace LauncherSilo.GraphicsInterop.WPF
{
    public class GraphicsInteropRectangle : GraphicsInteropFillableElement
    {
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

        private DrawRectangle2DCommand _drawRectangle = null;
        private FillRectangle2DCommand _fillRectangle = null;
        public override void OnPrepareRender(RenderFrame renderFrame)
        {
            if (_drawRectangle == null)
            {
                _drawRectangle = renderFrame.CreateDrawCommnad2D<DrawRectangle2DCommand>();
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
                _drawRectangle.strokeStyle = renderFrame.Controller.CreateStrokeStyle(NewStrokeStyleProperties);
                _isStrokeStyleChanged = false;
            }
            if (_isStrokeBrushChanged)
            {
                _drawRectangle.brush = StrokeBrush.ToNativeBrush(renderFrame);
                _isStrokeBrushChanged = false;
            }
            if (_isStrokeWidthChanged)
            {
                _drawRectangle.strokeWidth = (float)StrokeWidth;
                _isStrokeWidthChanged = false;
            }
            if (_fillRectangle == null && FillBrush != null)
            {
                _fillRectangle = renderFrame.CreateDrawCommnad2D<FillRectangle2DCommand>();
                _isFillBrushChanged = true;
            }
            if (_isFillBrushChanged)
            {
                if (_fillRectangle != null)
                {
                    if (FillBrush != null)
                    {
                        _fillRectangle.brush = FillBrush.ToNativeBrush(renderFrame);
                    }
                    else
                    {
                        _fillRectangle = null;
                    }
                }
                _isFillBrushChanged = false;
            }
            if (_isOtherPropertyChanged)
            {
                _drawRectangle.rect = new SharpDX.Mathematics.Interop.RawRectangleF((float)Left, (float)Top, (float)Right, (float)Bottom);
                if (_fillRectangle != null)
                {
                    _fillRectangle.rect = _drawRectangle.rect;
                }
                _isOtherPropertyChanged = false;
            }
        }
        public override void OnRender(RenderFrame renderFrame)
        {
            if (_fillRectangle != null)
            {
                renderFrame.PushDrawCommand(_fillRectangle);
            }
            renderFrame.PushDrawCommand(_drawRectangle);
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
