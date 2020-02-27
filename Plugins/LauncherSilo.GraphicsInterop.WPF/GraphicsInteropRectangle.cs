using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LauncherSilo.GraphicsInterop.WPF
{
    public class GraphicsInteropRectangle : GraphicsInteropElement
    {
        DrawRectangle2DCommand DrawRectangle = null;
        public override void OnRender(RenderFrame renderFrame)
        {
            if (DrawRectangle == null)
            {
                DrawRectangle = renderFrame.CreateDrawCommnad2D<DrawRectangle2DCommand>();
                {
                    DrawRectangle.brush = renderFrame.Controller.CreateSolidColorBrush(SharpDX.Color.Black);
                    DrawRectangle.rect = new SharpDX.Mathematics.Interop.RawRectangleF(10, 10, 90, 90);
                    DrawRectangle.strokeWidth = 2;
                };
            }
            renderFrame.PushDrawCommand(DrawRectangle);
        }
    }
}
