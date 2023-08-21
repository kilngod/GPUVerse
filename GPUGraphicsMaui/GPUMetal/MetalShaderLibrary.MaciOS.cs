using System;
using GPUGraphicsMaui.GPUDevices;
using Metal;
using VulkanPlatform;

namespace GPUGraphicsMaui.GPUMetal
{
	public static class MetalShaderLibrary
	{

        public static void AddDefaultLibrary(this GPUSupportMetal support)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("MetalShaderLibrary.AddDefaultLibrary");
#endif

            support.MetalDevice.CreateDefaultLibrary();
            
        }

        public static async Task<bool> AddLibraryKernels(this GPUSupportMetal support, string name, string kernelsString)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("MetalShaderLibrary.AddLibraryKernels");
#endif

            // add async

            MTLCompileOptions mTLCompileOptions = new MTLCompileOptions()
            {
                LibraryType = MTLLibraryType.Executable
            };


            support.MetalLibrary = await support.MetalDevice.CreateLibraryAsync(kernelsString, mTLCompileOptions);

            return support.MetalLibrary != null;


        }


    }
}

