using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LauncherSilo.GraphicsInterop.WPF
{
    public class GraphicsInteropEllipse : GraphicsInteropElement
    {
        DrawEllipse2DCommand DrawEllipse = null;

        public override void OnRender(RenderFrame renderFrame)
        {
            if (DrawEllipse == null)
            {
                DrawEllipse = renderFrame.CreateDrawCommnad2D<DrawEllipse2DCommand>();
                {
                    DrawEllipse.brush = renderFrame.Controller.CreateSolidColorBrush(SharpDX.Color.Black);
                    DrawEllipse.ellipse = new SharpDX.Direct2D1.Ellipse(new SharpDX.Mathematics.Interop.RawVector2(50, 50), 50, 50);
                    DrawEllipse.strokeWidth = 2;
                };
            }
            renderFrame.PushDrawCommand(DrawEllipse);
        }
    }
}
