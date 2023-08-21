﻿
#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#else

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
#endif
using GPUGraphicsMaui.GPUHandlers;
using GPUGraphicsMaui.GPURenderers;
using GPUGraphicsMaui.GPUDevices;
using System;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using System.Runtime.InteropServices;
using WinRT;
using VulkanPlatform;

namespace GPUGraphicsMaui.GPUViews
{
    public partial class PlatformGPUView : Panel, IWinRTObject, ICustomQueryInterface// FrameworkElement// SwapChainPanel // UserControl//SwapChainBackgroundPanel
    {


        public IntPtr ChildWindow = IntPtr.Zero;

        private IDrawable _drawable;


        public IDrawable Drawable
        {
            get => _drawable;
            set
            {
                _drawable = value;
                //Invalidate();
            }
        }

        public PlatformGPUView(GPUSupport support) : base()
		{
#if DEBUG
            VulkanFlowTracer.AddItem("PlatformGPUView.PlatformGPUView");
#endif
         //   _gpuSupport = support;

            
            
           
        }

       

        public void Invalidate()
        {
       
            
        //    base.InvalidateViewport(); //?
        }

        protected override Windows.Foundation.Size ArrangeOverride(Windows.Foundation.Size finalSize)
        {
            if (this.Height != finalSize.Height) {
                ViewSizeChanged?.Invoke();
            }
            return base.ArrangeOverride(finalSize);
        }

        protected override Windows.Foundation.Size MeasureOverride(Windows.Foundation.Size availableSize)
        {
           
            if (this.Height != availableSize.Height) {
                ViewSizeChanged?.Invoke();
            }

            return base.MeasureOverride(availableSize);

           
        }



      
    }


}

