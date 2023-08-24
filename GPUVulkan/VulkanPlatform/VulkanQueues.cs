using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanPlatform;

namespace GPUVulkan.VulkanPlatform
{
    public static class VulkanQueues
    {
        public unsafe static void GetQueue(this VulkanSupport support, uint queueFamilyIndex, uint queueIndex, ref VkQueue queue)
        {
            fixed (VkQueue* queuePtr = &queue) 
            {
                VulkanNative.vkGetDeviceQueue(support.Device, queueFamilyIndex, queueIndex, queuePtr);
            }
        }
    }
}
