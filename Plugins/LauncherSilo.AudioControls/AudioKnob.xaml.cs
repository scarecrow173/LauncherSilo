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
    /// <summary>
    /// AudioKnob.xaml の相互作用ロジック
    /// </summary>
    public partial class AudioKnob : UserControl
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(AudioKnob), new PropertyMetadata((double)50.0, ValuePropertyChanged, ValuePropertyCoerce));
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(AudioKnob), new PropertyMetadata((double)100.0, MaximumPropertyChanged, MaximumPropertyCoerce));
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(AudioKnob), new PropertyMetadata((double)0.0, MinimumPropertyChanged, MinimumPropertyCoerce));
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
        public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register("Interval", typeof(double), typeof(AudioKnob), new PropertyMetadata((double)1.0));
        public double Interval
        {
            get { return (double)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }
        public static readonly DependencyProperty BackgroundBrushProperty = DependencyProperty.Register("BackgroundBrush", typeof(Brush), typeof(AudioKnob), new PropertyMetadata(Brushes.LightGray));
        public Brush BackgroundBrush
        {
            get { return (Brush)GetValue(BackgroundBrushProperty); }
            set { SetValue(BackgroundBrushProperty, value); }
        }
        public static readonly DependencyProperty AccentBrushProperty = DependencyProperty.Register("AccentBrush", typeof(Brush), typeof(AudioKnob), new PropertyMetadata(Brushes.Black));
        public Brush AccentBrush
        {
            get { return (Brush)GetValue(AccentBrushProperty); }
            set { SetValue(AccentBrushProperty, value); }
        }
        public static readonly DependencyProperty CircleDiameterProperty = DependencyProperty.Register("CircleDiameter", typeof(double), typeof(AudioKnob), new PropertyMetadata(100.0));
        private double CircleDiameter
        {
            get { return (double)GetValue(CircleDiameterProperty); }
            set { SetValue(CircleDiameterProperty, value); }
        }
        public static readonly DependencyProperty AngleDiameterProperty = DependencyProperty.Register("AngleDiameter", typeof(double), typeof(AudioKnob), new PropertyMetadata(10.0));
        private double AngleDiameter
        {
            get { return (double)GetValue(AngleDiameterProperty); }
            set { SetValue(AngleDiameterProperty, value); }
        }
        public static readonly DependencyProperty AngleMarginProperty = DependencyProperty.Register("AngleMargin", typeof(Thickness), typeof(AudioKnob), new PropertyMetadata(new Thickness(0,0,0,80)));
        private Thickness AngleMargin
        {
            get { return (Thickness)GetValue(AngleMarginProperty); }
            set { SetValue(AngleMarginProperty, value); }
        }
        public static readonly DependencyProperty AngleOffsetProperty = DependencyProperty.Register("AngleOffset", typeof(Point), typeof(AudioKnob), new PropertyMetadata(new Point(0.5, 10.5)));
        private Point AngleOffset
        {
            get { return (Point)GetValue(AngleOffsetProperty); }
            set { SetValue(AngleOffsetProperty, value); }
        }

        private TransformGroup _knobAngleTransform = new TransformGroup();
        private RotateTransform _knobAngleRotate = new RotateTransform();
        private bool _isMouseDown = false;
        private Point _previousMousePosition = new Point();
        private double _mouseMoveThreshold = 5;

        public AudioKnob()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LayoutElements();
            _knobAngleTransform.Children.Add(_knobAngleRotate);
            knobAngle.RenderTransform = _knobAngleTransform;
        }
        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            LayoutElements();
        }
        private void OuterCircle_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double d = e.Delta / 120;
            Value += d * Interval;
        }

        private void OuterCircle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender as Ellipse).CaptureMouse())
            {
                return;
            }
            _isMouseDown = true;
            _previousMousePosition = e.GetPosition((Ellipse)sender);
        }

        private void OuterCircle_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isMouseDown)
            {
                Point newMousePosition = e.GetPosition((Ellipse)sender);
                double dY = (_previousMousePosition.Y - newMousePosition.Y);
                if (Math.Abs(dY) > _mouseMoveThreshold)
                {
                    Value += Math.Sign(dY) * Interval;
                    _previousMousePosition = newMousePosition;
                }
            }
        }

        private void OuterCircle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = false;
            (sender as Ellipse).ReleaseMouseCapture();
        }

        private void OuterCircle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }
        private void OuterCircle_LostMouseCapture(object sender, MouseEventArgs e)
        {
            _isMouseDown = false;
        }
        private void LayoutElements()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                double diameter = Math.Min(ActualHeight, ActualWidth);
                outerCircle.Width = diameter;
                outerCircle.Height = diameter;

                double knob_margin = diameter * 0.8;
                AngleDiameter = knob_margin * 0.1;
                AngleMargin = new Thickness(0, 0, 0, knob_margin);
                double CenterY = ((knob_margin / AngleDiameter) * 0.5) + 0.5;
                AngleOffset = new Point(0.5, CenterY);
            }));


        }
        public void UpdateValueAngle()
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
            _knobAngleRotate.Angle = (int)(percent * 360.0);
        }

        private static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AudioKnob control = d as AudioKnob;
            if (control != null)
            {
                control.UpdateValueAngle();
            }
        }
        private static object ValuePropertyCoerce(DependencyObject d, object baseValue)
        {
            AudioKnob control = d as AudioKnob;
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
            AudioKnob control = d as AudioKnob;
            if (control != null)
            {
                control.UpdateValueAngle();
            }
        }
        private static object MaximumPropertyCoerce(DependencyObject d, object baseValue)
        {
            AudioKnob control = d as AudioKnob;
            if (control != null)
            {
            }
            return baseValue;
        }
        private static void MinimumPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AudioKnob control = d as AudioKnob;
            if (control != null)
            {
                control.UpdateValueAngle();
            }
        }
        private static object MinimumPropertyCoerce(DependencyObject d, object baseValue)
        {
            AudioKnob control = d as AudioKnob;
            if (control != null)
            {

            }
            return baseValue;
        }


    }
}
