using System;
using GPUVulkan;


namespace VulkanPlatform
{
	public interface IVulkanSupport
	{
        DeliveryPlatform Platform { get; }

        bool EnableValidationLayers { get; set; }

        VulkanStringList RequiredExtensions { get; }

        VulkanStringList AvailableExtensions { get; }

        VulkanStringList RequiredValidationLayers { get; }

        VulkanStringList AvailableValidationLayers { get; }

        VulkanStringList InstanceExtensions { get; }

        VulkanStringList DeviceExtensions { get; }

        VkDevice Device { get; }

        void SetDevice(VkDevice device);

        VkInstance Instance { get; }

        void SetInstance(VkInstance instance);

        VkPhysicalDevice PhysicalDevice { get; }

        void SetPhysicalDevice(VkPhysicalDevice physicalDevice);

        VkPhysicalDeviceProperties DeviceProperties { get; }

        void CleanupVulkanSupport();

        

    }
}

