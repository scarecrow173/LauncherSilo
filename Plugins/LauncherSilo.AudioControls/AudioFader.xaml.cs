using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LauncherSilo.AudioControls
{
    /// <summary>
    /// AudioFader.xaml の相互作用ロジック
    /// </summary>
    public partial class AudioFader : UserControl
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(AudioFader), new FrameworkPropertyMetadata((double)50.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ValuePropertyChanged, ValuePropertyCoerce));
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(AudioFader), new PropertyMetadata((double)100.0, MaximumPropertyChanged, MaximumPropertyCoerce));
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(AudioFader), new PropertyMetadata((double)0.0, MinimumPropertyChanged, MinimumPropertyCoerce));
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
        public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register("Interval", typeof(double), typeof(AudioFader), new PropertyMetadata((double)1.0));
        public double Interval
        {
            get { return (double)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }
        public static readonly DependencyProperty LengthProperty = DependencyProperty.Register("Length", typeof(double), typeof(AudioFader), new PropertyMetadata((double)0.0));
        public double Length
        {
            get { return (double)GetValue(LengthProperty); }
            set { SetValue(LengthProperty, value); }
        }
        public static readonly DependencyProperty ValuePositionProperty = DependencyProperty.Register("ValuePosition", typeof(double), typeof(AudioFader), new PropertyMetadata((double)0.0));
        private double ValuePosition
        {
            get { return (double)GetValue(ValuePositionProperty); }
            set { SetValue(ValuePositionProperty, value); }
        }
        public static readonly DependencyProperty BackgroundBrushProperty = DependencyProperty.Register("BackgroundBrush", typeof(Brush), typeof(AudioFader), new PropertyMetadata(Brushes.LightGray));
        public Brush BackgroundBrush
        {
            get { return (Brush)GetValue(BackgroundBrushProperty); }
            set { SetValue(BackgroundBrushProperty, value); }
        }
        public static readonly DependencyProperty AccentBrushProperty = DependencyProperty.Register("AccentBrush", typeof(Brush), typeof(AudioFader), new PropertyMetadata(Brushes.White));
        public Brush AccentBrush
        {
            get { return (Brush)GetValue(AccentBrushProperty); }
            set { SetValue(AccentBrushProperty, value); }
        }
        public static readonly DependencyProperty CursorSizeProperty = DependencyProperty.Register("CursorSize", typeof(double), typeof(AudioFader), new PropertyMetadata((double)50));
        private double CursorSize
        {
            get { return (double)GetValue(CursorSizeProperty); }
            set { SetValue(CursorSizeProperty, value); }
        }
        public static readonly DependencyProperty OuterSizeProperty = DependencyProperty.Register("OuterSize", typeof(double), typeof(AudioFader), new PropertyMetadata((double)20));
        private double OuterSize
        {
            get { return (double)GetValue(OuterSizeProperty); }
            set { SetValue(OuterSizeProperty, value); }
        }
        public static readonly DependencyProperty InnerSizeProperty = DependencyProperty.Register("InnerSize", typeof(double), typeof(AudioFader), new PropertyMetadata((double)10));
        private double InnerSize
        {
            get { return (double)GetValue(InnerSizeProperty); }
            set { SetValue(InnerSizeProperty, value); }
        }
        public static readonly DependencyProperty BackgroundCornerRadiusProperty = DependencyProperty.Register("BackgroundCornerRadius", typeof(CornerRadius), typeof(AudioFader), new PropertyMetadata(new CornerRadius(10)));
        private CornerRadius BackgroundCornerRadius
        {
            get { return (CornerRadius)GetValue(BackgroundCornerRadiusProperty); }
            set { SetValue(BackgroundCornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty HeightlightCornerRadiusProperty = DependencyProperty.Register("HeightlightCornerRadius", typeof(CornerRadius), typeof(AudioFader), new PropertyMetadata(new CornerRadius(0,0,10,10)));
        private CornerRadius HeightlightCornerRadius
        {
            get { return (CornerRadius)GetValue(HeightlightCornerRadiusProperty); }
            set { SetValue(HeightlightCornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty CursorCornerRadiusProperty = DependencyProperty.Register("CursorCornerRadius", typeof(CornerRadius), typeof(AudioFader), new PropertyMetadata(new CornerRadius(1)));

        private CornerRadius CursorCornerRadius
        {
            get { return (CornerRadius)GetValue(CursorCornerRadiusProperty); }
            set { SetValue(CursorCornerRadiusProperty, value); }
        }

        public AudioFader()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LayoutElements();
        }
        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            LayoutElements();
        }
        private void LayoutElements()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Length = ActualHeight;
                CursorSize = ActualHeight * 0.1;
                OuterSize = ActualWidth;
                InnerSize = ActualWidth * 0.5;
                BackgroundCornerRadius = new CornerRadius(InnerSize);
                HeightlightCornerRadius = new CornerRadius(0, 0, InnerSize, InnerSize);
                CursorCornerRadius = new CornerRadius(CursorSize * 0.1);
                Update();
            }));


        }
        private void Update()
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
            double position = (Length - CursorSize) * percent;
            ValuePosition = position;
        }

        private void Thumb_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double d = e.Delta / 120;
            Value += d * Interval;
            Update();
        }
        private void Thumb_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            if (!(sender as Thumb).CaptureMouse())
            {
                return;
            }
        }
        private void Thumb_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            (sender as Thumb).ReleaseMouseCapture();
        }

        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            double move = Canvas.GetBottom(sender as UIElement) - e.VerticalChange;
            if (move <= 0)
            {
                move = 0;
            }
            if (move >= Length - CursorSize)
            {
                move = Length - CursorSize;
            }
            Value =  (Maximum - Minimum) * (move / (Length - CursorSize));
            Update();
        }
        private static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AudioFader control = d as AudioFader;
            if (control != null)
            {
                control.Update();
            }
        }
        private static object ValuePropertyCoerce(DependencyObject d, object baseValue)
        {
            AudioFader control = d as AudioFader;
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
            AudioFader control = d as AudioFader;
            if (control != null)
            {
                control.Update();
            }
        }
        private static object MaximumPropertyCoerce(DependencyObject d, object baseValue)
        {
            AudioFader control = d as AudioFader;
            if (control != null)
            {
            }
            return baseValue;
        }
        private static void MinimumPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AudioFader control = d as AudioFader;
            if (control != null)
            {
                control.Update();
            }
        }
        private static object MinimumPropertyCoerce(DependencyObject d, object baseValue)
        {
            AudioFader control = d as AudioFader;
            if (control != null)
            {

            }
            return baseValue;
        }


    }
}
