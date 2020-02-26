using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace LauncherSilo.GraphicsInterop.WPF
{
    [ContentProperty("Children")]
    public class GraphicsInteropPanel : GraphicsInteropElement
    {
        public ObservableCollection<GraphicsInteropElement> Children { get; } = new ObservableCollection<GraphicsInteropElement>();
        private GraphicsInteropImageSource imageSource = null;
        private System.Windows.Controls.Image image = null;
        private Clear2DCommand Clear = null;

        public GraphicsInteropPanel()
        {
            Children.CollectionChanged += Children_CollectionChanged;
            Loaded += GraphicsInteropPanel_Loaded;
            Unloaded += GraphicsInteropPanel_Unloaded;
            
        }
        private void GraphicsInteropPanel_Loaded(object sender, RoutedEventArgs e)
        {
            RenderFrame ParentRenderfarame = FindParentRenderFrame();
            if (ParentRenderfarame == null)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    Renderframe = new OffscreenRenderFrame();
                    Renderframe.Height = (int)ActualHeight;
                    Renderframe.Width = (int)ActualWidth;
                    Renderframe.RenderTargetChanged += Renderframe_RenderTargetChanged;
                    Renderframe.Initialize();
                    Clear = Renderframe.CreateDrawCommnad2D<Clear2DCommand>();
                    Clear.clearColor = SharpDX.Color.White;
                    image = new System.Windows.Controls.Image();
                    image.Width = ActualWidth;
                    image.Height = ActualHeight;
                    imageSource = new GraphicsInteropImageSource();
                    image.Source = imageSource;
                    SizeChanged += GraphicsInteropPanel_SizeChanged;
                    System.Windows.Media.CompositionTarget.Rendering += CompositionTarget_Rendering;
                }));
            }
        }
        private void GraphicsInteropPanel_Unloaded(object sender, RoutedEventArgs e)
        {

        }
        private void Renderframe_RenderTargetChanged(object sender, RenderTargetChanageArgs e)
        {
            if (imageSource != null)
            {
                imageSource.SetSurface(e.RenderTargetTexture);
            }
        }
        private void GraphicsInteropPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Renderframe != null)
            {
                Renderframe.Height = (int)e.NewSize.Height;
                Renderframe.Width = (int)e.NewSize.Width;
            }
        }
        private void Children_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (GraphicsInteropElement Child in e.OldItems)
                {
                    if (Child.Parent == this)
                    {
                        RemoveLogicalChild(Child);
                    }
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                var children = sender as IEnumerable;
                foreach (GraphicsInteropElement Child in children)
                {
                    if (Child.Parent == null)
                    {
                        AddLogicalChild(Child);
                    }
                }
            }
            else if (e.NewItems != null)
            {
                foreach (GraphicsInteropElement Child in e.NewItems)
                {
                    if (Child.Parent == null)
                    {
                        AddLogicalChild(Child);
                    }
                }
            }
        }
        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (Renderframe != null)
            {
                OnRender(Renderframe);
            }
        }
        public override void OnRender(RenderFrame renderFrame)
        {
            Renderframe?.PushDrawCommand(Clear);
            foreach (GraphicsInteropElement Child in Children)
            {
                Child.OnRender(renderFrame);
            }
            Renderframe?.FlushDrawCommand();
            imageSource?.Invalidate();
        }
    }
}
