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

    public abstract class RenderFrame : IDisposable
    {
        public int Width { get; set; } = 0;
        public int Height { get; set; } = 0;
        public SharpDX.Direct2D1.AlphaMode AlphaMode { get; set; } = SharpDX.Direct2D1.AlphaMode.Premultiplied;
        public SharpDX.DXGI.Format Format { get; set; } = Format.B8G8R8A8_UNorm;
        public SharpDX.Direct2D1.AntialiasMode AntialiasMode { get; set; } = SharpDX.Direct2D1.AntialiasMode.PerPrimitive;
        public SharpDX.Direct2D1.TextAntialiasMode TextAntialiasMode { get; set; } = SharpDX.Direct2D1.TextAntialiasMode.Cleartype;

        protected GraphicsController _controller = null;
        protected Texture2D _renderTargetTexture = null;
        protected Surface _renderTargetSurface = null;
        public RenderTarget _renderTarget2D = null;
        protected SharpDX.Direct2D1.Factory _factory2D = null;

        public RenderFrame()
        {
            _controller = new GraphicsController();
            _factory2D = new SharpDX.Direct2D1.Factory();
        }
        public virtual bool Initialize()
        {
            CreateTargets();
            return true;
        }
        public virtual void Resize(int width, int height)
        {
            Width = width;
            Height = height;
            CreateTargets();
        }
        public virtual void CreateTargets()
        {
            if (_controller == null)
            {
                return;
            }
            if (Width == 0 || Height == 0)
            {
                return;
            }
            Utils.SafeDispose(ref _renderTargetTexture);
            Utils.SafeDispose(ref _renderTargetSurface);
            Utils.SafeDispose(ref _renderTarget2D);
            _renderTargetTexture = CreateRenderTargetTexture();
            _renderTargetSurface = _renderTargetTexture.QueryInterface<Surface>();
            var rtp = new RenderTargetProperties(new PixelFormat(Format.Unknown, AlphaMode));
            _renderTarget2D = new RenderTarget(_factory2D, _renderTargetSurface, rtp);
            _renderTarget2D.AntialiasMode = AntialiasMode;
            _renderTarget2D.TextAntialiasMode = TextAntialiasMode;
            _controller.SetViewport(0, 0, Width, Height);
        }
        protected abstract Texture2D CreateRenderTargetTexture();
        

        public bool IsDisposed { get; private set; }
        public void Dispose()
        {
            if (IsDisposed) return;
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            Utils.SafeDispose(ref _renderTargetTexture);
            Utils.SafeDispose(ref _renderTargetSurface);
            Utils.SafeDispose(ref _renderTarget2D);
            Utils.SafeDispose(ref _factory2D);
            Utils.SafeDispose(ref _controller);
            IsDisposed = true;
        }
    }
}
