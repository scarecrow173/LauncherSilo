using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D11;
using SharpDX.Direct2D1;
using SharpDX.DXGI;

namespace LauncherSilo.GraphicsInterop
{
    public class SwapChainRenderFrame : RenderFrame
    {
        public IntPtr DisplayHandle { get; set; } = IntPtr.Zero;
        public SwapChain SwapChain { get; set; } = null;
        protected override Texture2D CreateRenderTargetTexture()
        {
            if (DisplayHandle == IntPtr.Zero)
            {
                return null;
            }
            var desc = new SwapChainDescription()
            {
                BufferCount = 1,
                ModeDescription = new ModeDescription(Width, Height, new Rational(60, 1), Format),
                IsWindowed = true,
                OutputHandle = DisplayHandle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            };
            SwapChain = _controller.CreateSwapChain(desc);
            return Texture2D.FromSwapChain<Texture2D>(SwapChain, 0);

        }
    }
}
