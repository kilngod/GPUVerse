# GPU-Verse

## KilnGod's Multi-Platform Vulkan Demonstration Library

ILGPU project has been looking for ways to support high performance rendering along with high performance Computing. Therefore we would like to visualize dynamic fluid simulations while not having to rewrite rendering code for each platform.


### ILGPU targeting.
ILGPU supports CUDA, OpenCl, Velocity (CPU Parallel, Mac & AVX2 vector) compute platforms. ILGPU is considering targeting Vulkan Compute and possibly AMD ROCm.


## Vulkan Code Development
Targeting Vulkan development is a difficult task as Vulkan is designed as a high performance GPU framework with support for nearly any use case. Most coding frameworks ultimately end up in the coding graveyard due to originating developers misinformed belief they know every possible use case and they know best.

This framework is written with the assumption we don't know best! We want to get developers up and running on Vulkan and make it easy for developers to go in a different direction by not locking in developers choices by our own design decisions. In general we will provide what seems to be an optimal implementation for common use cases but we want the developer in the field to be in charge of their software..

### Static Inversion
We accomplish our goal by rolling our own "VulkanPlatform" which has a few basic interfaces for compute and rendering and we give developers control by "Static Inversion". All compute rendering methods are static classes where the rendering or compute interface are passed to the static classes for processing.
If developers do not like our static rendering or compute methods, they can roll their own static compute or render classes rather than being forced to use our solution as we don't live in a one size fits all world.



## Mac Git Notes (perhaps MS will fix one day)

Personally I've run into a endless of issues with Visual Studio 2022's source control working properly on a Mac. After months of aggravation using Git Desktop I finally found a solution, it's unclear why there is no tooling in Visual Studio 2022 Mac to correct this issue.

1) Create solution with Visual Studio 2022 on Windows then push to GitHub. 
2) Use Visual Studio Code to clone the solution from GitHub to your Mac. (key step, proper VS2022 cloning is broken on Mac)
3) Open the cloned solution on your Mac with VS2022 (assuming you've setup GitHub with VS2022) and it just works.



The Vulkan Code Generator is knocked off from Evergine https://github.com/EvergineTeam/Vulkan.NET who may have knocked off code from Mellinoe  https://github.com/mellinoe/vk.







