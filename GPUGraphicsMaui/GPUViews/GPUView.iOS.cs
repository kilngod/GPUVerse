// ---------------------------------------------------------------------------------------
//                                        ILGPU
//                           Copyright (c) 2023 ILGPU Project
//                                    www.ilgpu.net
//
// File: .cs
//
// This file is part of ILGPU and is distributed under the University of Illinois Open
// Source License. See LICENSE.txt for details.
// ---------------------------------------------------------------------------------------

using System;
using VulkanPlatform;

namespace GPUGraphicsMaui.GPUViews
{
	public partial class GPUView
	{
        public static GPUView GPUMetal(int requestedWidth, int requestedHeight)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("GPUView.GPUMetal");
#endif
            return new GPUView(GPUEngine.Metal, GPUPlatform.iOS)
            {
                WidthRequest = requestedWidth,
                HeightRequest = requestedHeight
            };


        }

        public static GPUView GPUVulkan(int requestedWidth, int requestedHeight)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("GPUView.GPUVulkan");
#endif
            return new GPUView(GPUEngine.Vulkan, GPUPlatform.iOS)
            {
                WidthRequest = requestedWidth,
                HeightRequest = requestedHeight
            };
        }
    }
}

