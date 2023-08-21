using System;
namespace GPUGraphicsMaui.GPUViews
{
	public partial class GPUView : View, IGPUView
	{
		/*
		public static GPUView GPUOpenGL(int requestedWidth, int requestedHeight)
		{
			return new GPUView(GPUEngine.OpenGL, GPUPlatform.Android)
            {
                WidthRequest = requestedWidth,
                HeightRequest = requestedHeight
            };
		}
		*/
		public static GPUView GPUVulkan(int requestedWidth, int requestedHeight)
		{
			return new GPUView(GPUEngine.Vulkan, GPUPlatform.Android)
			{
				WidthRequest=requestedWidth,
				HeightRequest=requestedHeight
			};
		}


	}
}

