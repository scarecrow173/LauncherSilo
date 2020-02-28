using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace LauncherSilo.GraphicsInterop.WPF
{
    [ContentProperty("Content")]
    [TemplatePart(Name = "PART_Canvas", Type = typeof(GraphicsInteropImage))]
    public class GraphicsInteropViewport : Control
    {
        public GraphicsInteropElement Content { get; set; }
        private GraphicsInteropImage ControlImage { get; set; }
        private Clear2DCommand Clear = null;
        static GraphicsInteropViewport()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GraphicsInteropViewport), new FrameworkPropertyMetadata(typeof(GraphicsInteropViewport)));
        }
        public GraphicsInteropViewport()
        {
            Loaded += GraphicsInteropViewport_Loaded; ;
            Unloaded += GraphicsInteropViewport_Unloaded;
        }

        private void GraphicsInteropViewport_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                ControlImage.Width = (int)ActualWidth;
                ControlImage.Height = (int)ActualHeight;
                InvalidateVisual();
            }));

        }

        private void GraphicsInteropViewport_Unloaded(object sender, RoutedEventArgs e)
        {
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ControlImage = this.GetTemplateChild("PART_Canvas") as GraphicsInteropImage;
            Clear = ControlImage.Renderframe.CreateDrawCommnad2D<Clear2DCommand>();
            Clear.clearColor = SharpDX.Color.White;
            ControlImage.OnRenderNative += ControlImage_OnRenderNative;
        }
        private void ControlImage_OnRenderNative(object sender, OnRenderNativeArgs e)
        {
            Content?.OnPrepareRender(ControlImage.Renderframe);
            ControlImage.Renderframe.PushDrawCommand(Clear);
            Content?.OnRender(ControlImage.Renderframe);
        }

    }
}
