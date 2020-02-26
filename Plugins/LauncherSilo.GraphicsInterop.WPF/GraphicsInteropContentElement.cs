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
    [ContentProperty("Content")]
    public abstract class GraphicsInteropContentElement : GraphicsInteropElement
    {
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(GraphicsInteropElement), typeof(GraphicsInteropContentElement), new PropertyMetadata(null));
        [Bindable(true)]
        public object Content
        {
            get
            {
                return (GraphicsInteropElement)GetValue(ContentProperty);
            }
            set
            {
                SetValue(ContentProperty, value);
            }
        }

        public static readonly DependencyProperty HorizontalContentAlignmentProperty = DependencyProperty.Register("HorizontalContentAlignment", typeof(System.Windows.HorizontalAlignment), typeof(GraphicsInteropContentElement), new PropertyMetadata(System.Windows.HorizontalAlignment.Center));
        public System.Windows.HorizontalAlignment HorizontalContentAlignment
        {
            get
            {
                return (System.Windows.HorizontalAlignment)GetValue(HorizontalContentAlignmentProperty);
            }
            set
            {
                SetValue(HorizontalContentAlignmentProperty, value);
            }
        }

        public static readonly DependencyProperty VerticalContentAlignmentProperty = DependencyProperty.Register("VerticalContentAlignment", typeof(System.Windows.VerticalAlignment), typeof(GraphicsInteropContentElement), new PropertyMetadata(System.Windows.VerticalAlignment.Center));
        public System.Windows.VerticalAlignment VerticalContentAlignment
        {
            get
            {
                return (System.Windows.VerticalAlignment)GetValue(VerticalContentAlignmentProperty);
            }
            set
            {
                SetValue(VerticalContentAlignmentProperty, value);
            }
        }
    }
}
