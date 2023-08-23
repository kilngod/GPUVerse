# GPU-Verse

## KilnGod's Multi-Platform Vulkan Demonstration Library

ILGPU project has been looking for ways to support high performance rendering along with high performance Computing. We would like to visualize dynamic fluid simulations while not having to rewrite rendering code for each platform.

This solution is an illustration for the ILGPU team on how to Vulkan for both simulation and rendering..


### ILGPU compute targeting.
ILGPU supports CUDA, OpenCl, Velocity (CPU Parallel, Mac & AVX2 vector) compute platforms. ILGPU is considering targeting Vulkan Compute and perhaps AMD ROCm.


## Vulkan Code Development
Targeting Vulkan development is a difficult task as Vulkan is designed as a high performance GPU framework with support for nearly any use case. Most coding frameworks ultimately end up in the coding graveyard due to originating developers misinformed belief they know every possible use case and they know best.

This framework is written with the assumption we don't know best! We want to get developers up and running on Vulkan and make it easy for developers to go in a different direction by not locking in developers choices by our own design decisions. In general we will provide what seems to be an optimal implementation for common use cases but we want the developer in the field to be in charge of their software..

### Static Inversion
We accomplish our goal by rolling our own "VulkanPlatform" which has a few basic interfaces for compute and rendering and we give developers control by "Static Inversion". All compute rendering methods are static classes where the rendering or compute interface are passed to the static classes for processing.
If developers do not like our static rendering or compute methods, they can roll their own static compute or render classes rather than being forced to use our solution as we don't live in a one size fits all world.

### Vulkan C# Code generation from 
The Vulkan Code Generator is knocked off from Evergine https://github.com/EvergineTeam/Vulkan.NET who may have knocked off code generator from Mellinoe https://github.com/mellinoe/vk.



## Maui (iOS, Mac, Android and Windows)
### Android
Android phones usually include Vulkan as part of the build, 

### iOS
Dynamically linked libraries do not work without packaging on iOS, therefor we will likely update the code generator to create a static library or create a "framework" around the dynamic library.

### MacCatalyst
Unlike IOS, a MacCatalyst can work with a dynamic library. That said, Apple may or may not allow a non-framework dynamic library in the the Apple store.

### Windows
Maui windows is really a wrapper on WinRT which is based on DirectX 11 or 9. Microsoft in their finite wisdom locked down WinRT, one can grab the window handle with Vulkan and paint a few frames. But as soon a the WinRT compositor starts vulkan rendering stops.

The Avalonia project (https://github.com/AvaloniaUI/) demonstrates how to render to a bitmap from Vulkan to display with their compositor via DX11 on Windows. Our guess is this could be used with Maui on Windows, however performance is likely questionable. 

## Desktop (Mac, Windows, Linux)
We've implemented sample projects for Windows and MacOS. Linux should be identical to these implementations.

## Mac GitHub Notes (perhaps MS will fix VS2022 Mac Version control one day)

Personally I've run into a endless of issues with Visual Studio 2022's source control working properly on a Mac. After months of aggravation using GitHub Desktop I finally found a solution, it's unclear why GitHub has not been corrected Visual Studio 2022 Mac.


1) Create solution with Visual Studio 2022 on Windows then push to GitHub. 
2) Use Visual Studio Code to clone the solution from GitHub to your Mac. (key step, proper VS2022 cloning is broken on Mac) My guess is Apple's authentication key chain from setting up a preview version of Visual Studio 2022 Mac screwed things up and VS2022 does not check if this key chain is in order so it stays broken forever.
3) Open the cloned solution on your Mac with VS2022 (assuming you've setup GitHub with VS2022) and it just works.












