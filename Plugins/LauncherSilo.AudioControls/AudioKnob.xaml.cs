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
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(AudioKnob), new FrameworkPropertyMetadata((double)50.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ValuePropertyChanged, ValuePropertyCoerce));
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
        public static readonly DependencyProperty StartAngleProperty = DependencyProperty.Register("StartAngle", typeof(double), typeof(AudioKnob), new PropertyMetadata((double)-150));
        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }
        public static readonly DependencyProperty EndAngleProperty = DependencyProperty.Register("EndAngle", typeof(double), typeof(AudioKnob), new PropertyMetadata((double)150));
        public double EndAngle
        {
            get { return (double)GetValue(EndAngleProperty); }
            set { SetValue(EndAngleProperty, value); }
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
        public static readonly DependencyProperty CircleDiameterProperty = DependencyProperty.Register("CircleDiameter", typeof(double), typeof(AudioKnob), new PropertyMetadata(80.0));
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
        public static readonly DependencyProperty GaugeDiameterProperty = DependencyProperty.Register("GaugeDiameter", typeof(double), typeof(AudioKnob), new PropertyMetadata(100.0));
        private double GaugeDiameter
        {
            get { return (double)GetValue(GaugeDiameterProperty); }
            set { SetValue(GaugeDiameterProperty, value); }
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
        public static readonly DependencyProperty ValueAngleProperty = DependencyProperty.Register("ValueAngle", typeof(double), typeof(AudioKnob), new PropertyMetadata((double)0));
        public double ValueAngle
        {
            get { return (double)GetValue(ValueAngleProperty); }
            set { SetValue(ValueAngleProperty, value); }
        }
        private TransformGroup _knobAngleTransform = new TransformGroup();
        private RotateTransform _knobAngleRotate = new RotateTransform();
        private bool _isMouseDown = false;
        private Cursor _previousCursor = Cursors.None;
        private System.Drawing.Point _previousMousePosition = new System.Drawing.Point();
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


        private void OuterCircle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender as Ellipse).CaptureMouse())
            {
                return;
            }
            _isMouseDown = true;
            _previousCursor = Cursor;
            Cursor = Cursors.None;
            _previousMousePosition = System.Windows.Forms.Cursor.Position;
        }

        private void OuterCircle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = false;
            if (Cursor == Cursors.None)
            {
                Cursor = _previousCursor;
            }
            (sender as Ellipse).ReleaseMouseCapture();
        }

        private void OuterCircle_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isMouseDown)
            {
                System.Drawing.Point newMousePosition = System.Windows.Forms.Cursor.Position;
                double dY = (_previousMousePosition.Y - newMousePosition.Y);
                if (Math.Abs(dY) > _mouseMoveThreshold)
                {
                    Value += Math.Sign(dY) * Interval;
                    System.Windows.Forms.Cursor.Position = _previousMousePosition;
                }
            }
        }

        private void OuterCircle_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double d = e.Delta / 120;
            Value += d * Interval;
        }

        private void OuterCircle_LostMouseCapture(object sender, MouseEventArgs e)
        {
            _isMouseDown = false;
            if (Cursor == Cursors.None)
            {
                Cursor = _previousCursor;
            }
        }
        private void LayoutElements()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                GaugeDiameter = Math.Min(ActualHeight, ActualWidth);
                CircleDiameter = GaugeDiameter * 0.8;

                double knob_margin = GaugeDiameter * 0.6;
                AngleDiameter = GaugeDiameter * 0.1;
                AngleMargin = new Thickness(0, 0, 0, knob_margin);
                double CenterY = ((knob_margin / AngleDiameter) * 0.5) + 0.5;
                AngleOffset = new Point(0.5, CenterY);

                Update();
            }));


        }
        public void Update()
        {
            if (!IsLoaded)
            {
                return;
            }
            if ((Maximum - Minimum) == 0.0)
            {
                return;
            }

            double newAngle = (EndAngle - StartAngle) / (Maximum - Minimum) * (Value - Minimum) + StartAngle;
            _knobAngleRotate.Angle = newAngle;
            ValueAngle = newAngle;
        }

        private static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AudioKnob control)
            {
                control.Update();
            }
        }
        private static object ValuePropertyCoerce(DependencyObject d, object baseValue)
        {
            if (d is AudioKnob control)
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
            if (d is AudioKnob control)
            {
                control.Update();
            }
        }
        private static object MaximumPropertyCoerce(DependencyObject d, object baseValue)
        {
            if (d is AudioKnob control)
            {
            }
            return baseValue;
        }
        private static void MinimumPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AudioKnob control)
            {
                control.Update();
            }
        }
        private static object MinimumPropertyCoerce(DependencyObject d, object baseValue)
        {
            if (d is AudioKnob control)
            {

            }
            return baseValue;
        }


    }
}
