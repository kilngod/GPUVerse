using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPUVulkan;
using Microsoft.VisualBasic;

namespace VulkanPlatform
{
#nullable disable
    public static class VulkanComputing
    {


        public static unsafe void CreateComputePipeline(this VkDevice device, ref VkComputePipelineCreateInfo pipelineCreateInfo, ref VkPipeline pipeline, ref VkAllocationCallbacks allocationCallbacks, VkPipelineCache cache = default(VkPipelineCache))
        {
            fixed (VkPipeline* pipelinePtr = &pipeline) 
            {
                fixed (VkComputePipelineCreateInfo* pipelineCreateInfoPtr = &pipelineCreateInfo)
                {
                    fixed (VkAllocationCallbacks* callbacksPtr = &allocationCallbacks)
                    {
                        VulkanNative.vkCreateComputePipelines(device, cache, 1, pipelineCreateInfoPtr, callbacksPtr, pipelinePtr);
                    }
                }
            }
     
        }
    }
}
