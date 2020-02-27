using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace LauncherSilo.GraphicsInterop.WPF
{
    public class OnRenderNativeArgs : EventArgs
    {
        public RenderFrame Renderframe { get; private set; }
        public OnRenderNativeArgs(RenderFrame renderframe)
        {
            Renderframe = renderframe;
        }
    }
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
        public OffscreenRenderFrame Renderframe { get; set; } = new OffscreenRenderFrame();
        public event EventHandler<OnRenderNativeArgs> OnRenderNative;


        private GraphicsInteropImageSource imageSource = new GraphicsInteropImageSource();

        static GraphicsInteropImage()
        {
            StretchProperty.OverrideMetadata(typeof(GraphicsInteropImage), new FrameworkPropertyMetadata(Stretch.Fill));
        }
        public GraphicsInteropImage()
        {
            base.Loaded += GraphicsInteropImage_Loaded;
            base.Unloaded += GraphicsInteropImage_Unloaded;
            base.Stretch = System.Windows.Media.Stretch.Fill;
            SizeChanged += GraphicsInteropImage_SizeChanged;
        }

        private void GraphicsInteropImage_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsInDesignMode)
            {
                return;
            }
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Renderframe.Height = (int)DesiredSize.Height;
                Renderframe.Width = (int)DesiredSize.Width;
                Renderframe.RenderTargetChanged += RenderFrame_RenderTargetChanged;
                Renderframe.Initialize();
                Source = imageSource;
                System.Windows.Media.CompositionTarget.Rendering += CompositionTarget_Rendering;
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
            Renderframe.Height = (int)ActualHeight;
            Renderframe.Width = (int)ActualWidth;
        }
        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            OnRenderNative?.Invoke(this, new OnRenderNativeArgs(Renderframe));
            Renderframe.FlushDrawCommand();
            imageSource.Invalidate();
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
        }

    }
}
