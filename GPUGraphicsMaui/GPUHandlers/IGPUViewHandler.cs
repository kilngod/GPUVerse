using GPUGraphicsMaui.GPUViews;



namespace GPUGraphicsMaui.GPUHandlers
{
    public interface IGPUViewHandler : IViewHandler
    {
        new IGPUView VirtualView { get; }
        new PlatformGPUView PlatformView { get; }
    }
}
