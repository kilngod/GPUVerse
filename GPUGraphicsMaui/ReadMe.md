# GPUGraphicsMaui

This library demonstrates how to use vulkan or metal with Maui.

## Android
Android just works! It took a while to figure out how to get Vulkan started on Android seems but this seems to work. 

## iOS
Our understanding of Apple policy is to allow static libraries or the creation of a XCode "framework library" for dynamic library to be allowed in the Apple store.
Currently we our dynamically loading of Molten library, which is a problem we our investigating. We will either create a "framework" or statically link MoltenVK.

## MacCatalyst
MacCatalyst seems to work fine, an application using MacCatalyst could be distributed by normal downloads. 
We're not sure Apple would allow a dynamic library into their store. As noted for iOS, we're looking into options with MoltenVk.


## Windows
We're still reviewing Windows. The issue is Maui-Windows is a highly locked down wrapper of WinRt (DirectX11?) which is highly locked down. 
We can render a few Vulkan frames but once the WinRT compositor kicks on it takes over and while Vulkan still works it can't interact with the display.
It appears from many of the blog posts we have read, the Maui team is not concerned about performance outside of their minimal corporate "use" cases.




