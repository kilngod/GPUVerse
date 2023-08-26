using System;
using GPUVulkan;

namespace VulkanPlatform
{
	public static class VulkanBuffer
	{
		public unsafe static void CreateBuffer(this IVulkanSupport support, ulong bufferSize, ref VkBuffer buffer, VkBufferUsageFlags usageFlags = VkBufferUsageFlags.VK_BUFFER_USAGE_STORAGE_BUFFER_BIT, VkSharingMode mode = VkSharingMode.VK_SHARING_MODE_EXCLUSIVE)
		{
			VkBufferCreateInfo createInfo = new VkBufferCreateInfo()
			{
				 sType = VkStructureType.VK_STRUCTURE_TYPE_BUFFER_CREATE_INFO,
				 sharingMode = mode,
				 size = bufferSize,
				 usage = usageFlags
            };

			fixed (VkBuffer* bufferPtr = &buffer)
			{

				VulkanNative.vkCreateBuffer(support.Device, &createInfo, null, bufferPtr);
			}
			
			
		}
	}
}

