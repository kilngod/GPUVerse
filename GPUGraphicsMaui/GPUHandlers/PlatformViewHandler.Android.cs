﻿using GPUGraphicsMaui.GPUViews;
using GPUGraphicsMaui.GPURenderers;
using GPUGraphicsMaui.GPUDevices;
using Microsoft.Maui.Handlers;
using VulkanPlatform;

namespace GPUGraphicsMaui.GPUHandlers
{
    public partial class PlatformViewHandler : ViewHandler<GPUView, PlatformGPUView>
    {
        public PlatformViewHandler(IPropertyMapper mapper, CommandMapper commandMapper = null) : base(mapper, commandMapper)
        {
        }

        

        protected override PlatformGPUView CreatePlatformView()
        {
#if DEBUG
            VulkanFlowTracer.AddItem("PlatformViewHandler.CreatePlatformView");
#endif


            _gpuSupport = new GPUSupportVulkan(VirtualView.Platform, VirtualView.Engine);
            _gpuSupport.RendererCreated += RendererCreated;
            _gpuSupport.RendererRemoved += RendererRemoved;

            var pv = new PlatformGPUView(Context, (_gpuSupport as GPUSupport));

            return pv;
        }



        private void RendererCreated()
        {
#if DEBUG
            VulkanFlowTracer.AddItem("PlatformViewHandler.RendererCreated");
#endif
            if (!_gpuSupport.Renderer.PipelineInitialized)
            {
                _gpuSupport.Renderer.SetupPipeline();
                _gpuSupport.Renderer.Active = true;
            }
            _gpuSupport.Renderer.RequestRender();

        }

        private void RendererRemoved()
        {
#if DEBUG
            VulkanFlowTracer.AddItem("PlatformViewHandler.RendererRemoved");
#endif
            _gpuSupport.Renderer.RemoveFromView();

        }




        protected override void ConnectHandler(PlatformGPUView platformView)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("PlatformViewHandler.ConnectHandler");
#endif

            base.ConnectHandler(platformView);

            platformView.Connect(VirtualView);

            _gpuSupport.AttachPlatformView(PlatformView);

        }

        protected override void DisconnectHandler(PlatformGPUView platformView)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("PlatformViewHandler.DisconnectHandler");
#endif
            platformView.Disconnect();

            // Perform cleanup when disconnecting the handler from the virtualView
            _gpuSupport.ReleasePlatformView();

            base.DisconnectHandler(platformView);


        }


#nullable enable
        public static void MapInvalidate(IGPUViewHandler handler, IGPUView view, object? arg)
        {
            handler.PlatformView?.Invalidate();
        }
#nullable disable

    }
}
