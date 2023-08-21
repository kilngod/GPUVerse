﻿using System;
using GPUGraphicsMaui.GPUDevices;
using GPUGraphicsMaui.GPURenderers;
using GPUGraphicsMaui.GPUViews;
using Microsoft.Maui.Handlers;

namespace GPUGraphicsMaui.GPUHandlers
{
    public partial class PlatformViewHandler :
#if IOS || MACCATALYST || WINDOWS || ANDROID
    IGPUViewHandler
#else
    ViewHandler<IGPUView, PlatformGPUView>, IGPUViewHandler
#endif
    {
#nullable disable
        private IGPUSupport _gpuSupport;
#nullable enable


        public static IPropertyMapper<IGPUView, IGPUViewHandler> Mapper = new PropertyMapper<IGPUView, PlatformViewHandler>(ViewHandler.ViewMapper)
        {


        };

        public static CommandMapper<IGPUView, IGPUViewHandler> CommandMapper = new(ViewCommandMapper)
        {
            [nameof(IPlatformGPUView.Invalidate)] = MapInvalidate
        };

        public PlatformViewHandler() : base(Mapper, CommandMapper)
        {
        }



#if IOS || MACCATALYST || WINDOWS || ANDROID
        IGPUView IGPUViewHandler.VirtualView => VirtualView;


        PlatformGPUView IGPUViewHandler.PlatformView => PlatformView;


#else

        IGPUView IGPUViewHandler.VirtualView => VirtualView;

        PlatformGPUView IGPUViewHandler.PlatformView => (PlatformGPUView)PlatformView;



        protected override PlatformGPUView CreatePlatformView()
        {
            throw new NotImplementedException();
        }

        public static void MapInvalidate(IGPUViewHandler handler, IGPUView view, object? arg) { }
       
#endif
    }
}

