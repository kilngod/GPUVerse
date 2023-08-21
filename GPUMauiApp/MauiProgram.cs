using Microsoft.Extensions.Logging;

namespace GPUMauiApp;
using GPUGraphicsMaui.GPUViews;

using GPUViewHandler = GPUGraphicsMaui.GPUHandlers.PlatformViewHandler;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
             .ConfigureMauiHandlers(handlers =>
             {
                 handlers.AddHandler<GPUView, GPUViewHandler>();
             })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}