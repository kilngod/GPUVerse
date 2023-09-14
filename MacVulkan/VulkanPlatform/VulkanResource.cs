using System;
using GPUVulkan;

namespace VulkanPlatform
{
	/// <summary>
	/// 
	/// </summary>
	public class VulkanResource
	{
		
        private VkDescriptorSetLayout _descriptorSetLayout;
		private VkDescriptorPool _descriptorPool;

		public bool InUse { get; set; } = false;
		public bool Allocated { get; set; } = false;
        public VulkanResource()
		{
		}
	}
}

