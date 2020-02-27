using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LauncherSilo.GraphicsInterop.WPF
{
    public class GraphicsInteropLine : GraphicsInteropElement
    {
        DrawLine2DCommand DrawLine = null;
        public override void OnRender(RenderFrame renderFrame)
        {
            if (DrawLine == null)
            {
                DrawLine = renderFrame.CreateDrawCommnad2D<DrawLine2DCommand>();
                {
                    DrawLine.brush = renderFrame.Controller.CreateSolidColorBrush(SharpDX.Color.Black);
                    DrawLine.point0 = new SharpDX.Mathematics.Interop.RawVector2(0, 0);
                    DrawLine.point1 = new SharpDX.Mathematics.Interop.RawVector2(100, 100);
                    DrawLine.strokeWidth = 2;
                };
            }
            renderFrame.PushDrawCommand(DrawLine);
        }
    }
}
