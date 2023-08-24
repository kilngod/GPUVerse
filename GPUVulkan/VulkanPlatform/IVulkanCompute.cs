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

        /*
         * 
         * 
         // setup for vulkan
          ProbeInstallation();
    CreateInstance();
    RegisterDebugReportCallback();
    GetPhysicalDevice();
    FindQueueFamily();
    CreateLogicalDevice();
    GetQueue();
        // compute
    CreateBuffer();
    AllocateDeviceMemory();
    BindDeviceMemory();
    CreateDescriptorSetLayout();
    CreateDescriptorPool();
    CreateDescriptorSets();
    ConnectBufferWithDescriptorSets();
    CreateShaderModule("shaders/comp.spv");
    CreateComputePipeline();
    CreateCommandPool();
    CreateCommandBuffers();
    FillCommandBuffer();
    SubmitAndWait();
    SaveRenderedImage("mandelbrot.png");

        */
    }
}

