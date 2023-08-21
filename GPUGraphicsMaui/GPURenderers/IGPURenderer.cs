using System;
using GPUGraphicsMaui.GPUViews;

#if IOS || MACCATALYST
using Metal;
using MetalKit;
#endif

namespace GPUGraphicsMaui.GPURenderers
{
	public interface IGPURenderer : IDisposable
	{
        bool Active { get; set; }

        bool PipelineInitialized { get; set; }

		void SetupPipeline();

        void RequestRender();

#if IOS || MACCATALYST
        void Draw(MTKView view);
#else
        void Draw();
#endif
        void AddToView(PlatformGPUView platformGPUView);

        void RemoveFromView();

        void SizeChanged(float width, float height);
        

    }
}

