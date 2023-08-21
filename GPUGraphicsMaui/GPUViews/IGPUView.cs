using Microsoft.Maui.Controls;

using GPUGraphicsMaui.GPUHandlers;


namespace GPUGraphicsMaui.GPUViews
{
    public interface IGPUView : IView
    {
        GPUEngine Engine { get; }
        GPUPlatform Platform { get; }

        void Invalidate();
    }
}
