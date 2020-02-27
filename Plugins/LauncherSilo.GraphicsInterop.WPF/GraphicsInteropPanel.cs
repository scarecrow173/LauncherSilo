using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace LauncherSilo.GraphicsInterop.WPF
{
    [ContentProperty("Children")]
    public class GraphicsInteropPanel : GraphicsInteropElement
    {
        public ObservableCollection<GraphicsInteropElement> Children { get; } = new ObservableCollection<GraphicsInteropElement>();

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
        public override void OnRender(RenderFrame renderFrame)
        {
            foreach (GraphicsInteropElement Child in Children)
            {
                Child.OnRender(renderFrame);
            }
        }
    }
}
