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
        private GraphicsInteropImage image = null;

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
                image = new GraphicsInteropImage();
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    image.Width = ActualWidth;
                    image.Height = ActualHeight;
                    image.OnRenderNative += Image_OnRenderNative;
                    Renderframe = image.Renderframe;
                    AddLogicalChild(image);
                    AddVisualChild(image);
                    Measure(new Size(ActualWidth, ActualHeight));
                    image.Measure(new Size(ActualWidth, ActualHeight));
                    InvalidateVisual();
                    image.InvalidateArrange();
                    image.InvalidateMeasure();
                    image.InvalidateVisual();

                }));
            }
        }



        private void GraphicsInteropPanel_Unloaded(object sender, RoutedEventArgs e)
        {

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
        private void Image_OnRenderNative(object sender, OnRenderNativeArgs e)
        {
            OnRender(e.Renderframe);
        }
        public override void OnRender(RenderFrame renderFrame)
        {
            foreach (GraphicsInteropElement Child in Children)
            {
                Child.OnRender(renderFrame);
            }
        }
    }
}
