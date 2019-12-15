using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;

namespace Snowflake.Execution.Extensibility
{
    public sealed class EmulatedController : IEmulatedController
    {
        public EmulatedController(int portIndex,
            IInputDevice physicalDevice,
            IInputDriverInstance driverInstance,
            IControllerLayout targetLayout,
            IControllerElementMappings layoutMapping)
        {
            this.PortIndex = portIndex;
            this.PhysicalDevice = physicalDevice;
            this.PhysicalDeviceInstance = driverInstance;
            this.TargetLayout = targetLayout;
            this.LayoutMapping = layoutMapping;
        }

        public int PortIndex { get; }

        public IInputDevice PhysicalDevice { get; }

        public IControllerLayout TargetLayout { get; }

        public IControllerElementMappings LayoutMapping { get; }

        public IInputDriverInstance PhysicalDeviceInstance { get; }
    }
}
