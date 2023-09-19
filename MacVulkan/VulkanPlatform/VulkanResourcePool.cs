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
using System.Linq;

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
        private ConcurrentBag<VulkanResourceElement> sharedResources;
        private ConcurrentDictionary<int, VulkanResourceThread> vulkanResource
            = new ConcurrentDictionary<int, VulkanResourceThread>();


        public VulkanResourcePool(VkDevice device, int maxThreads)
        {
            this.device = device;
            this.MaxThreads = maxThreads;
        }


        public unsafe void AddComputeResources(VulkanResourceElement sharedResource)
        {

            sharedResources.Add(sharedResource);

            uint totalDescriptors = 0;
            

            for(int iSegment = 0; iSegment < sharedResource.Segments.Length; iSegment++)
            {
                totalDescriptors += sharedResource.Segments[iSegment].DescriptorPoolSize.descriptorCount;
            }

            VkDescriptorPoolSize[] DescriptorPoolSizes = sharedResource.Segments.Select(x => x.DescriptorPoolSize).ToArray(); 

            Parallel.For(0, MaxThreads, i =>
            {

                VulkanResourceThread resource = new VulkanResourceThread();


                if (vulkanResource.TryAdd(i, resource))
                {


                    VkDescriptorSetLayoutBinding layoutBinding = new VkDescriptorSetLayoutBinding()
                    {
                        descriptorType = VkDescriptorType.VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                        descriptorCount = totalDescriptors,
                        stageFlags = VkShaderStageFlags.VK_SHADER_STAGE_COMPUTE_BIT
                    };

                    VkDescriptorSetLayoutCreateInfo layoutCreateInfo = new VkDescriptorSetLayoutCreateInfo()
                    {
                        sType = VkStructureType.VK_STRUCTURE_TYPE_DESCRIPTOR_SET_LAYOUT_CREATE_INFO,
                        bindingCount = 1,
                        pBindings = &layoutBinding
                    };

                    device.CreateDescriptorSetLayout(ref layoutCreateInfo, ref resource.DescriptorSetLayout);

                    fixed (VkDescriptorPoolSize* poolPtr = &DescriptorPoolSizes[0])
                    {

                        VkDescriptorPoolCreateInfo poolCreateInfo = new VkDescriptorPoolCreateInfo()
                        {
                            sType = VkStructureType.VK_STRUCTURE_TYPE_DESCRIPTOR_POOL_CREATE_INFO,
                            poolSizeCount = (uint)sharedResource.Segments.Length,
                            pPoolSizes = poolPtr,
                            maxSets = totalDescriptors
                        };

                        device.CreateDescriptorPool(ref poolCreateInfo, ref resource.DescriptorPool);

                    }


                    resource.DescriptorSets = new VkDescriptorSet[totalDescriptors];

                    // descriptor sets
                    VkDescriptorSetAllocateInfo allocateInfo = new VkDescriptorSetAllocateInfo()
                    {
                        sType = VkStructureType.VK_STRUCTURE_TYPE_DESCRIPTOR_SET_ALLOCATE_INFO,
                        descriptorPool = resource.DescriptorPool,
                        descriptorSetCount = totalDescriptors,
                        pSetLayouts = &resource.DescriptorSetLayout
                    };

                    device.AllocateDescriptorSets(ref allocateInfo, ref resource.DescriptorSets[0]);

                    uint iStart = 0;

                    for (int i = 0; i < sharedResource.Segments.Length; i++)
                    {

                        fixed (VkDescriptorBufferInfo* bufferInfoPtr = &sharedResource.Segments[i].DescriptorBufferSegmentInfos[0])
                        {

                            VkWriteDescriptorSet writeDescriptorSet = new VkWriteDescriptorSet()
                            {
                                dstSet = resource.DescriptorSets[iStart],
                                descriptorCount = (uint)resource.DescriptorSets.Length,
                                dstBinding = sharedResource.Segments[i].BindingPoint,
                                descriptorType = VkDescriptorType.VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                                pBufferInfo = bufferInfoPtr
                            };

                            iStart += (uint) sharedResource.Segments[i].DescriptorBufferSegmentInfos.Length;

                            device.UpdateDescriptorSet(ref writeDescriptorSet);
                        }

                    }




                }

            });
        }


    }



}
    


