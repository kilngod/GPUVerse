# GPUVulkan

This is a low level C# wrapper of Vulkan. 

## Evolution

This project was cloned from the EvergineTeam

https://github.com/EvergineTeam/Vulkan.NET

Who appears to have cloned from Eric Mellino

https://github.com/mellinoe/vk

We would like to thank both for their work, our code is in the VulkanPlatform folder. We also made a minor mode to allow this code to work Apple platforms.

As we have noted elsewhere, we will like have to create an Xcode "framework" or update the generator for static linking to allow developers to publish to the Apple Store.

## Static Inversion

It seemed less than prudent to create a giant object library wrapper around Vulkan as we simply do not know everyone's possible "use" cases.
Therefor, we elected a minimalist library to wrap Vulkan with 3 Interfaces and 1 object, everything else is accomplished via extension methods.

The logic is very simple, if you do not like our extension method, feel free to code your own extension method.

## Object and Interfaces
IVulkanSupport (interface) & VulkanSupport (object) host Vulkan's Device, PhysicalDevice and Instance objects, this provides one stop shopping for these mandatory hosted objects.

IVulkanRender & IVulkanCompute are the typical interfaces for renderer or computing, users can create their own interfaces and objects when they do not have a supported use case.




