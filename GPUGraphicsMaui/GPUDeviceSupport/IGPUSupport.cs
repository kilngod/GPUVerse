using System;
using GPUGraphicsMaui.GPURenderers;
using GPUGraphicsMaui.GPUViews;

namespace GPUGraphicsMaui.GPUDevices
{
    public interface IGPUSupport : IDisposable
    {
        event Action RendererCreated;
        event Action RendererRemoved;

        public IGPURenderer Renderer { get; }
        public PlatformGPUView PlatformGPUView { get; }
        void AttachPlatformView(PlatformGPUView view);
        void ReleasePlatformView();

     

    }
}

