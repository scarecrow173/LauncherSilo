using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

namespace LauncherSilo.GraphicsInterop.WPF
{
    public class GraphicsInteropImage : System.Windows.Controls.Image
    {
        public bool IsInDesignMode
        {
            get
            {
                var prop = DesignerProperties.IsInDesignModeProperty;
                var isDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue;
                return isDesignMode;
            }
        }
        private OffscreenRenderFrame renderFrame = new OffscreenRenderFrame();
        private GraphicsInteropImageSource imageSource = new GraphicsInteropImageSource();

        public GraphicsInteropImage()
        {
            Loaded += GraphicsInteropImage_Loaded;
            Unloaded += GraphicsInteropImage_Unloaded;
            Stretch = System.Windows.Media.Stretch.Fill;
        }

        private void GraphicsInteropImage_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsInDesignMode)
            {
                return;
            }
            Dispatcher.BeginInvoke(new Action(() =>
            {
                renderFrame.Height = (int)ActualHeight;
                renderFrame.Width = (int)ActualWidth;
                renderFrame.RenderTargetChanged += RenderFrame_RenderTargetChanged; ;
                renderFrame.Initialize();
                Source = imageSource;
                SizeChanged += GraphicsInteropImage_SizeChanged;

            }));
        }

        private void GraphicsInteropImage_Unloaded(object sender, RoutedEventArgs e)
        {
            if (IsInDesignMode)
            {
                return;
            }
        }

        private void RenderFrame_RenderTargetChanged(object sender, RenderTargetChanageArgs e)
        {
            imageSource.SetSurface(e.RenderTargetTexture);
        }
        private void GraphicsInteropImage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            renderFrame.Height = (int)ActualHeight;
            renderFrame.Width = (int)ActualWidth;
        }
    }
}
