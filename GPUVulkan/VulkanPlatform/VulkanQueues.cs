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

        public unsafe static uint FindComputeQueueFamilyIndex(this VkPhysicalDevice device, VkQueueFlags queueFlag)
        {
            uint queueFamilyCount = 0;

            VulkanNative.vkGetPhysicalDeviceQueueFamilyProperties(device, &queueFamilyCount, null);

            VkQueueFamilyProperties* queueFamilies = stackalloc VkQueueFamilyProperties[(int)queueFamilyCount];

            VulkanNative.vkGetPhysicalDeviceQueueFamilyProperties(device, &queueFamilyCount, queueFamilies);

            for (uint i = 0; i < queueFamilyCount; i++)
            {
                var queueFamily = queueFamilies[i];
                if ((queueFamily.queueFlags & queueFlag) != 0)
                {
                    
                    return i;
                }

            }
            return uint.MaxValue;
        }
        public unsafe static void GetQueue(this VkDevice device, int queueFamilyIndex, uint queueIndex, ref VkQueue queue)
        {
            fixed (VkQueue* queuePtr = &queue) 
            {
                VulkanNative.vkGetDeviceQueue(device,(uint) queueFamilyIndex, queueIndex, queuePtr);
            }
        }
    }
}
