using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.Mathematics.Interop;

namespace LauncherSilo.GraphicsInterop
{
    public class  GraphicsController : IDisposable
    {
        private Device Dx11Device = null;

        public GraphicsController()
        {
            Dx11Device = new Device(DriverType.Hardware, DeviceCreationFlags.BgraSupport);
        }
        ~GraphicsController()
        {
            Dispose(false);
        }
        public SharpDX.DXGI.SwapChain CreateSwapChain(SharpDX.DXGI.SwapChainDescription desc)
        {
            using (var factory = new SharpDX.DXGI.Factory1())
            {
                return new SharpDX.DXGI.SwapChain(factory, Dx11Device, desc);
            }
        }
        public Texture1D CreateTexture1D(Texture1DDescription desc)
        {
            return new Texture1D(Dx11Device, desc);
        }
        public Texture2D CreateTexture2D(Texture2DDescription desc)
        {
            return new Texture2D(Dx11Device, desc);
        }
        public Texture3D CreateTexture3D(Texture3DDescription desc)
        {
            return new Texture3D(Dx11Device, desc);
        }
        public void SetScissorRectangle(int left, int top, int right, int bottom)
        {
            Dx11Device?.ImmediateContext.Rasterizer.SetScissorRectangle(left, top, right, bottom);
        }
        public void SetViewport(float x, float y, float width, float height, float minZ = 0, float maxZ = 1)
        {
            Dx11Device?.ImmediateContext.Rasterizer.SetViewport(x, y, width, height, minZ, maxZ);
        }
        public void SetViewport(RawViewportF viewport)
        {
            Dx11Device?.ImmediateContext.Rasterizer.SetViewport(viewport);
        }
        public void SetViewports(RawViewportF[] viewports, int count = 0)
        {
            Dx11Device?.ImmediateContext.Rasterizer.SetViewports(viewports, count);
        }
        public unsafe void SetViewports(RawViewportF* viewports, int count = 0)
        {
            Dx11Device?.ImmediateContext.Rasterizer.SetViewports(viewports, count);
        }

        public bool IsDisposed { get; private set; }
        public void Dispose()
        {
            if (IsDisposed) return;
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            Utils.SafeDispose(ref Dx11Device);
            IsDisposed = true;
        }


    }
}
