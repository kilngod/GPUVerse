using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPUGraphicsMaui.GPUViews
{
    public interface IPlatformGPUView
    {
        event Action ViewSizeChanged;
        event Action ViewLoaded;
        event Action ViewRemoved;

        void Invalidate();
    }
}
