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
        public static readonly DependencyProperty RenderframeProperty = DependencyProperty.Register("Renderframe", typeof(RenderFrame), typeof(GraphicsInteropElement), new PropertyMetadata(null));
        public RenderFrame Renderframe
        {
            get
            {
                return (RenderFrame)GetValue(RenderframeProperty);
            }
            set
            {
                SetValue(RenderframeProperty, value);
            }
        }
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
        protected RenderFrame FindParentRenderFrame()
        {
            GraphicsInteropElement parentElement = this.Parent as GraphicsInteropElement;
            while (parentElement != null)
            {
                if (parentElement.Renderframe != null)
                {
                    return parentElement.Renderframe;
                }
                parentElement = parentElement.Parent as GraphicsInteropElement;
            }
            return null;
        }
        public abstract void OnRender(RenderFrame renderFrame);
        
    }
}
