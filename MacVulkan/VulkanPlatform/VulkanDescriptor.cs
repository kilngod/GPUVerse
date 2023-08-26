using System;
using GPUVulkan;

namespace VulkanPlatform
{
	public static class VulkanDescriptor
	{

        

		public unsafe static void CreateDescriptorSetLayout(this IVulkanCompute compute, ref VkDescriptorSetLayoutCreateInfo createInfo, ref VkDescriptorSetLayout layout)
        {
            fixed (VkDescriptorSetLayoutCreateInfo* createInfoPtr = &createInfo)
            {
                fixed (VkDescriptorSetLayout* layoutPtr = &layout)
                {
                    VulkanHelpers.CheckErrors(VulkanNative.vkCreateDescriptorSetLayout(compute.Support.Device, createInfoPtr, null, layoutPtr));
                }
            }
        }

        public unsafe static void CreateDescriptorPool(this IVulkanCompute compute, ref VkDescriptorPoolCreateInfo poolCreateInfo, ref VkDescriptorPool pool)
        {
            fixed (VkDescriptorPool* poolPtr = &pool)
            {
                fixed (VkDescriptorPoolCreateInfo* poolInfoPtr = &poolCreateInfo)
                {
                    VulkanHelpers.CheckErrors(VulkanNative.vkCreateDescriptorPool(compute.Support.Device, poolInfoPtr, null, poolPtr));
                }
            }
        }

		public unsafe static void AllocateDescriptorSets(this IVulkanCompute compute, ref VkDescriptorSetAllocateInfo allocInfo, ref VkDescriptorSet descriptorSet)
		{
			fixed (VkDescriptorSetAllocateInfo* allocPtr = &allocInfo)
			{
				fixed (VkDescriptorSet* descripterSetPtr = &descriptorSet)
				{
					VulkanHelpers.CheckErrors(VulkanNative.vkAllocateDescriptorSets(compute.Support.Device, allocPtr, descripterSetPtr));
				}
			}

        }

        public unsafe static void UpdateDescriptorSet(this IVulkanCompute compute, ref VkWriteDescriptorSet writeSet, ref VkCopyDescriptorSet copySet)
        {
            fixed (VkWriteDescriptorSet* writeSetPtr = &writeSet)
            {
                fixed (VkCopyDescriptorSet* copySetPtr = &copySet)
                {
                    VulkanNative.vkUpdateDescriptorSets(compute.Support.Device,1, writeSetPtr, 1, copySetPtr);
                }
            }

        }

        public unsafe static void UpdateDescriptorSets(this IVulkanCompute compute, ref VkWriteDescriptorSet[] writeSet, ref VkCopyDescriptorSet[] copySet)
        {
            fixed (VkWriteDescriptorSet* writeSetPtr = &writeSet[0])
            {
                fixed (VkCopyDescriptorSet* copySetPtr = &copySet[0])
                {
                    VulkanNative.vkUpdateDescriptorSets(compute.Support.Device,(uint) writeSet.Length, writeSetPtr, (uint)copySet.Length, copySetPtr);
                }
            }

        }



        /*

        https://github.com/andrecunha/mandelbrot_vulkan_cpp/blob/master/src/mandelbrot.cc

  void CreateDescriptorSetLayout() {
    auto descriptor_set_layout_binding = vk::DescriptorSetLayoutBinding();
    descriptor_set_layout_binding
        .setDescriptorType(vk::DescriptorType::eStorageBuffer)
        .setDescriptorCount(1)
        .setStageFlags(vk::ShaderStageFlagBits::eCompute);
    auto descriptor_set_layout_create_info =
        vk::DescriptorSetLayoutCreateInfo();
    descriptor_set_layout_create_info.setBindingCount(1).setPBindings(
        &descriptor_set_layout_binding);
    descriptor_set_layout_ = device_->createDescriptorSetLayoutUnique(
        descriptor_set_layout_create_info);
  }

  void CreateDescriptorPool() {
    auto descriptor_pool_size = vk::DescriptorPoolSize();
    descriptor_pool_size.setType(vk::DescriptorType::eStorageBuffer)
        .setDescriptorCount(1);
    auto descriptor_pool_create_info = vk::DescriptorPoolCreateInfo();
    descriptor_pool_create_info.setMaxSets(1).setPoolSizeCount(1).setPPoolSizes(
        &descriptor_pool_size);
    descriptor_pool_ =
        device_->createDescriptorPoolUnique(descriptor_pool_create_info);
  }

  void CreateDescriptorSets() {
    auto descriptor_set_allocate_info = vk::DescriptorSetAllocateInfo();
    descriptor_set_allocate_info.setDescriptorPool(*descriptor_pool_)
        .setDescriptorSetCount(1)
        .setPSetLayouts(&descriptor_set_layout_.get());
    descriptor_sets_ =
        device_->allocateDescriptorSets(descriptor_set_allocate_info);
  }

  void ConnectBufferWithDescriptorSets() {
    auto descriptor_buffer_info = vk::DescriptorBufferInfo();
    descriptor_buffer_info.setBuffer(*buffer_).setOffset(0).setRange(
        buffer_size);
    auto write_descriptor_set = vk::WriteDescriptorSet();
    write_descriptor_set.setDstSet(descriptor_sets_[0])
        .setDstBinding(0)
        .setDescriptorCount(1)
        .setDescriptorType(vk::DescriptorType::eStorageBuffer)
        .setPBufferInfo(&descriptor_buffer_info);
    device_->updateDescriptorSets({write_descriptor_set}, {});
  }

		*/
    }
}

