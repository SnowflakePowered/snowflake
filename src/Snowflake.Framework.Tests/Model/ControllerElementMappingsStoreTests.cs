using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;
using Snowflake.Model.Database;
using Snowflake.Model.Database.Models;
using Snowflake.Persistence;
using Snowflake.Services;
using Snowflake.Tests;
using Xunit;

namespace Snowflake.Model.Tests
{
    public class ControllerElementMappingsStoreTests
    {
        [Fact]
        public void GetProfileNames_Test()
        {
            var mapcol = new ControllerElementMappings("Keyboard",
                           "TEST_CONTROLLER",
                           InputDriverType.Keyboard,
                           IDeviceEnumerator.VirtualVendorID,
                           new XInputDeviceInstance(0).DefaultLayout);

            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var elementStore = new ControllerElementMappingsStore(optionsBuilder);

            elementStore.AddMappings(mapcol, "default");
        }

        [Fact]
        public void AddGamepadMappings_Test()
        {
            var stoneProvider = new StoneProvider();
            foreach (var testmappings in stoneProvider.Controllers.Values)
            {
                var mapcol = new ControllerElementMappings("Keyboard",
                           "TEST_CONTROLLER",
                           InputDriverType.Keyboard,
                           IDeviceEnumerator.VirtualVendorID,
                           new XInputDeviceInstance(0).DefaultLayout);

                var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");

                var elementStore = new ControllerElementMappingsStore(optionsBuilder);

                elementStore.AddMappings(mapcol, "default");

                var retStore = elementStore.GetMappings(mapcol.ControllerID, 
                    InputDriverType.Keyboard, 
                    "Keyboard",
                    IDeviceEnumerator.VirtualVendorID,
                    "default");

                foreach (var element in retStore)
                {
                    Assert.Contains(element.LayoutElement, mapcol.Select(x => x.LayoutElement));
                    Assert.Equal(element.DeviceCapability,
                        mapcol.First(x => x.LayoutElement == element.LayoutElement).DeviceCapability);
                }
            }
        }

        [Fact]
        public void UpdateMappedInputCollectionGamepad_Test()
        {
            var stoneProvider = new StoneProvider();
            foreach (var testmappings in stoneProvider.Controllers.Values)
            {
                var mapcol = new ControllerElementMappings("Keyboard",
                           "TEST_CONTROLLER",
                           InputDriverType.Keyboard,
                           IDeviceEnumerator.VirtualVendorID,
                           new XInputDeviceInstance(0).DefaultLayout);

                var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");

                var elementStore = new ControllerElementMappingsStore(optionsBuilder);

                elementStore.AddMappings(mapcol, "default");

                // map buttonA to buttonB.
                mapcol[ControllerElement.ButtonA] = DeviceCapability.Button1;

                elementStore.UpdateMappings(mapcol, "default");
                var retStore = elementStore.GetMappings(mapcol.ControllerID,
                    InputDriverType.Keyboard, mapcol.DeviceName, IDeviceEnumerator.VirtualVendorID, "default");

                foreach (var element in retStore)
                {
                    Assert.Contains(element.LayoutElement, mapcol.Select(x => x.LayoutElement));
                    Assert.Equal(element.DeviceCapability,
                        mapcol.First(x => x.LayoutElement == element.LayoutElement).DeviceCapability);
                }

                // Switch left joycon has no A button 
                if (testmappings.Layout[ControllerElement.ButtonA] != null)
                {
                    Assert.Equal(DeviceCapability.Button1, retStore[ControllerElement.ButtonA]);
                }
            }
        }

        [Fact]
        public void DeleteMappings_Test()
        {
            var stoneProvider = new StoneProvider();
            foreach (var testmappings in stoneProvider.Controllers.Values)
            {
                var mapcol = new ControllerElementMappings("Keyboard",
                           "TEST_CONTROLLER",
                           InputDriverType.Keyboard,
                           IDeviceEnumerator.VirtualVendorID,
                           new XInputDeviceInstance(0).DefaultLayout);

                var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
                var elementStore = new ControllerElementMappingsStore(optionsBuilder);

                elementStore.AddMappings(mapcol, "default");

                var retStore = elementStore.GetMappings(mapcol.ControllerID,
                    InputDriverType.Keyboard, mapcol.DeviceName, IDeviceEnumerator.VirtualVendorID, "default");
                Assert.NotNull(retStore);
                elementStore.DeleteMappings(mapcol.ControllerID, InputDriverType.Keyboard, mapcol.DeviceName, 
                    IDeviceEnumerator.VirtualVendorID, "default");
                var deletedRetStore = elementStore.GetMappings(mapcol.ControllerID, InputDriverType.Keyboard,
                    mapcol.DeviceName, IDeviceEnumerator.VirtualVendorID, "default");
                Assert.Null(deletedRetStore);
            }
        }

        [Fact]
        public void GetMultipleMappings_Test()
        {
            var stoneProvider = new StoneProvider();
            foreach (var testmappings in stoneProvider.Controllers.Values)
            {
                var mapcol = new ControllerElementMappings("Keyboard",
                            "TEST_CONTROLLER",
                            InputDriverType.Keyboard,
                            IDeviceEnumerator.VirtualVendorID,
                            new XInputDeviceInstance(0).DefaultLayout);

                var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
                var elementStore = new ControllerElementMappingsStore(optionsBuilder);

                elementStore.AddMappings(mapcol, "default");
                elementStore.AddMappings(mapcol, "default2");

                Assert.Equal(2, elementStore.GetMappings(mapcol.ControllerID, mapcol.DeviceName,
                    IDeviceEnumerator.VirtualVendorID)
                    .Count());
                elementStore.DeleteMappings(mapcol.ControllerID, InputDriverType.Keyboard, mapcol.DeviceName, 
                    IDeviceEnumerator.VirtualVendorID, "default");
                Assert.Single(elementStore.GetMappings(mapcol.ControllerID, mapcol.DeviceName, mapcol.VendorID));
                elementStore.DeleteMappings(mapcol.ControllerID, mapcol.DeviceName, IDeviceEnumerator.VirtualVendorID);
                Assert.Empty(elementStore.GetMappings(mapcol.ControllerID, mapcol.DeviceName, mapcol.VendorID));
            }
        }

        [Fact]
        public void AddKeyboardMappings_Test()
        {
            var stoneProvider = new StoneProvider();
            foreach (var testmappings in stoneProvider.Controllers.Values)
            {
                var mapcol = new ControllerElementMappings("Keyboard",
                           "TEST_CONTROLLER",
                           InputDriverType.Keyboard,
                           IDeviceEnumerator.VirtualVendorID,
                           new XInputDeviceInstance(0).DefaultLayout);

                var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
                var elementStore = new ControllerElementMappingsStore(optionsBuilder);

                elementStore.AddMappings(mapcol, "default");

                var retStore = elementStore.GetMappings(
                    mapcol.ControllerID, InputDriverType.Keyboard, "Keyboard", 
                    IDeviceEnumerator.VirtualVendorID, "default");
                foreach (var element in retStore)
                {
                    Assert.Contains(element.LayoutElement, mapcol.Select(x => x.LayoutElement));
                    Assert.Equal(element.DeviceCapability,
                        mapcol.First(x => x.LayoutElement == element.LayoutElement).DeviceCapability);
                }
            }
        }
    }
}
