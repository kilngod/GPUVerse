using GPUGraphicsMaui.GPUViews;
using Microsoft.Maui.Graphics;

namespace GPUMauiApp.Pages;

public class GPUViewPage : ContentPage
{
    IGPUView _gpuView;

    public GPUViewPage()
    {
        int widthRequest = 500;
        int heightRequest = 500;
#if ANDROID
        _gpuView = GPUView.GPUVulkan(widthRequest, heightRequest);
#elif IOS || MACCATALYST
        //_gpuView = GPUView.GPUMetal(widthRequest, heightRequest);
        _gpuView = GPUView.GPUVulkan(widthRequest, heightRequest);
#elif WINDOWS
         _gpuView = GPUView.GPUVulkan(widthRequest, heightRequest);
       // _gpuView = GPUView.GPUDirectX(widthRequest, heightRequest);

      //  _gpuView= GPUView.GPUOpenGL(widthRequest, heightRequest);
#endif




        Content =

            new VerticalStackLayout
            {
                Children = {
                    new Label { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Text = "Welcome to .NET MAUI!"
                    },
                    
                    _gpuView


                },
                

            };
        BackgroundColor = Color.FromRgba(0, 0, 0, 0);
        
    }

     
    protected override void OnAppearing()
    {
        base.OnAppearing();

      
    }
}
