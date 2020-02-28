using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace LauncherSilo.GraphicsInterop.WPF
{
    public abstract class GraphicsInteropFillableElement : GraphicsInteropPrimitiveElement
    {
        public static readonly DependencyProperty FillBrushProperty = DependencyProperty.Register("FillBrush", typeof(Brush), typeof(GraphicsInteropRectangle), new PropertyMetadata(null, FillBrushChanged));
        public Brush FillBrush
        {
            get { return (Brush)GetValue(FillBrushProperty); }
            set { SetValue(FillBrushProperty, value); }
        }
        protected bool _isFillBrushChanged = false;

        protected static void FillBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GraphicsInteropFillableElement element)
            {
                element._isFillBrushChanged = true;
            }
        }
    }
}
