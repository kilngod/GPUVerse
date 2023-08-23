using System;
using GPUVulkan;

namespace VulkanPlatform
{
	public static class VulkanBuffer
	{
		public unsafe static VkBuffer* CreateBuffer(this IVulkanSupport support, ulong bufferSize, VkBufferUsageFlags usageFlags = VkBufferUsageFlags.VK_BUFFER_USAGE_STORAGE_BUFFER_BIT, VkSharingMode mode = VkSharingMode.VK_SHARING_MODE_EXCLUSIVE)
		{
			VkBufferCreateInfo createInfo = new VkBufferCreateInfo()
			{
				 sType = VkStructureType.VK_STRUCTURE_TYPE_BUFFER_CREATE_INFO,
				 sharingMode = mode,
				 size = bufferSize,
				 usage = usageFlags
            };

			VkBuffer* bufferResult = default;

			VulkanNative.vkCreateBuffer(support.Device, &createInfo, null, bufferResult);

			return bufferResult;
			
		}
	}
}

