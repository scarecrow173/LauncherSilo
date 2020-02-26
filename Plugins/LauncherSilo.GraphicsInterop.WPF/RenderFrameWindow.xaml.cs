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


namespace LauncherSilo.GraphicsInterop.WPF
{
    /// <summary>
    /// RenderFrameWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class RenderFrameWindow : System.Windows.Window
    {
        public IntPtr Handle
        {
            get
            {
                var helper = new System.Windows.Interop.WindowInteropHelper(this);
                return helper.Handle;
            }
        }

        private OffscreenRenderFrame renderFrame = new OffscreenRenderFrame();
        private GraphicsInteropImageSource imageSource = new GraphicsInteropImageSource();
        private System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        private SharpDX.Direct2D1.SolidColorBrush EllipseBrush = null;
        private SharpDX.Color BackgroundColor = new SharpDX.Color();
        private Clear2DCommand Clear = null;
        private DrawEllipse2DCommand DrawEllipse = null;
        private FillEllipse2DCommand FillEllipse = null;
        private DrawLine2DCommand DrawLine = null;
        public RenderFrameWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                renderFrame.Height = (int)(this.Content as Grid).RenderSize.Height;
                renderFrame.Width = (int)(this.Content as Grid).RenderSize.Width;
                renderFrame.RenderTargetChanged += RenderFrame_RenderTargetChanged;
                //renderFrame.DisplayHandle = Handle;
                renderFrame.Initialize();
                TargetImage.Source = imageSource;
                BackgroundColor = Interop.MediaColorToNativeColor((Background as SolidColorBrush).Color);

                Clear = renderFrame.CreateDrawCommnad2D<Clear2DCommand>();
                Clear.clearColor = BackgroundColor;

                SharpDX.Direct2D1.Ellipse ellipse = new SharpDX.Direct2D1.Ellipse(new SharpDX.Mathematics.Interop.RawVector2(100, 100), 50, 50);
                EllipseBrush = renderFrame.Controller.CreateSolidColorBrush(SharpDX.Color.Black);

                DrawEllipse = renderFrame.CreateDrawCommnad2D<DrawEllipse2DCommand>();
                DrawEllipse.ellipse = ellipse;
                DrawEllipse.brush = EllipseBrush;

                FillEllipse = renderFrame.CreateDrawCommnad2D<FillEllipse2DCommand>();
                FillEllipse.ellipse = ellipse;
                FillEllipse.brush = EllipseBrush;

                DrawLine = renderFrame.CreateDrawCommnad2D<DrawLine2DCommand>();
                DrawLine.point0 = new SharpDX.Mathematics.Interop.RawVector2(0, 0);
                DrawLine.point1 = new SharpDX.Mathematics.Interop.RawVector2(100, 100);
                DrawLine.strokeWidth = 10;
                DrawLine.brush = EllipseBrush;

                System.Windows.Media.CompositionTarget.Rendering += OnRendering;
                //dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                //dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 16);
                //dispatcherTimer.Start();
            }));
        }

        private void RenderFrame_RenderTargetChanged(object sender, RenderTargetChanageArgs e)
        {
            imageSource.SetSurface(e.RenderTargetTexture);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            renderFrame.Height = (int)(this.Content as Grid).RenderSize.Height;
            renderFrame.Width = (int)(this.Content as Grid).RenderSize.Width;
        }

        private void OnRendering(object sender, EventArgs e)
        {
            renderFrame.PushDrawCommand(Clear);
            renderFrame.PushDrawCommand(DrawEllipse);
            renderFrame.PushDrawCommand(FillEllipse);
            renderFrame.PushDrawCommand(DrawLine);
            renderFrame.FlushDrawCommand();
            imageSource.Invalidate();
        }


    }
}
