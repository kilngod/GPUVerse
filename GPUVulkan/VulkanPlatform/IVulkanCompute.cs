using System;
using GPUVulkan;

namespace VulkanPlatform
{
    public interface IVulkanCompute
    {
        public IVulkanSupport Support { get; }

        VkCommandPool CommandPool { get; set; }
        VkCommandBuffer[] CommandBuffers { get; set; }
        VkPipelineLayout PipelineLayout { get; set; }
        VkPipeline GraphicsPipeline { get; set; }
        VkDescriptorSet ComputeDescriptor {get;set;}
        VkDescriptorSetLayout ComputeLayout { get; set; }
        VkSemaphore ComputeSemaphore { get; set; }
        VkQueue ComputeQueue { get; }
        void SetComputeQueue(VkQueue computeQueue);


    }
}

