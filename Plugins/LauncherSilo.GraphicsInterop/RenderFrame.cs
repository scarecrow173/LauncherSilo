using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D11;
using SharpDX.Direct2D1;
using SharpDX.DXGI;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;

namespace LauncherSilo.GraphicsInterop
{
    public class RenderTargetChanageArgs : EventArgs
    {
        protected RenderTarget RenderTarget { get; private set; }
        public Texture2D RenderTargetTexture { get; private set; }
        public Surface RenderTargetSurface { get; private set; }
        public RenderTargetChanageArgs(RenderTarget renderTarget, Texture2D renderTargetTexture, Surface renderTargetSurface)
        {
            RenderTarget = renderTarget;
            RenderTargetTexture = renderTargetTexture;
            RenderTargetSurface = renderTargetSurface;
        }
    }
    public abstract class RenderFrame : IDisposable
    {
        public int Width
        {
            get => _Width;
            set
            {
                if (_Width != value)
                {
                    _Width = value;
                    IsDirtyRenderTarget = true;
                }
            }
        }
        private int _Width = 0;
        public int Height
        {
            get => _Height;
            set
            {
                if (_Height != value)
                {
                    _Height = value;
                    IsDirtyRenderTarget = true;
                }
            }
        }
        private int _Height = 0;
        public SharpDX.Direct2D1.AlphaMode AlphaMode
        {
            get => _AlphaMode;
            set
            {
                if (_AlphaMode != value)
                {
                    _AlphaMode = value;
                    IsDirtyRenderTarget = true;
                }
            }
        }
        private SharpDX.Direct2D1.AlphaMode _AlphaMode = SharpDX.Direct2D1.AlphaMode.Premultiplied;
        public SharpDX.DXGI.Format Format
        {
            get => _Format;
            set
            {
                if (_Format != value)
                {
                    _Format = value;
                    IsDirtyRenderTarget = true;
                }
            }
        }
        private SharpDX.DXGI.Format _Format = Format.B8G8R8A8_UNorm;
        public SharpDX.Direct2D1.AntialiasMode AntialiasMode
        {
            get => _AntialiasMode;
            set
            {
                if (_AntialiasMode != value)
                {
                    _AntialiasMode = value;
                    IsDirtyRenderTarget = true;
                }
            }
        }
        private SharpDX.Direct2D1.AntialiasMode _AntialiasMode = SharpDX.Direct2D1.AntialiasMode.PerPrimitive;
        public SharpDX.Direct2D1.TextAntialiasMode TextAntialiasMode
        {
            get => _TextAntialiasMode;
            set
            {
                if (_TextAntialiasMode != value)
                {
                    _TextAntialiasMode = value;
                    IsDirtyRenderTarget = true;
                }
            }
        }
        private SharpDX.Direct2D1.TextAntialiasMode _TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode.Cleartype;

        public bool IsDirtyRenderTarget { get; set; } = false;

        public GraphicsController Controller
        {
            get => _Controller;
            set => _Controller = value;
        }
        private GraphicsController _Controller = null;

        public event EventHandler<RenderTargetChanageArgs> RenderTargetChanged;

        protected Texture2D _renderTargetTexture = null;
        protected Surface _renderTargetSurface = null;
        protected RenderTarget _renderTarget2D = null;
        protected SharpDX.Direct2D1.Factory _factory2D = null;
        protected Queue<IDrawCommand> _drawCommandQueue = null;

        public RenderFrame()
        {
            _Controller = new GraphicsController();
            _factory2D = new SharpDX.Direct2D1.Factory();
            _drawCommandQueue = new Queue<IDrawCommand>();
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
            if (_Controller == null)
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
            _Controller.SetRenderTarget(_renderTarget2D);
            _Controller.SetViewport(0, 0, Width, Height);
            RenderTargetChanged?.Invoke(this, new RenderTargetChanageArgs(_renderTarget2D, _renderTargetTexture, _renderTargetSurface));
            IsDirtyRenderTarget = false;
        }

        public void PushDrawCommand(IDrawCommand drawCommand)
        {
            _drawCommandQueue.Enqueue(drawCommand);
        }
        public virtual void FlushDrawCommand()
        {
            if (IsDirtyRenderTarget)
            {
                CreateTargets();
                IsDirtyRenderTarget = false;
            }
            _Controller.BeginDraw();
            while(_drawCommandQueue.Count != 0)
            {
                IDrawCommand drawCommand = _drawCommandQueue.Dequeue();
                drawCommand.Draw();
            }
            _Controller.EndDraw();
        }
        public T CreateDrawCommnad2D<T>() where T : DrawCommand2D, new()
        {
            T Instance = new T();
            Instance.Context2D = _Controller;
            return Instance;
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
            Utils.SafeDispose(ref _Controller);
            IsDisposed = true;
        }
    }
}
