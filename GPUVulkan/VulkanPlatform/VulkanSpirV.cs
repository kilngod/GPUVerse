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

    
}
