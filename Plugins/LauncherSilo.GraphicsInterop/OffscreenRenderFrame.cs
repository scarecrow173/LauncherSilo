using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace LauncherSilo.GraphicsInterop
{
    public class OffscreenRenderFrame : RenderFrame
    {
        protected override Texture2D CreateRenderTargetTexture()
        {
            var renderDesc = new Texture2DDescription
            {
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                Format = Format,
                Width = Width,
                Height = Height,
                MipLevels = 1,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default,
                OptionFlags = ResourceOptionFlags.Shared,
                CpuAccessFlags = CpuAccessFlags.None,
                ArraySize = 1
            };
            return _controller.CreateTexture2D(renderDesc);
        }
    }
}
