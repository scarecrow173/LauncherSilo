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
using System.Windows.Threading;

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
        public static readonly DependencyProperty AccentProperty = DependencyProperty.Register("Accent", typeof(Brush), typeof(AudioMeter), new PropertyMetadata(Brushes.LightGray, HighlightPropertyChanged));
        public Brush Accent
        {
            get { return (Brush)GetValue(AccentProperty); }
            set { SetValue(AccentProperty, value); }
        }
        public static readonly DependencyProperty ChartLengthProperty = DependencyProperty.Register("ChartLength", typeof(double), typeof(AudioMeter), new PropertyMetadata((double)0.0));
        public double ChartLength
        {
            get { return (double)GetValue(ChartLengthProperty); }
            set { SetValue(ChartLengthProperty, value); }
        }
        public static readonly DependencyProperty PeakBarSizeProperty = DependencyProperty.Register("PeakBarSize", typeof(double), typeof(AudioMeter), new PropertyMetadata((double)0.0));
        public double PeakBarSize
        {
            get { return (double)GetValue(PeakBarSizeProperty); }
            set { SetValue(PeakBarSizeProperty, value); }
        }
        public static readonly DependencyProperty PeakBarPositionProperty = DependencyProperty.Register("PeakBarPosition", typeof(double), typeof(AudioMeter), new PropertyMetadata((double)0.0));
        public double PeakBarPosition
        {
            get { return (double)GetValue(PeakBarPositionProperty); }
            set { SetValue(PeakBarPositionProperty, value); }
        }
        public static readonly DependencyProperty PeakBarUpdateIntervalProperty = DependencyProperty.Register("PeakBarUpdateInterval", typeof(int), typeof(AudioMeter), new PropertyMetadata((int)25));
        public int PeakBarUpdateInterval
        {
            get { return (int)GetValue(PeakBarUpdateIntervalProperty); }
            set { SetValue(PeakBarUpdateIntervalProperty, value); }
        }
        public static readonly DependencyProperty PeakBarFallDelayProperty = DependencyProperty.Register("PeakBarFallDelay", typeof(int), typeof(AudioMeter), new PropertyMetadata((int)100));
        public int PeakBarFallDelay
        {
            get { return (int)GetValue(PeakBarFallDelayProperty); }
            set { SetValue(PeakBarFallDelayProperty, value); }
        }
        private TransformGroup _peakBarTransform = new TransformGroup();
        private TranslateTransform _peakBarTranslation = new TranslateTransform();
        private readonly DispatcherTimer _animationTimer = null;
        private TimeSpan _animationElapsedtime = new TimeSpan();

        public AudioMeter()
        {
            InitializeComponent();
            _animationTimer = new DispatcherTimer(DispatcherPriority.ApplicationIdle)
            {
                Interval = TimeSpan.FromMilliseconds(PeakBarUpdateInterval),
            };
            _animationTimer.Tick += _animationTimer_Tick; ;
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
        }
        private void _animationTimer_Tick(object sender, EventArgs e)
        {
            if (ChartLength >= -_peakBarTranslation.Y)
            {
                _peakBarTranslation.Y = -ChartLength;
                _animationElapsedtime = TimeSpan.FromMilliseconds(0);
            }
            else
            {
                _animationElapsedtime += _animationTimer.Interval;
                if (_animationElapsedtime > TimeSpan.FromMilliseconds(PeakBarFallDelay))
                {
                    _peakBarTranslation.Y += PeakBarSize;
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            PeakBarSize = this.ActualHeight * 0.05;
            _peakBarTransform.Children.Add(_peakBarTranslation);
            peakbar.RenderTransform = _peakBarTransform;
            UpdateChartLength();
            _animationTimer.Start();

        }
        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
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
