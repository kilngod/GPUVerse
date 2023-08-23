using System;
using GPUVulkan;

namespace VulkanPlatform
{
	public static class VulkanMemory
	{


		public static uint FindMemoryType(uint memory_type_bits, VkPhysicalDeviceMemoryProperties memoryProperties, VkMemoryPropertyFlags memoryFlags)
		{

            VkMemoryPropertyFlags memoryPropertyFlags = memoryProperties.GetMemoryType(1).propertyFlags;
         
            if (((memory_type_bits << 1 & 1) == 1) & (memoryPropertyFlags & memoryFlags) == memoryFlags)
            {
                return 1;
            }

            memoryPropertyFlags = memoryProperties.GetMemoryType(2).propertyFlags;
            if (((memory_type_bits << 2 & 1) == 1) & (memoryPropertyFlags & memoryFlags) == memoryFlags)
            {
                return 2;
            }

            memoryPropertyFlags = memoryProperties.GetMemoryType(3).propertyFlags;
            if (((memory_type_bits << 3 & 1) == 1) & (memoryPropertyFlags & memoryFlags) == memoryFlags)
            {
                return 3;
            }

            memoryPropertyFlags = memoryProperties.GetMemoryType(4).propertyFlags;
            if (((memory_type_bits << 4 & 1) == 1) & (memoryPropertyFlags & memoryFlags) == memoryFlags)
            {
                return 4;
            }

            memoryPropertyFlags = memoryProperties.GetMemoryType(5).propertyFlags;
            if (((memory_type_bits << 5 & 1) == 1) & (memoryPropertyFlags & memoryFlags) == memoryFlags)
            {
                return 5;
            }

            memoryPropertyFlags = memoryProperties.GetMemoryType(6).propertyFlags;
            if (((memory_type_bits << 6 & 1) == 1) & (memoryPropertyFlags & memoryFlags) == memoryFlags)
            {
                return 6;
            }

            memoryPropertyFlags = memoryProperties.GetMemoryType(7).propertyFlags;
            if (((memory_type_bits << 7 & 1) == 1) & (memoryPropertyFlags & memoryFlags) == memoryFlags)
            {
                return 7;
            }

            memoryPropertyFlags = memoryProperties.GetMemoryType(8).propertyFlags;
            if (((memory_type_bits << 8 & 1) == 1) & (memoryPropertyFlags & memoryFlags) == memoryFlags)
            {
                return 8;
            }

            memoryPropertyFlags = memoryProperties.GetMemoryType(9).propertyFlags;
            if (((memory_type_bits << 9 & 1) == 1) & (memoryPropertyFlags & memoryFlags) == memoryFlags)
            {
                return 9;
            }

            memoryPropertyFlags = memoryProperties.GetMemoryType(10).propertyFlags;
            if (((memory_type_bits << 10 & 1) == 1) & (memoryPropertyFlags & memoryFlags) == memoryFlags)
            {
                return 10;
            }
            return uint.MaxValue;

        }

		public static unsafe VkDeviceMemory* AllocateMemory(this IVulkanSupport support, ref VkBuffer buffer)
		{
			VkMemoryRequirements memoryRequirements = default(VkMemoryRequirements);
			VkPhysicalDeviceMemoryProperties memoryProperties = default(VkPhysicalDeviceMemoryProperties);

            VulkanNative.vkGetBufferMemoryRequirements(support.Device, buffer, &memoryRequirements);

            VulkanNative.vkGetPhysicalDeviceMemoryProperties(support.PhysicalDevice, &memoryProperties);

			VkMemoryAllocateInfo allocateInfo = new VkMemoryAllocateInfo()
			{
				sType = VkStructureType.VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO,
				memoryTypeIndex = FindMemoryType(memoryRequirements.memoryTypeBits, memoryProperties, VkMemoryPropertyFlags.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT| VkMemoryPropertyFlags.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT),
				allocationSize = memoryRequirements.size
			};

            VkDeviceMemory* memoryDevice = default(VkDeviceMemory*);

            VulkanHelpers.CheckErrors(VulkanNative.vkAllocateMemory(support.Device, &allocateInfo, null, memoryDevice));

            return memoryDevice;

		}

        public static unsafe void BindDeviceMemory(this IVulkanSupport support, ref VkBuffer buffer, ref VkDeviceMemory memory, uint Offset )
        {

            VulkanHelpers.CheckErrors(VulkanNative.vkBindBufferMemory(support.Device, buffer, memory, Offset));

        }
    }
}

