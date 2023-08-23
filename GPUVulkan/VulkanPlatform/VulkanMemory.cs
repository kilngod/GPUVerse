using System;
using GPUVulkan;

namespace VulkanPlatform
{
	public static class VulkanMemory
	{
		public static void AllowcateMemory(this IVulkanSupport support, ref VkBuffer buffer, ref VkBufferCreateInfo createInfo)
		{
			VkMemoryRequirements memoryRequirements = new VkMemoryRequirements()
			{
				size = createInfo.size,
				memoryTypeBits = 
			};

			VkMemoryAllocateInfo allocateInfo = new VkMemoryAllocateInfo()
			{
				sType = VkStructureType.VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO,
				memoryTypeIndex
			}
		}
	}
}

