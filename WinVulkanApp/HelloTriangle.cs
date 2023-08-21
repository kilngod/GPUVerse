using GPUVulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanPlatform;

#nullable disable

namespace WinVulkanApp
{

    public partial class HelloTriangle
    {
        const uint WIDTH = 800;
        const uint HEIGHT = 600;

        private Form window;

        private VulkanSupport support;

        private IVulkanRenderer renderer;

        public Form InitWindow()
        {
            window = new Form();
            window.Text = "Vulkan Triangle Rasterization";
            window.Size = new System.Drawing.Size((int)WIDTH, (int)HEIGHT);
            window.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            window.Show();
            return window;
        }

        public void InitVulkan()
        {
            support = new VulkanSupport(DeliveryPlatform.Windows);

            renderer = new WinVulkanRenderer(window);
            (renderer as WinVulkanRenderer).VSupport = support;

            (renderer as WinVulkanRenderer).SetupPipeline();


        }

        private void MainLoop()
        {
            bool isClosing = false;
            window.FormClosing += (s, e) =>
            {
                isClosing = true;
            };

            while (!isClosing)
            {
                Application.DoEvents();

                renderer.DrawFrame();
            }

            VulkanHelpers.CheckErrors(VulkanNative.vkDeviceWaitIdle(support.Device));
        }

        public void CleanUp()
        {
            (renderer as WinVulkanRenderer).CleanUpPipeline();
            support.CleanupVulkanSupport();
        }

        public void Run()
        {
            this.InitWindow();

            this.InitVulkan();

            this.MainLoop();

            this.CleanUp();
        }

    }
}
