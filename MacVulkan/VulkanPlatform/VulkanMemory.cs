using System;
using GPUVulkan;

namespace VulkanPlatform
{
	public static class VulkanMemory
	{


		public static uint FindMemoryType(uint memory_type_bits, VkPhysicalDeviceMemoryProperties memoryProperties, VkMemoryPropertyFlags requestedMemoryFlags, ref bool localMemory)
		{

            VkMemoryPropertyFlags memoryPropertyFlags = memoryProperties.GetMemoryType(0).propertyFlags;
         
            if ((memoryPropertyFlags & requestedMemoryFlags) == requestedMemoryFlags)
            {
                localMemory = (requestedMemoryFlags & VkMemoryPropertyFlags.VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT) >0;
                return 0;
            }
            for(int i = 1; i < 32; i++)
            {
                memoryPropertyFlags = memoryProperties.GetMemoryType(1).propertyFlags;
                if (((memory_type_bits << (i-1) & 1) == 1) & (memoryPropertyFlags & requestedMemoryFlags) == requestedMemoryFlags)
                {
                    localMemory = (memoryPropertyFlags & VkMemoryPropertyFlags.VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT) > 0;
                    return (uint) i;
                }
            }
            return uint.MaxValue;
            
        }

		public static unsafe void AllocateMemory(this IVulkanSupport support, ref VkBuffer buffer, ref VkDeviceMemory deviceMemory, ref bool localMemory)
		{
			VkMemoryRequirements memoryRequirements = default(VkMemoryRequirements);
			VkPhysicalDeviceMemoryProperties memoryProperties = default(VkPhysicalDeviceMemoryProperties);

            VulkanNative.vkGetBufferMemoryRequirements(support.Device, buffer, &memoryRequirements);

            VulkanNative.vkGetPhysicalDeviceMemoryProperties(support.PhysicalDevice, &memoryProperties);

			VkMemoryAllocateInfo allocateInfo = new VkMemoryAllocateInfo()
			{
				sType = VkStructureType.VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO,
				memoryTypeIndex = FindMemoryType(memoryRequirements.memoryTypeBits, memoryProperties, VkMemoryPropertyFlags.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT| VkMemoryPropertyFlags.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT, ref localMemory),
				allocationSize = memoryRequirements.size
			};

            fixed (VkDeviceMemory* memoryDevicePtr = &deviceMemory)
            {
                VulkanHelpers.CheckErrors(VulkanNative.vkAllocateMemory(support.Device, &allocateInfo, null, memoryDevicePtr));

            }

		}

        public static unsafe void BindDeviceMemory(this IVulkanSupport support, ref VkBuffer buffer, ref VkDeviceMemory memory, uint Offset )
        {

            VulkanHelpers.CheckErrors(VulkanNative.vkBindBufferMemory(support.Device, buffer, memory, Offset));

        }
    }
}

