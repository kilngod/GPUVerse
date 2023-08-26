using GPUVulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VulkanPlatform
{
   
    public struct VulkanSpirV
    {
        public string Name { get; set; }

        public VkShaderStageFlags ShaderStageType { get; set; }

        public string EntryName { get; set; }
        
        public byte[] SpirVByte { get; set; }

    }

    public static class VulkanShaders
    {
        public static unsafe VkShaderModule CreateShaderModule(this VkDevice device, byte[] code)
        {
            VkShaderModuleCreateInfo createInfo = new VkShaderModuleCreateInfo();
            createInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_SHADER_MODULE_CREATE_INFO;
            createInfo.codeSize = (UIntPtr)code.Length;

            fixed (byte* sourcePointer = code)
            {
                createInfo.pCode = (uint*)sourcePointer;
            }

            VkShaderModule shaderModule = default(VkShaderModule);

            VulkanHelpers.CheckErrors(VulkanNative.vkCreateShaderModule(device, &createInfo, null, &shaderModule));

            return shaderModule;
        }

    }

}
