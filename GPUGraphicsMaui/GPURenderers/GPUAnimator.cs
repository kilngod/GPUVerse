using System;
using System.Diagnostics;

namespace GPUGraphicsMaui.GPURenderers
{
	public partial class GPUAnimator
	{
        Action AnimateMethod { get; set; }

        public bool isRunning = false;
        public void set(Action animateMethod)
        {
            AnimateMethod = animateMethod;
        }

        private void update()
        {
            if (isRunning)
            {
                AnimateMethod?.Invoke();
            }
        }

#if ANDROID || IOS || MACCATALYST || WINDOWS

#else
        public void start()
        {
            isRunning = true;
            Task.Factory.StartNew(() => Loop(), TaskCreationOptions.LongRunning);
        }

        int frameTime = 1000 / 60;//每秒60帧
        private void Loop()
        {
            while (isRunning)
            {
                try
                {
                    Thread.Sleep(frameTime);

                    update();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Encountered an error while rendering: " + e);
                    //throw;
                }
            }
        }


        public void cancel()
        {
            isRunning = false;
        }

#endif

    }
}

