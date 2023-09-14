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

using System.Collections.Concurrent;
using GPUVulkan;
using System.Threading;
using CoreVideo;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace VulkanPlatform
{

    /*
     * descriptors essentially describe what is in a storage or frame buffer, while its not necessarily obvious its likely
     * far more efficient to bundle/pool descriptor with what they represent
     */ 
    public class VulkanResourcePool
    {
        public int MaxThreads { get; private set; } // should be limited to CPU performance cores less 2
        private VkDevice device;
        private VkDescriptorSetLayout descriptorSetLayout;
        private ConcurrentBag<SharedResource> sharedResources;
        private ConcurrentDictionary<int, VulkanResource> vulkanResource
            = new ConcurrentDictionary<int, VulkanResource>();

        private VkDescriptorPoolSize[] _descriptorPoolSizes;

        public VulkanResourcePool(VkDevice device, int maxThreads)
        {
            this.device = device;
            this.MaxThreads = maxThreads;
        }


        public void AddComputeResources(SharedResource sharedResource)
        {

            sharedResources.Add(sharedResource);


            Parallel.For(0, MaxThreads, i =>
            {

                VulkanResource resource = new VulkanResource();

                if (vulkanResource.TryAdd(i, resource))
                {
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
                    this.Support.Device.CreateDescriptorSetLayout(ref layoutCreateInfo, ref _descriptorSetLayout);

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


                    this.Support.Device.CreateDescriptorPool(ref poolCreateInfo, ref _descriptorPool);

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

                        this.Support.Device.AllocateDescriptorSets(ref allocateInfo, ref _descriptorSets[0]);
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
                        descriptorType = VkDescriptorType.VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                        pBufferInfo = &descriptorBufferInfo
                    };


                    this.Support.Device.UpdateDescriptorSet(ref writeDescriptorSet);




                }

            });
        }

          
        }

        public VkDescriptorPool GetThreadDescriptorPool()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            return descriptorPools.GetOrAdd(threadId, _ => CreateDescriptorPool(device,));
        }

        

        public VkDescriptorSet AllocateVulkanResources()
        {
            VkDescriptorPool descriptorPool = GetThreadDescriptorPool();

            VkDescriptorSetAllocateInfo descriptorSetAllocateInfo = new VkDescriptorSetAllocateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_DESCRIPTOR_SET_ALLOCATE_INFO,
                descriptorPool = descriptorPool,
                descriptorSetCount = 1,
                pSetLayouts = new VkDescriptorSetLayout[] { descriptorSetLayout }
            };

            vkAllocateDescriptorSets(device, ref descriptorSetAllocateInfo, out VkDescriptorSet descriptorSet);
            return descriptorSet;
        }

        public void UpdateDescriptorSet(VkDescriptorSet descriptorSet, VkBuffer buffer, ulong offset, ulong range)
        {
            VkDescriptorBufferInfo bufferInfo = new VkDescriptorBufferInfo
            {
                buffer = buffer,
                offset = offset,
                range = range
            };

            VkWriteDescriptorSet writeDescriptorSet = new VkWriteDescriptorSet
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_WRITE_DESCRIPTOR_SET,
                dstSet = descriptorSet,
                dstBinding = 0,
                dstArrayElement = 0,
                descriptorCount = 1,
                descriptorType = VkDescriptorType.VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                pBufferInfo = new VkDescriptorBufferInfo[] { bufferInfo }
            };

            vkUpdateDescriptorSets(device, 1, ref writeDescriptorSet, 0, null);
        }
    }
    
}

