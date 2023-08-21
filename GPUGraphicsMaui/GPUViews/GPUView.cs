﻿using Microsoft.Maui.Controls;

using GPUGraphicsMaui.GPURenderers;

using GPUGraphicsMaui.GPUHandlers;

namespace GPUGraphicsMaui.GPUViews
{
    public partial class GPUView : View, IGPUView
    {
  
        private GPUView(GPUEngine engine, GPUPlatform platform)
        {
            Engine = engine;
            Platform = platform;

            // from https://github.com/dotnet/maui/issues/7698
            this.propertyMapper = new PropertyMapper<View>();
            var overrides = GetRendererOverrides<GPUView>();
            overrides.ModifyMapping("Background",
                    (handler, view, act) =>
                    {
                        string viewstr1 = handler.VirtualView.ToString();
                        string viewstr2 = view.ToString();
                        string actor = act?.ToString();
                    });
          //  Background = null;

//BackgroundColor = new Color(255,255,255,255);
        }

        public GPUEngine Engine { get; }
        public GPUPlatform Platform { get; }


        public void Invalidate()
        {
            Handler?.Invoke(nameof(IGPUView.Invalidate));
        }

        


    }
}

