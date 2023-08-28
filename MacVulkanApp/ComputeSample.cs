using System;
using System.Collections.Generic;
using GPUVulkan;
using VulkanPlatform;

namespace WinVulkanApp
{
	public class ComputeSample:IVulkanCompute
	{
        IVulkanSupport _support;

		public ComputeSample(IVulkanSupport support)
		{
            _support = support;
		}


        int _computeFamilyIndex = -1;
        public int ComputeCommandBuffers { get; set; } = 1;
        public int ComputeFamilyIndex { get { return _computeFamilyIndex; } }

        private VkQueue _computeQueue;
        public VkQueue ComputeQueue { get { return _computeQueue; } }

        public IVulkanSupport Support { get { return _support; } }

        public VkCommandPool CommandPool { get; set; }
        public VkCommandBuffer[] CommandBuffers { get; set; }
        public VkPipelineLayout PipelineLayout { get; set; }

        public VkPipeline Pipeline { get; set; }
        public VkDescriptorSet[] ComputeDescriptors { get; set; }
        public VkDescriptorSetLayout ComputeLayout { get; set; }
        public VkSemaphore ComputeSemaphore { get; set; }

      

        public VkPipeline ComputePipeline { get; set; }

        private VkBuffer _buffer;
        private VkDeviceMemory _deviceMemory;

        // mandlebrot information
        
        const int kWidth = 3200;
        const int kHeight = 2400;
        const int kWorkgroupSize = 32;
        ulong buffer_size;

        public void SaveRenderedImage(string fileNamePath)
        {

        }

        public void SetupComputePipeline()
        {
            GetComputeQueue();
            // compute
           
            CreateBuffersAndMemory();
            
         

            CreateDescriptors();
            List<VulkanSpirV> computeShaderList = new List<VulkanSpirV>();
            string resourceFolder = AppContext.BaseDirectory + "\\Shaders\\";
            VulkanSpirV computeV = new VulkanSpirV() { EntryName = "main", Name = "Compute", ShaderStageType = VkShaderStageFlags.VK_SHADER_STAGE_COMPUTE_BIT, SpirVByte = MacIO.LoadRawResource(resourceFolder + "comp.spv") };
            computeShaderList.Add(computeV);

            CreatePipeline(computeShaderList);

        }

        private unsafe void Compute()
        {
            this.CreateCommandPool();
            this.CreateCommandBuffers();
            
            this.FillCommandBuffer( kWidth / kWorkgroupSize, kHeight / kWorkgroupSize, 1);

            fixed (VkCommandBuffer* commandBuffersPtr = &CommandBuffers[0])
            {
                VkSubmitInfo submitInfo = new VkSubmitInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_SUBMIT_INFO,
                    commandBufferCount = (uint)ComputeCommandBuffers,
                    pCommandBuffers = commandBuffersPtr
                };
                this.SubmitAndWait(new VkSubmitInfo[] { submitInfo });
            }
            SaveRenderedImage("mandelbrot.png");
        }

        public void SaveRenderedImage(string ImagePathAndName)
        {

        }



        private void GetComputeQueue()
        {

            _computeFamilyIndex = (int) Support.PhysicalDevice.FindQueueFamilyIndex(VkQueueFlags.VK_QUEUE_COMPUTE_BIT);

            Support.Device.GetQueue(ComputeFamilyIndex, 0, ref _computeQueue);
        }

        private unsafe void CreateBuffersAndMemory()
        {
            // size pixel map
            buffer_size = (ulong)sizeof(Pixel) * kWidth * kHeight;
            _buffer = default(VkBuffer);
            Support.CreateBuffer(buffer_size, ref _buffer);
            
            //allocate memory
            _deviceMemory = default(VkDeviceMemory);
            Support.AllocateMemory(ref _buffer, ref _deviceMemory);

            // bind memory
            Support.BindDeviceMemory(ref _buffer, ref _deviceMemory, 0);
        }



