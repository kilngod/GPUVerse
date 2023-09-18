using System;
using GPUVulkan;

namespace VulkanPlatform
{
	/// <summary>
	/// allocate per thread resouces
	/// </summary>
	public struct VulkanResourceThread
	{
		public VkDescriptorSet[] DescriptorSets;
		public VkDescriptorSetLayout DescriptorSetLayout; 
		public VkDescriptorPool DescriptorPool;

        public bool InUse { get; set; } = false;
		public bool Allocated { get; set; } = false;
        public VulkanResourceThread()
		{
		}
	}
}

