using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace LauncherSilo.GraphicsInterop.WPF
{
    public abstract class GraphicsInteropElement : FrameworkElement
    {
        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register("Background", typeof(Brush), typeof(GraphicsInteropElement), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));
        public Brush Background
        {
            get
            {
                return (Brush)GetValue(BackgroundProperty);
            }
            set
            {
                SetValue(BackgroundProperty, value);
            }
        }
        public abstract void OnPrepareRender(RenderFrame renderFrame);
        public abstract void OnRender(RenderFrame renderFrame);
        
    }
}
