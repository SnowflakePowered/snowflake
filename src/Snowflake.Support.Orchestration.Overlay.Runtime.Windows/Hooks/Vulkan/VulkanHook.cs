using Snowflake.Orchestration.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using X64 = Reloaded.Hooks.Definitions.X64;
using X86 = Reloaded.Hooks.Definitions.X86;
using Silk.NET.Vulkan;
using Reloaded.Hooks;
using Reloaded.Hooks.Definitions;
using Vanara.PInvoke;
using Silk.NET.Core;

namespace Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks.Vulkan
{
    internal class VulkanHook
    {

        private Vk VK;

        private nint ImGuiContext = 0;

        [X64.Function(X64.CallingConventions.Microsoft)]
        [X86.Function(X86.CallingConventions.Stdcall)]
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public unsafe delegate PfnVoidFunction vkCreateInstance(InstanceCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, Instance* pInstance);

        [X64.Function(X64.CallingConventions.Microsoft)]
        [X86.Function(X86.CallingConventions.Stdcall)]
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public unsafe delegate PfnVoidFunction vkCreateDevice(PhysicalDevice physicalDevice, DeviceCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, Device* device);

        [X64.Function(X64.CallingConventions.Microsoft)]
        [X86.Function(X86.CallingConventions.Stdcall)]
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public unsafe delegate Result vkQueuePresentKHR(Queue queue, PresentInfoKHR* pPresentInfo);

        [X64.Function(X64.CallingConventions.Microsoft)]
        [X86.Function(X86.CallingConventions.Stdcall)]
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public unsafe delegate void vkCmdDrawIndexed(CommandBuffer cmdBuffer, uint indexCount, uint instanceCount, uint firstIndex, int vertexOff, uint firstInstance);

        [X64.Function(X64.CallingConventions.Microsoft)]
        [X86.Function(X86.CallingConventions.Stdcall)]
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public unsafe delegate void vkAcquireNextImageKHR(Device device, SwapchainKHR swapChain, ulong timeout, Silk.NET.Vulkan.Semaphore semaphore, Fence fence, uint* pImageIndex);


        public unsafe VulkanHook(Instance? i, Device? d, IngameIpc ingameIpc)
        {
            this.IngameIpc = ingameIpc;
            this.VK = Vk.GetApi();

            var handle = Kernel32.GetModuleHandle("vulkan-1");
            
            if (i != null && d != null)
            {
                this.VK.CurrentInstance = i;
                this.VK.CurrentDevice = d;

                long x = (long)this.VK.GetDeviceProcAddr(d.Value, "vkQueuePresentKHR").Handle;
                //Console.WriteLine($"{x:x}");
                this.VkQueuePresentKHRHook = ReloadedHooks.Instance.CreateHook<vkQueuePresentKHR>(QueuePresentKHRFn, x);
            }

            unsafe {
                this.VkCreateInstanceHook = ReloadedHooks.Instance.CreateHook<vkCreateInstance>(CreateInstanceImpl, this.VK.Context.GetProcAddress("vkCreateInstance")).Activate();
                this.VkCreateDeviceHook = ReloadedHooks.Instance.CreateHook<vkCreateDevice>(CreateDeviceImpl, this.VK.Context.GetProcAddress("vkCreateDevice")).Activate();
            }
            this.IngameIpc.CommandReceived += CommandReceivedHandler;
        }
        
        [StructLayout(LayoutKind.Sequential)]
        unsafe struct LayerDeviceCreateInfo
        {
            public StructureType SType;
            public void* PNext;
            public uint LayerFunction;
            public LayerInstanceLink* pLayerInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        unsafe struct LayerInstanceLink
        {
            public LayerInstanceLink* PNext;
            public delegate* unmanaged[Cdecl]<Instance, byte*, PfnVoidFunction> pfnNextGetInstanceProcAddr;
            public delegate* unmanaged[Cdecl]<Device, byte*, PfnVoidFunction> pfnNextGetDeviceProcAddr;

        }

        private unsafe PfnVoidFunction CreateDeviceImpl(PhysicalDevice physicalDevice, DeviceCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, Device* device)
        {
            Console.WriteLine("[vk] cDI");
            var res = this.VkCreateDeviceHook.OriginalFunction(physicalDevice, pCreateInfo, pAllocator, device);

            if (this.VkQueuePresentKHRHook == null)
            {
                this.VK.CurrentDevice = *device;
                long x = (long)this.VK.GetDeviceProcAddr(*device, "vkQueuePresentKHR").Handle;
                Console.WriteLine($"{x:x}");
                this.VkQueuePresentKHRHook = ReloadedHooks.Instance.CreateHook<vkQueuePresentKHR>(QueuePresentKHRFn, x).Activate();
            }
            return res;
        }

        private unsafe PfnVoidFunction CreateInstanceImpl(InstanceCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, Instance* pInstance)
        {
            Console.WriteLine("[vk] cII");
            var res = this.VkCreateInstanceHook.OriginalFunction(pCreateInfo, pAllocator, pInstance);
            this.VK.CurrentInstance = *pInstance;
            //this.CurrentInstance = pInstance;
            return res;
        }

        private unsafe void AcquireNextImageImpl(Device device, SwapchainKHR swapChain, ulong timeout, Silk.NET.Vulkan.Semaphore semaphore, Fence fence, uint* pImageIndex)
        {
            Console.WriteLine("[vk] anI");
            this.AcquireNextImageImpl(device, swapChain, timeout, semaphore, fence, pImageIndex);
        }

        private void CmdDrawIndexedImpl(CommandBuffer cmdBuffer, uint indexCount, uint instanceCount, uint firstIndex, int vertexOff, uint firstInstance)
        {
            Console.WriteLine("[vk] drawIndexed");
            this.VkCmdDrawIndexImpl.OriginalFunction(cmdBuffer, indexCount, instanceCount, firstIndex, vertexOff, firstInstance);
        }

        private unsafe Result QueuePresentKHRFn(Queue queue, PresentInfoKHR* pPresentInfo)
        {
            Console.WriteLine("[vk] Present");
            return this.VkQueuePresentKHRHook.OriginalFunction(queue, pPresentInfo);
        }


        public void Activate()
        {
            Console.WriteLine("Activated Vulkan Hooks");
            this.VkCreateInstanceHook.Activate();
            this.VkCreateDeviceHook.Activate();
            this.VkQueuePresentKHRHook?.Activate();
        }

        private void CommandReceivedHandler(GameWindowCommand command)
        {
            if (command.Type == GameWindowCommandType.OverlayTextureEvent)
            {
                Console.WriteLine($"Got texhandle {command.TextureEvent.TextureHandle.ToString("x")} from PID {command.TextureEvent.SourceProcessId}");
                //this.Overlay.Refresh(command.TextureEvent);
            }
        }

        public IngameIpc IngameIpc { get; }
        public IHook<vkCreateInstance> VkCreateInstanceHook { get; }
        //public IHook<vkGetDeviceProcAddr> VkDeviceInstanceHook { get; }
        public IHook<vkQueuePresentKHR> VkQueuePresentKHRHook { get; set; }
        public IHook<vkCmdDrawIndexed> VkCmdDrawIndexImpl { get; }
        public IHook<vkAcquireNextImageKHR> VkAcquireNextImageKHRHook { get; }
        public IHook<vkCreateDevice> VkCreateDeviceHook { get; }
        public unsafe Instance* CurrentInstance { get; private set; }
    }
}