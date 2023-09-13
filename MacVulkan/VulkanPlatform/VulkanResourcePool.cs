using System;

using System.Collections.Concurrent;
using GPUVulkan;
using System.Threading;

namespace VulkanPlatform
{

    /*
     * descriptors essentially describe what is in a storage or frame buffer, while its not necessarily obvious its likely
     * far more efficient to bundle/pool descriptor with what they represent
     * 
    public class ResourcePool
    {
        private VkDevice device;
        private VkDescriptorSetLayout descriptorSetLayout;
        private ConcurrentDictionary<int, VkDescriptorPool> descriptorPools
            = new ConcurrentDictionary<int, VkDescriptorPool>();

        private VkDescriptorPoolSize[] descriptorPoolSizes;

        public ResourcePool(VkDevice device, VkDescriptorSetLayout descriptorSetLayout)
        {
            this.device = device;
            this.descriptorSetLayout = descriptorSetLayout;
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
    */
}

