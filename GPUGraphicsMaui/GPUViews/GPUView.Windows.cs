﻿using System;
#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#else

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
#endif
using Microsoft.Maui.Graphics;

namespace GPUGraphicsMaui.GPUViews
{
	public partial class GPUView : View, IGPUView, IDrawable
    {

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
           throw new NotImplementedException();

        }

        /*
        public static GPUView GPUOpenGL(int requestedWidth, int requestedHeight)
        {
            return new GPUView(GPUEngine.OpenGL, GPUPlatform.Windows)
            {
                WidthRequest = requestedWidth,
                HeightRequest = requestedHeight
            };
        }

        public static GPUView GPUDirectX(int requestedWidth, int requestedHeight)
        {
            return new GPUView(GPUEngine.DirectX, GPUPlatform.Windows)
            {
                WidthRequest = requestedWidth,
                HeightRequest = requestedHeight
            };
        }
        
        */

        public static GPUView GPUVulkan(int requestedWidth, int requestedHeight)
        {
            return new GPUView(GPUEngine.Vulkan, GPUPlatform.Windows)
            {
                WidthRequest = requestedWidth,
                HeightRequest = requestedHeight
                --, ZIndex = 10
            };
        }
     
   
    }
}

