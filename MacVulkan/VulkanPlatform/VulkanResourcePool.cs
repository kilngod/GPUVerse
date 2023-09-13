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
        private ConcurrentDictionary<int, VulkanResource> vulkanResource
            = new ConcurrentDictionary<int, VulkanResource>();

        private VkDescriptorPoolSize[] _descriptorPoolSizes;

        public VulkanResourcePool(VkDevice device, int maxThreads, VkDescriptorPoolSize[] descriptorPoolSizes)
        {
            this.device = device;
            this.MaxThreads = maxThreads;
            _descriptorPoolSizes = descriptorPoolSizes;
        }



        public VkDescriptorPool GetThreadDescriptorPool()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            return descriptorPools.GetOrAdd(threadId, _ => CreateDescriptorPool(device,));
        }

        

        public VkDescriptorSet AllocateDescriptorSet()
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

