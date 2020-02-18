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

        private TransformGroup _knobAngleTransform = new TransformGroup();
        private RotateTransform _knobAngleRotate = new RotateTransform();


        public AudioKnob()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _knobAngleTransform.Children.Add(_knobAngleRotate);
            knobAngle.RenderTransform = _knobAngleTransform;
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
