using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LauncherSilo.AudioControls
{
    public enum ChartOrientation
    {
        LeftToRight,
        RightToLeft,
        TopToBottom,
        BottomToTop,
    }
    /// <summary>
    /// UserControl1.xaml の相互作用ロジック
    /// </summary>
    public partial class AudioMeter : UserControl
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(AudioMeter), new PropertyMetadata((double)50.0, ValuePropertyChanged, ValuePropertyCoerce));
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(AudioMeter), new PropertyMetadata((double)100.0, MaximumPropertyChanged, MaximumPropertyCoerce));
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(AudioMeter), new PropertyMetadata((double)0.0, MinimumPropertyChanged, MinimumPropertyCoerce));
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(ChartOrientation), typeof(AudioMeter), new PropertyMetadata(ChartOrientation.BottomToTop, OrientationPropertyChanged));
        public ChartOrientation Orientation
        {
            get { return (ChartOrientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Brush), typeof(AudioMeter), new PropertyMetadata(Brushes.White, ColorPropertyChanged));
        public Brush Color
        {
            get { return (Brush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }
        public static readonly DependencyProperty HighlightProperty = DependencyProperty.Register("Highlight", typeof(Brush), typeof(AudioMeter), new PropertyMetadata(Brushes.LightGray, HighlightPropertyChanged));
        public Brush Highlight
        {
            get { return (Brush)GetValue(HighlightProperty); }
            set { SetValue(HighlightProperty, value); }
        }
        public static readonly DependencyProperty ChartLengthProperty = DependencyProperty.Register("ChartLength", typeof(double), typeof(AudioMeter), new PropertyMetadata((double)0.0));
        public double ChartLength
        {
            get { return (double)GetValue(ChartLengthProperty); }
            set { SetValue(ChartLengthProperty, value); }
        }
        public static readonly DependencyProperty TensionSizeProperty = DependencyProperty.Register("TensionSize", typeof(double), typeof(AudioMeter), new PropertyMetadata((double)0.0));
        public double TensionSize
        {
            get { return (double)GetValue(TensionSizeProperty); }
            set { SetValue(TensionSizeProperty, value); }
        }
        public static readonly DependencyProperty TensionTargetPositionProperty = DependencyProperty.Register("TensionTargetPosition", typeof(double), typeof(AudioMeter), new PropertyMetadata((double)0.0));
        public double TensionTargetPosition
        {
            get { return (double)GetValue(TensionTargetPositionProperty); }
            set { SetValue(TensionTargetPositionProperty, value); }
        }
        private TransformGroup _tensionTransform = new TransformGroup();
        private TranslateTransform _tensionTranslation = new TranslateTransform();
        private System.Windows.Media.Animation.Storyboard _beginTentionDown = null;

        public AudioMeter()
        {
            InitializeComponent();
            _tensionTransform.Children.Add(_tensionTranslation);
        }

        public void UpdateChartLength()
        {
            if (!IsLoaded)
            {
                return;
            }
            if ((Maximum - Minimum) == 0.0)
            {
                return;
            }

            double percent = (Value - Minimum) / (Maximum - Minimum);
            ChartLength = percent * this.ActualHeight;
            if ((-_tensionTranslation.Y) < ChartLength)
            {
                _tensionTranslation.Y = -ChartLength;
                _beginTentionDown?.Stop();
            }
            else
            {
                _beginTentionDown?.Begin();
            }

        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TensionSize = this.ActualHeight * 0.05;
            tension.RenderTransform = _tensionTransform;
            _beginTentionDown = Resources["BeginTentionDown"] as System.Windows.Media.Animation.Storyboard;
            UpdateChartLength();

        }
        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateChartLength();
        }
        private static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AudioMeter control = d as AudioMeter;
            if (control != null)
            {
                control.UpdateChartLength();
            }
        }
        private static object ValuePropertyCoerce(DependencyObject d, object baseValue)
        {
            AudioMeter control = d as AudioMeter;
            if (control != null)
            {
                double NewValue = (double)baseValue;
                if (NewValue >= control.Maximum)
                {
                    return control.Maximum;
                }
                if (NewValue <= control.Minimum)
                {
                    return control.Minimum;
                }
            }
            return baseValue;
        }
        private static void MaximumPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AudioMeter control = d as AudioMeter;
            if (control != null)
            {
                control.UpdateChartLength();
            }
        }
        private static object MaximumPropertyCoerce(DependencyObject d, object baseValue)
        {
            AudioMeter control = d as AudioMeter;
            if (control != null)
            {
            }
            return baseValue;
        }
        private static void MinimumPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AudioMeter control = d as AudioMeter;
            if (control != null)
            {
                control.UpdateChartLength();
            }
        }
        private static object MinimumPropertyCoerce(DependencyObject d, object baseValue)
        {
            AudioMeter control = d as AudioMeter;
            if (control != null)
            {

            }
            return baseValue;
        }
        private static void OrientationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AudioMeter control = d as AudioMeter;
            if (control != null)
            {

            }
        }
        private static void ColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AudioMeter control = d as AudioMeter;
            if (control != null)
            {
            }
        }
        private static void HighlightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AudioMeter control = d as AudioMeter;
            if (control != null)
            {
            }
        }


    }
}
