using System;
using GPUVulkan;
using VulkanPlatform;

namespace MacVulkanApp
{
	public class ComputeSample:IVulkanCompute
	{
        VulkanSupport _support;

		public ComputeSample(IVulkanSupport support)
		{
            _support = support as VulkanSupport;
		}

        private VkQueue _computeQueue;

        public IVulkanSupport Support { get { return _support; } }

        public VkCommandPool CommandPool { get; set; }
        public VkCommandBuffer[] CommandBuffers { get; set; }
        public VkPipelineLayout PipelineLayout { get; set; }
        public VkDescriptorSet ComputeDescriptor { get; set; }
        public VkDescriptorSetLayout ComputeLayout { get; set; }
        public VkSemaphore ComputeSemaphore { get; set; }

        public VkQueue ComputeQueue => throw new NotImplementedException();

        public VkPipeline ComputePipeline { get; set; }

   

        public void SetupComputePipeline()
        {
            GetComputeQueue();
            // compute
            /*
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

        private void GetComputeQueue()
        {
            

        }
    }
}

