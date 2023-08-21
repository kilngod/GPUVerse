using GPUVulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin;



namespace VulkanPlatform
{
#if DEBUG
    public static class VulkanDebug
    {
        
        private static VkDebugUtilsMessengerEXT debugMessenger;


        public static unsafe void SetupDebugMessenger(this IVulkanSupport support)
        {
#if false
            VulkanFlowTracer.AddItem("VulkanDebug.SetupDebugMessenger");
            fixed (VkDebugUtilsMessengerEXT* debugMessengerPtr = &debugMessenger)
            {
                var funcPtr = VulkanNative.vkGetInstanceProcAddr(support.Instance, "vkCreateDebugUtilsMessengerEXT".ToPointer());
                if (funcPtr != IntPtr.Zero)
                {
                    VkDebugUtilsMessengerCreateInfoEXT createInfo = new VkDebugUtilsMessengerCreateInfoEXT()
                    {
                        sType = VkStructureType.VK_STRUCTURE_TYPE_DEBUG_UTILS_MESSENGER_CREATE_INFO_EXT,
                        messageSeverity = VkDebugUtilsMessageSeverityFlagsEXT.VK_DEBUG_UTILS_MESSAGE_SEVERITY_VERBOSE_BIT_EXT | VkDebugUtilsMessageSeverityFlagsEXT.VK_DEBUG_UTILS_MESSAGE_SEVERITY_WARNING_BIT_EXT | VkDebugUtilsMessageSeverityFlagsEXT.VK_DEBUG_UTILS_MESSAGE_SEVERITY_ERROR_BIT_EXT,
                        messageType = VkDebugUtilsMessageTypeFlagsEXT.VK_DEBUG_UTILS_MESSAGE_TYPE_GENERAL_BIT_EXT | VkDebugUtilsMessageTypeFlagsEXT.VK_DEBUG_UTILS_MESSAGE_TYPE_PERFORMANCE_BIT_EXT | VkDebugUtilsMessageTypeFlagsEXT.VK_DEBUG_UTILS_MESSAGE_TYPE_VALIDATION_BIT_EXT,
                        pfnUserCallback = Marshal.GetFunctionPointerForDelegate(DebugCallback),
                        pUserData = null
                    };
                    VulkanHelpers.CheckErrors(VulkanNative.vkCreateDebugUtilsMessengerEXT(support.Instance, &createInfo, null, debugMessengerPtr));
                }
            }
#endif

        }


  
        private unsafe static VkBool32 DebugCallback(VkDebugUtilsMessageSeverityFlagsEXT messageSeverity,
            VkDebugUtilsMessageTypeFlagsEXT messageType,
            VkDebugUtilsMessengerCallbackDataEXT pCallbackData,
            void* pUserData)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("VulkanDebug.DebugCallback");

            if (messageSeverity > VkDebugUtilsMessageSeverityFlagsEXT.VK_DEBUG_UTILS_MESSAGE_SEVERITY_VERBOSE_BIT_EXT)
            {
                Console.WriteLine
                    ($"{messageSeverity} {messageType}" + Marshal.PtrToStringAnsi((IntPtr)pCallbackData.pMessage));

            }
#endif

            return false;
        }

        public static unsafe void DestroyDebugMessenger(this IVulkanSupport support)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("VulkanDebug.DestroyDebugMessenger");
            var funcPtr = VulkanNative.vkGetInstanceProcAddr(support.Instance, "vkDestroyDebugUtilsMessengerEXT".ToPointer());
            if (funcPtr != IntPtr.Zero)
            {
                VulkanNative.vkDestroyDebugUtilsMessengerEXT(support.Instance, debugMessenger, null);
            }
#endif
        }

    }
#endif
        }
