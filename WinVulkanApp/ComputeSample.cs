using System;
using System.Windows.Forms;
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


        bool _unifiedMemory = false;

        public bool UnifiedMemory { get; }

        int _computeFamilyIndex = -1;
        public int ComputeCommandBuffers { get; set; } = 1;
        public int ComputeFamilyIndex { get { return _computeFamilyIndex; } }

        private VkQueue _computeQueue;
        public VkQueue ComputeQueue { get { return _computeQueue; } }

        public IVulkanSupport Support { get { return _support; } }

        public VkCommandPool CommandPool { get; set; }
        public VkCommandBuffer[] CommandBuffers { get; set; }
        public VkPipelineLayout PipelineLayout { get { return _pipelineLayout; } }

     
        public uint ComputeDescriptorSets { get; set; } = 1;

        VkDescriptorSet[] _descriptorSets;
        public VkDescriptorSet[] DescriptorSets { get { return _descriptorSets; } }
        public VkDescriptorSetLayout ComputeLayout { get; set; }
        public VkSemaphore ComputeSemaphore { get; set; }

      

        public VkPipeline ComputePipeline { get { return _computePipeline; } }

        private VkBuffer _buffer;
        private VkDeviceMemory _deviceMemory;

        // mandlebrot information
        
        const uint kWidth = 3200;
        const uint kHeight = 2400;
        uint kWorkgroupSize = 32;
        ulong buffer_size;

        public static byte[] LoadRawResource(string ResourceFilePath)
        {
            byte[] byteResult;

            using (FileStream rawStream = new FileStream(ResourceFilePath, FileMode.Open))
            {

                // MemoryStream apparently corrects corruption issue with using Seek on MacCatalyst or Android
                MemoryStream stream = new MemoryStream();
                rawStream.CopyTo(stream);
                byteResult = stream.ToArray();



                rawStream.Close();
            }

            return byteResult;

        }


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
            VulkanSpirV computeV = new VulkanSpirV() { EntryName = "main", Name = "Compute", ShaderStageType = VkShaderStageFlags.VK_SHADER_STAGE_COMPUTE_BIT, SpirVByte = LoadRawResource(resourceFolder + "comp.spv") };
            computeShaderList.Add(computeV);

            CreatePipeline(computeShaderList);

           
            

        }

        public unsafe void Compute()
        {
            this.CreateCommandPool();

            this.CreateCommandBuffers();

            if (kWorkgroupSize > Support.DeviceProperties.limits.maxComputeWorkGroupSize_0)
            {
                kWorkgroupSize = Support.DeviceProperties.limits.maxComputeWorkGroupSize_0;
            }

            this.FillCommandBuffer(kWidth / kWorkgroupSize, kHeight / kWorkgroupSize, 1);

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
            Support.AllocateMemory(ref _buffer, ref _deviceMemory, ref _unifiedMemory);

            // bind memory
            Support.BindDeviceMemory(ref _buffer, ref _deviceMemory, 0);
        }



        VkDescriptorSetLayout _descriptorSetLayout = default(VkDescriptorSetLayout);
        VkDescriptorPool _descriptorPool = default(VkDescriptorPool);

        //https://vkguide.dev/docs/extra-chapter/abstracting_descriptors/
        // "Creating and managing descriptor sets is one of the most painful things about Vulkan"
        // managing descriptor sets falls under the label of future research.
        private unsafe void CreateDescriptors()
        {
            // descriptor binding
            VkDescriptorSetLayoutBinding layoutBinding = new VkDescriptorSetLayoutBinding()
            {   
                descriptorType = VkDescriptorType.VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                descriptorCount = ComputeDescriptorSets,
                stageFlags = VkShaderStageFlags.VK_SHADER_STAGE_COMPUTE_BIT
            };

            VkDescriptorSetLayoutCreateInfo layoutCreateInfo = new VkDescriptorSetLayoutCreateInfo() 
            { 
                sType = VkStructureType.VK_STRUCTURE_TYPE_DESCRIPTOR_SET_LAYOUT_CREATE_INFO,
                bindingCount = 1, 
                pBindings = &layoutBinding 
            };
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
                pPoolSizes = &descriptorPoolSize,
                maxSets = 1
            };

           
            this.CreateDescriptorPool(ref poolCreateInfo, ref _descriptorPool);

            fixed (VkDescriptorSetLayout* layoutPtr = &_descriptorSetLayout)
            {
                _descriptorSets = new VkDescriptorSet[ComputeDescriptorSets];

                // descriptor sets
                VkDescriptorSetAllocateInfo allocateInfo = new VkDescriptorSetAllocateInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_DESCRIPTOR_SET_ALLOCATE_INFO,
                    descriptorPool = _descriptorPool,
                    descriptorSetCount = ComputeDescriptorSets,
                    pSetLayouts = layoutPtr
                };
            
                this.AllocateDescriptorSets(ref allocateInfo, ref _descriptorSets[0]);
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
                dstSet = _descriptorSets[0],
                descriptorCount = (uint)_descriptorSets.Length,
                dstBinding = 0,
                descriptorType =  VkDescriptorType.VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                pBufferInfo = &descriptorBufferInfo
            };

      
            this.UpdateDescriptorSet(ref writeDescriptorSet);

            
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

                /*
                VkPipelineCacheCreateInfo cacheCreateInfo = new VkPipelineCacheCreateInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_CACHE_CREATE_INFO,
                    
                }
                VkPipelineCache
                */



                Support.Device.CreatePipelineLayout(ref pipelineLayoutInfo, ref _pipelineLayout);

                VkComputePipelineCreateInfo pipelineCreateInfo = new VkComputePipelineCreateInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_COMPUTE_PIPELINE_CREATE_INFO,
                    layout = _pipelineLayout,
                    stage = stageInfo[0]                    
                    
                };

              
                
                Support.Device.CreateComputePipeline(ref pipelineCreateInfo, ref _computePipeline);
            }
        }
           
        
    }

    struct Pixel
    {
        float r, g, b, a;
    };
}

