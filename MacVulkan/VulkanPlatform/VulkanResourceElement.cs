﻿using GPUVulkan;
using System;
using System.Collections.Generic;
using System.Text;

namespace VulkanPlatform
{
    /// <summary>
    /// single resources to be shared among multiple threads
    /// </summary>
    public struct VulkanResourceElement
    {

        public uint BufferSize;

        public VkBuffer Buffer;

        public VkDeviceMemory DeviceMemory;

        public VkDescriptorPoolSize[] DescriptorPoolSizes;

        // one set of descriptor buffer infor per pool size.
        public VkDescriptorBufferInfo[][] DescriptorBufferSegments;
    }

   
}