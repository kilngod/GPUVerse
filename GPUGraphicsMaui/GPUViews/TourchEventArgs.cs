using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPUGraphicsMaui.GPUViews
{

    public class TouchEventArgs : EventArgs
    {
        public TouchEventArgs()
        {

        }

        public TouchEventArgs(PointF[] points, bool isInsideBounds)
        {
            Touches = points;
            IsInsideBounds = isInsideBounds;
        }

        /// <summary>
        /// This is only used for EndInteraction;
        /// </summary>
        public bool IsInsideBounds { get; private set; }

        public PointF[] Touches { get; private set; }
    }
}
