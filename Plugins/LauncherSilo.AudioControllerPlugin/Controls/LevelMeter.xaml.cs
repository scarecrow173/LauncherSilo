using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace LauncherSilo.AudioControllerPlugin.Controls
{
    public enum LevelOrientation
    {
        LeftToRight,
        RightToLeft,
        TopToBottom,
        BottomToTop,
    }

    /// <summary>
    /// LevelMeter.xaml の相互作用ロジック
    /// </summary>
    public partial class LevelMeter : UserControl
    {
        public static readonly DependencyProperty LevelProperty = DependencyProperty.Register("Level", typeof(int), typeof(LevelMeter), new PropertyMetadata((int)-90, LevelPropertyChanged, LevelPropertyCoerce));
        public int Level
        {
            get { return (int)GetValue(LevelProperty); }
            set { SetValue(LevelProperty, value); }
        }
        private static readonly DependencyProperty LevelPercentageProperty = DependencyProperty.Register("LevelPercentage", typeof(float), typeof(LevelMeter), new PropertyMetadata((float)0.0));
        private float LevelPercentage
        {
            get { return (float)GetValue(LevelPercentageProperty); }
            set { SetValue(LevelPercentageProperty, value); }
        }
        private static readonly DependencyProperty LevelPercentageDisplayProperty = DependencyProperty.Register("LevelPercentageDisplay", typeof(float), typeof(LevelMeter), new PropertyMetadata((float)0.0));
        private float LevelPercentageDisplay
        {
            get { return (float)GetValue(LevelPercentageDisplayProperty); }
            set { SetValue(LevelPercentageDisplayProperty, value); }
        }
        private static readonly DependencyProperty TensionTopProperty = DependencyProperty.Register("TensionTop", typeof(float), typeof(LevelMeter), new PropertyMetadata((float)-0.1));
        private float TensionTop
        {
            get { return (float)GetValue(TensionTopProperty); }
            set { SetValue(TensionTopProperty, value); }
        }
        private static readonly DependencyProperty TensionBottomProperty = DependencyProperty.Register("TensionBottom", typeof(float), typeof(LevelMeter), new PropertyMetadata((float)0.0));
        private float TensionBottom
        {
            get { return (float)GetValue(TensionBottomProperty); }
            set { SetValue(TensionBottomProperty, value); }
        }
        

        public static readonly DependencyProperty MaxLevelProperty = DependencyProperty.Register("MaxLevel", typeof(int), typeof(LevelMeter), new PropertyMetadata((int)0, MaxLevelPropertyChanged, MaxLevelPropertyCoerce));
        public int MaxLevel
        {
            get { return (int)GetValue(MaxLevelProperty); }
            set { SetValue(MaxLevelProperty, value); }
        }

        public static readonly DependencyProperty MinLevelProperty = DependencyProperty.Register("MinLevel", typeof(int), typeof(LevelMeter), new PropertyMetadata((int)-90, MinLevelPropertyChanged, MinLevelPropertyCoerce));
        public int MinLevel
        {
            get { return (int)GetValue(MinLevelProperty); }
            set { SetValue(MinLevelProperty, value); }
        }
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(LevelOrientation), typeof(LevelMeter), new PropertyMetadata(LevelOrientation.BottomToTop, OnOrientationChanged));
        public LevelOrientation Orientation
        {
            get { return (LevelOrientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }




        private readonly SynchronizationContext synchronizationContext;


        private static void LevelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LevelMeter meter = d as LevelMeter;
            if (meter != null)
            {
                meter.LevelPercentage = ((float)(((int)e.NewValue) - meter.MinLevel) / (float)(meter.MaxLevel - meter.MinLevel));
                if ((int)e.NewValue > (int)e.OldValue)
                {
                    meter._alpha = 1f;
                }
                else
                {
                    meter._alpha = 0f;
                }
                meter._prev = meter.LevelPercentageDisplay;
            }
        }
        private float Lerp(float a, float b, float t)
        {
            return (1f - t) * a + t * b;
        }

        private static object LevelPropertyCoerce(DependencyObject d, object baseValue)
        {
            LevelMeter meter = d as LevelMeter;
            if (meter != null)
            {
                int NewValue = (int)baseValue;
                if (NewValue >= meter.MaxLevel)
                {
                    return meter.MaxLevel;
                }
                if (NewValue <= meter.MinLevel)
                {
                    return meter.MinLevel;
                }
            }
            return baseValue;
        }
        
        private static void MaxLevelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LevelMeter meter = d as LevelMeter;
            if (meter != null)
            {
                meter.LevelPercentage = ((float)(meter.Level - meter.MinLevel) / (float)(((int)e.NewValue) - meter.MinLevel));
            }
        }
        private static object MaxLevelPropertyCoerce(DependencyObject d, object baseValue)
        {
            return baseValue;
        }
        private static void MinLevelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LevelMeter meter = d as LevelMeter;
            if (meter != null)
            {
                meter.LevelPercentage = ((float)(meter.Level - ((int)e.NewValue)) / (float)(meter.MaxLevel - ((int)e.NewValue)));
            }
        }
        private static object MinLevelPropertyCoerce(DependencyObject d, object baseValue)
        {
            return baseValue;
        }

        private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs value)
        {
            LevelMeter Meter = (LevelMeter)d;
            if (Meter != null)
            {
            }
        }

        public LevelMeter()
        {
            synchronizationContext = SynchronizationContext.Current;
            InitializeComponent();
            PeekUpdateTimer.Elapsed += PeekUpdateTimer_Elapsed;
            PeekUpdateTimer.Start();

        }

        private float _alpha = 1f;
        private float _prev = 1f;
        private float _startdownduration = 0.5f;
        private float _elapseddowntime = 0f;

        private System.Timers.Timer PeekUpdateTimer = new System.Timers.Timer(16) { AutoReset = true };

        private void PeekUpdateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                synchronizationContext.Post(s =>
                {

                    if (LevelPercentage >= TensionTop)
                    {
                        TensionTop = LevelPercentage;
                        TensionBottom = TensionTop + 0.02f;
                        _elapseddowntime = 0f;

                    }
                    else if (LevelPercentage < TensionTop)
                    {
                        _elapseddowntime += 0.016f;
                        if (_elapseddowntime >= _startdownduration)
                        {
                            TensionTop = TensionTop - 0.02f;
                            TensionBottom = TensionTop + 0.02f;
                        }
                    }
                    if (_alpha <= 1f)
                    {
                        _alpha += 0.2f;
                    }
                    if (LevelPercentage >= LevelPercentageDisplay)
                    {
                        LevelPercentageDisplay = LevelPercentage;
                    }
                    else if (LevelPercentage < LevelPercentageDisplay)
                    {
                        LevelPercentageDisplay = LevelPercentageDisplay - 0.02f;

                    }
                    //LevelPercentageDisplay = Lerp(_prev, LevelPercentage, _alpha);

                }, null);
            }
            catch(Exception ex)
            {

            }
        }
    }
}
