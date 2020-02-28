using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace LauncherSilo.GraphicsInterop.WPF
{
    public abstract class GraphicsInteropPrimitiveElement : GraphicsInteropElement
    {
        public static readonly DependencyProperty StrokeBrushProperty = DependencyProperty.Register("StrokeBrush", typeof(Brush), typeof(GraphicsInteropRectangle), new PropertyMetadata(new SolidColorBrush(Colors.Black), StrokeBrushChanged));
        public Brush StrokeBrush
        {
            get { return (Brush)GetValue(StrokeBrushProperty); }
            set { SetValue(StrokeBrushProperty, value); }
        }
        public static readonly DependencyProperty StrokeWidthProperty = DependencyProperty.Register("StrokeWidth", typeof(double), typeof(GraphicsInteropRectangle), new PropertyMetadata(1.0, StrokeWidthChanged));
        public double StrokeWidth
        {
            get { return (double)GetValue(StrokeWidthProperty); }
            set { SetValue(StrokeWidthProperty, value); }
        }
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

        protected bool _isStrokeBrushChanged = false;
        protected bool _isStrokeWidthChanged = false;
        protected bool _isStrokeStyleChanged = false;

        protected static void StrokeBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GraphicsInteropPrimitiveElement element)
            {
                element._isStrokeBrushChanged = true;
            }
        }
        protected static void StrokeWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GraphicsInteropPrimitiveElement element)
            {
                element._isStrokeWidthChanged = true;
            }
        }
        protected static void StrokeStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GraphicsInteropPrimitiveElement element)
            {
                element._isStrokeStyleChanged = true;
            }
        }

    }
}
