using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
            var mapcol = new ControllerElementMappingProfile("Keyboard",
                           "TEST_CONTROLLER",
                           InputDriver.Keyboard,
                           IDeviceEnumerator.VirtualVendorID,
                           new XInputDeviceInstance(0).DefaultLayout);

            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var elementStore = new ControllerElementMappingProfileStore(optionsBuilder);

            elementStore.AddMappings(mapcol, "default");
        }

        [Fact]
        public void AddGamepadMappings_Test()
        {
            var stoneProvider = new StoneProvider();
            foreach (var testmappings in stoneProvider.Controllers.Values)
            {
                var mapcol = new ControllerElementMappingProfile("Xbox",
                           "TEST_CONTROLLER",
                           InputDriver.XInput,
                           IDeviceEnumerator.VirtualVendorID,
                           new XInputDeviceInstance(0).DefaultLayout);

                var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");

                var elementStore = new ControllerElementMappingProfileStore(optionsBuilder);

                elementStore.AddMappings(mapcol, "default");

                var retStore = elementStore.GetMappings(mapcol.ControllerID, 
                    InputDriver.XInput, 
                    "Xbox",
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
        public async Task AddGamepadAsyncMappings_Test()
        {
            var stoneProvider = new StoneProvider();
            foreach (var testmappings in stoneProvider.Controllers.Values)
            {
                var mapcol = new ControllerElementMappingProfile("Keyboard",
                           "TEST_CONTROLLER",
                           InputDriver.Keyboard,
                           IDeviceEnumerator.VirtualVendorID,
                           new XInputDeviceInstance(0).DefaultLayout);

                var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");

                var elementStore = new ControllerElementMappingProfileStore(optionsBuilder);

               await elementStore.AddMappingsAsync(mapcol, "default");

                var retStore = await elementStore.GetMappingsAsync(mapcol.ControllerID,
                    InputDriver.Keyboard,
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
                var mapcol = new ControllerElementMappingProfile("Keyboard",
                           "TEST_CONTROLLER",
                           InputDriver.Keyboard,
                           IDeviceEnumerator.VirtualVendorID,
                           new XInputDeviceInstance(0).DefaultLayout);

                var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");

                var elementStore = new ControllerElementMappingProfileStore(optionsBuilder);

                elementStore.AddMappings(mapcol, "default");

                // map buttonA to buttonB.
                mapcol[ControllerElement.ButtonA] = DeviceCapability.Button1;

                elementStore.UpdateMappings(mapcol, "default");
                var retStore = elementStore.GetMappings(mapcol.ControllerID,
                    InputDriver.Keyboard, mapcol.DeviceName, IDeviceEnumerator.VirtualVendorID, "default");

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
        public async Task UpdateMappedInputCollectionGamepadAsync_Test()
        {
            var stoneProvider = new StoneProvider();
            foreach (var testmappings in stoneProvider.Controllers.Values)
            {
                var mapcol = new ControllerElementMappingProfile("Keyboard",
                           "TEST_CONTROLLER",
                           InputDriver.Keyboard,
                           IDeviceEnumerator.VirtualVendorID,
                           new XInputDeviceInstance(0).DefaultLayout);

                var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");

                var elementStore = new ControllerElementMappingProfileStore(optionsBuilder);

                await elementStore.AddMappingsAsync(mapcol, "default");

                // map buttonA to buttonB.
                mapcol[ControllerElement.ButtonA] = DeviceCapability.Button1;

                await elementStore.UpdateMappingsAsync(mapcol, "default");
                var retStore = await elementStore.GetMappingsAsync(mapcol.ControllerID,
                    InputDriver.Keyboard, mapcol.DeviceName, IDeviceEnumerator.VirtualVendorID, "default");

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
                var mapcol = new ControllerElementMappingProfile("Keyboard",
                           "TEST_CONTROLLER",
                           InputDriver.Keyboard,
                           IDeviceEnumerator.VirtualVendorID,
                           new XInputDeviceInstance(0).DefaultLayout);

                var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
                var elementStore = new ControllerElementMappingProfileStore(optionsBuilder);

                elementStore.AddMappings(mapcol, "default");

                var retStore = elementStore.GetMappings(mapcol.ControllerID,
                    InputDriver.Keyboard, mapcol.DeviceName, IDeviceEnumerator.VirtualVendorID, "default");
                Assert.NotNull(retStore);
                elementStore.DeleteMappings(mapcol.ControllerID, InputDriver.Keyboard, mapcol.DeviceName, 
                    IDeviceEnumerator.VirtualVendorID, "default");
                var deletedRetStore = elementStore.GetMappings(mapcol.ControllerID, InputDriver.Keyboard,
                    mapcol.DeviceName, IDeviceEnumerator.VirtualVendorID, "default");
                Assert.Null(deletedRetStore);
            }
        }


        [Fact]
        public async Task DeleteMappingsAsync_Test()
        {
            var stoneProvider = new StoneProvider();
            foreach (var testmappings in stoneProvider.Controllers.Values)
            {
                var mapcol = new ControllerElementMappingProfile("Keyboard",
                           "TEST_CONTROLLER",
                           InputDriver.Keyboard,
                           IDeviceEnumerator.VirtualVendorID,
                           new XInputDeviceInstance(0).DefaultLayout);

                var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
                var elementStore = new ControllerElementMappingProfileStore(optionsBuilder);

                await elementStore.AddMappingsAsync(mapcol, "default");

                var retStore = await elementStore.GetMappingsAsync(mapcol.ControllerID,
                    InputDriver.Keyboard, mapcol.DeviceName, IDeviceEnumerator.VirtualVendorID, "default");
                Assert.NotNull(retStore);
                await elementStore.DeleteMappingsAsync(mapcol.ControllerID, InputDriver.Keyboard, mapcol.DeviceName,
                    IDeviceEnumerator.VirtualVendorID, "default");
                var deletedRetStore = elementStore.GetMappings(mapcol.ControllerID, InputDriver.Keyboard,
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
                var mapcol = new ControllerElementMappingProfile("Keyboard",
                            "TEST_CONTROLLER",
                            InputDriver.Keyboard,
                            IDeviceEnumerator.VirtualVendorID,
                            new XInputDeviceInstance(0).DefaultLayout);

                var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
                var elementStore = new ControllerElementMappingProfileStore(optionsBuilder);

                elementStore.AddMappings(mapcol, "default");
                elementStore.AddMappings(mapcol, "default2");

                Assert.Equal(2, elementStore.GetProfileNames(mapcol.ControllerID, InputDriver.Keyboard, mapcol.DeviceName,
                    IDeviceEnumerator.VirtualVendorID)
                    .Count());
                elementStore.DeleteMappings(mapcol.ControllerID, InputDriver.Keyboard, mapcol.DeviceName, 
                    IDeviceEnumerator.VirtualVendorID, "default");
                Assert.Single(elementStore.GetProfileNames(mapcol.ControllerID, InputDriver.Keyboard, mapcol.DeviceName,
                    IDeviceEnumerator.VirtualVendorID));
                elementStore.DeleteMappings(mapcol.ControllerID, mapcol.DeviceName, IDeviceEnumerator.VirtualVendorID);
                Assert.Empty(elementStore.GetProfileNames(mapcol.ControllerID, InputDriver.Keyboard, mapcol.DeviceName,
                    IDeviceEnumerator.VirtualVendorID));
            }
        }

        [Fact]
        public void AddKeyboardMappings_Test()
        {
            var stoneProvider = new StoneProvider();
            foreach (var testmappings in stoneProvider.Controllers.Values)
            {
                var mapcol = new ControllerElementMappingProfile("Keyboard",
                           "TEST_CONTROLLER",
                           InputDriver.Keyboard,
                           IDeviceEnumerator.VirtualVendorID,
                           new KeyboardDeviceInstance().DefaultLayout);

                var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
                var elementStore = new ControllerElementMappingProfileStore(optionsBuilder);

                elementStore.AddMappings(mapcol, "default");

                var retStore = elementStore.GetMappings(
                    mapcol.ControllerID, InputDriver.Keyboard, "Keyboard", 
                    IDeviceEnumerator.VirtualVendorID, "default");
                foreach (var element in retStore)
                {
                    Assert.Contains(element.LayoutElement, mapcol.Select(x => x.LayoutElement));
                    Assert.Equal(element.DeviceCapability,
                        mapcol.First(x => x.LayoutElement == element.LayoutElement).DeviceCapability);
                }
            }
        }

        [Fact]
        public async Task AddKeyboardMappingsAsync_Test()
        {
            var stoneProvider = new StoneProvider();
            foreach (var testmappings in stoneProvider.Controllers.Values)
            {
                var mapcol = new ControllerElementMappingProfile("Keyboard",
                           "TEST_CONTROLLER",
                           InputDriver.Keyboard,
                           IDeviceEnumerator.VirtualVendorID,
                           new KeyboardDeviceInstance().DefaultLayout);

                var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
                var elementStore = new ControllerElementMappingProfileStore(optionsBuilder);

                await elementStore.AddMappingsAsync(mapcol, "default");

                var retStore = await elementStore.GetMappingsAsync(
                    mapcol.ControllerID, InputDriver.Keyboard, "Keyboard",
                    IDeviceEnumerator.VirtualVendorID, "default");
                foreach (var element in retStore)
                {
                    Assert.Contains(element.LayoutElement, mapcol.Select(x => x.LayoutElement));
                    Assert.Equal(element.DeviceCapability,
                        mapcol.First(x => x.LayoutElement == element.LayoutElement).DeviceCapability);
                }
            }
        }

        [Fact]
        public void ControllerElementMappingActualLayout_Test()
        {
            var stone = new StoneProvider();
            var nes = stone.Controllers["NES_CONTROLLER"];
            var mapcol = new ControllerElementMappingProfile("Keyboard",
                nes,
                           InputDriver.Keyboard,
                           IDeviceEnumerator.VirtualVendorID,
                           new KeyboardDeviceInstance().DefaultLayout);
            Assert.Equal(DeviceCapability.None, mapcol[ControllerElement.Button0]);
            Assert.Equal(nes.Layout.Count(), mapcol.Count());
        }
    }
}