        VkDescriptorSetLayout _descriptorSetLayout = default(VkDescriptorSetLayout);
        VkDescriptorPool _descriptorPool = default(VkDescriptorPool);
        VkDescriptorSet _descriptorSet = default(VkDescriptorSet);
        private unsafe void CreateDescriptors()
        {
            // descriptor binding
            VkDescriptorSetLayoutBinding binding = new VkDescriptorSetLayoutBinding()
            {                
                descriptorType = VkDescriptorType.VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                descriptorCount = 1,
                stageFlags = VkShaderStageFlags.VK_SHADER_STAGE_COMPUTE_BIT
            };

            VkDescriptorSetLayoutCreateInfo layoutCreateInfo = new VkDescriptorSetLayoutCreateInfo() { bindingCount = 1, pBindings = &binding };
            this.CreateDescriptorSetLayout(ref layoutCreateInfo, ref _descriptorSetLayout);

            // descriptor pool
            VkDescriptorPoolSize descriptorPoolSize = new VkDescriptorPoolSize() 
            { 
                descriptorCount = 1, 
                type = VkDescriptorType.VK_DESCRIPTOR_TYPE_STORAGE_BUFFER 
            };
            VkDescriptorPoolCreateInfo poolCreateInfo = new VkDescriptorPoolCreateInfo()
            { 
                sType = VkStructureType.VK_STRUCTURE_TYPE_DESCRIPTOR_POOL_CREATE_INFO,
                poolSizeCount = 1,
                pPoolSizes = &descriptorPoolSize
            };

           
            this.CreateDescriptorPool(ref poolCreateInfo, ref _descriptorPool);

            fixed (VkDescriptorSetLayout* layoutPtr = &_descriptorSetLayout)
            {
                // descriptor sets
                VkDescriptorSetAllocateInfo allocateInfo = new VkDescriptorSetAllocateInfo()
                {
                    descriptorPool = _descriptorPool,
                    descriptorSetCount = 1,
                    pSetLayouts = layoutPtr
                };
            
                this.AllocateDescriptorSets(ref allocateInfo, ref _descriptorSet);
            }

            // connect buffer to descriptor sets
            VkDescriptorBufferInfo descriptorBufferInfo = new VkDescriptorBufferInfo()
            {
                buffer = _buffer,
                offset = 0,
                range = buffer_size
            };
            VkWriteDescriptorSet writeDescriptorSet = new VkWriteDescriptorSet()
            {
                dstSet = _descriptorSet,
                descriptorCount = 1,
                dstBinding = 0,
                descriptorType =  VkDescriptorType.VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                pBufferInfo = &descriptorBufferInfo
            };

            VkCopyDescriptorSet copyDescriptorSet = default(VkCopyDescriptorSet);
            this.UpdateDescriptorSet(ref writeDescriptorSet, ref copyDescriptorSet);

            
        }


        VkPipelineLayout _pipelineLayout = default(VkPipelineLayout);
        VkPipeline _computePipeline = default(VkPipeline);
        public unsafe void CreatePipeline(List<VulkanSpirV> shaderSource)
        {

            VkShaderModule[] shaderModules = new VkShaderModule[shaderSource.Count];
            VkPipelineShaderStageCreateInfo[] stageInfo = new VkPipelineShaderStageCreateInfo[shaderSource.Count];

            for (int i = 0; i < shaderSource.Count; i++)
            {
                shaderModules[i] = this.Support.Device.CreateShaderModule(shaderSource[i].SpirVByte);
                stageInfo[i] = new VkPipelineShaderStageCreateInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_SHADER_STAGE_CREATE_INFO,
                    stage = shaderSource[i].ShaderStageType,
                    module = shaderModules[i],
                    pName = shaderSource[i].EntryName.ToPointer(),
                };

            }




            fixed (VkDescriptorSetLayout* layout = &_descriptorSetLayout)
            {
                VkPipelineLayoutCreateInfo pipelineLayoutInfo = new VkPipelineLayoutCreateInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_COMPUTE_PIPELINE_CREATE_INFO,
                    setLayoutCount = 1,
                    pSetLayouts = layout
                };

                Support.Device.CreatePipelineLayout(ref pipelineLayoutInfo, ref _pipelineLayout);

                VkComputePipelineCreateInfo pipelineCreateInfo = new VkComputePipelineCreateInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_COMPUTE_PIPELINE_CREATE_INFO,
                    layout = _pipelineLayout,
                    stage = stageInfo[0]

                };

                VkAllocationCallbacks allocationCallbacks = default( VkAllocationCallbacks );
                
                Support.Device.CreateComputePipeline(ref pipelineCreateInfo, ref _computePipeline, ref allocationCallbacks);
            }
        }
           
        
    }

    struct Pixel
    {
        float r, g, b, a;
    };
}

