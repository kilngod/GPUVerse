using GPUVulkan;
using System;
using System.Collections.Generic;
using System.Text;

namespace VulkanPlatform
{

    public struct SharedResource
    {
        public VkBuffer Buffer { get; set; }

        public uint BufferSize { get; set; }

        public VkDeviceMemory DeviceMemory { get; set; }

        public VkDescriptorPoolSize[] descriptorPoolSizes { get; set; }

    }
}
