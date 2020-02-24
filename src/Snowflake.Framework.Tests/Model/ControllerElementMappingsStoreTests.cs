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
                var mapcol = new ControllerElementMappings("Xbox",
                           "TEST_CONTROLLER",
                           InputDriverType.XInput,
                           IDeviceEnumerator.VirtualVendorID,
                           new XInputDeviceInstance(0).DefaultLayout);

                var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");

                var elementStore = new ControllerElementMappingsStore(optionsBuilder);

                elementStore.AddMappings(mapcol, "default");

                var retStore = elementStore.GetMappings(mapcol.ControllerID, 
                    InputDriverType.XInput, 
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
                var mapcol = new ControllerElementMappings("Keyboard",
                           "TEST_CONTROLLER",
                           InputDriverType.Keyboard,
                           IDeviceEnumerator.VirtualVendorID,
                           new XInputDeviceInstance(0).DefaultLayout);

                var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");

                var elementStore = new ControllerElementMappingsStore(optionsBuilder);

               await elementStore.AddMappingsAsync(mapcol, "default");

                var retStore = await elementStore.GetMappingsAsync(mapcol.ControllerID,
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
        public async Task UpdateMappedInputCollectionGamepadAsync_Test()
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

                await elementStore.AddMappingsAsync(mapcol, "default");

                // map buttonA to buttonB.
                mapcol[ControllerElement.ButtonA] = DeviceCapability.Button1;

                await elementStore.UpdateMappingsAsync(mapcol, "default");
                var retStore = await elementStore.GetMappingsAsync(mapcol.ControllerID,
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
        public async Task DeleteMappingsAsync_Test()
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

                await elementStore.AddMappingsAsync(mapcol, "default");

                var retStore = await elementStore.GetMappingsAsync(mapcol.ControllerID,
                    InputDriverType.Keyboard, mapcol.DeviceName, IDeviceEnumerator.VirtualVendorID, "default");
                Assert.NotNull(retStore);
                await elementStore.DeleteMappingsAsync(mapcol.ControllerID, InputDriverType.Keyboard, mapcol.DeviceName,
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
        public async Task GetMultipleMappingsAsync_Test()
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

                await elementStore.AddMappingsAsync(mapcol, "default");
                await elementStore.AddMappingsAsync(mapcol, "default2");

                Assert.Equal(2, await elementStore.GetMappingsAsync(mapcol.ControllerID, mapcol.DeviceName,
                    IDeviceEnumerator.VirtualVendorID)
                    .CountAsync());
                await elementStore.DeleteMappingsAsync(mapcol.ControllerID, InputDriverType.Keyboard, mapcol.DeviceName,
                    IDeviceEnumerator.VirtualVendorID, "default");
                Assert.Single(elementStore.GetMappings(mapcol.ControllerID, mapcol.DeviceName, mapcol.VendorID));
                await elementStore.DeleteMappingsAsync(mapcol.ControllerID, mapcol.DeviceName, IDeviceEnumerator.VirtualVendorID);
                Assert.True(await elementStore.GetMappingsAsync(mapcol.ControllerID, mapcol.DeviceName, mapcol.VendorID).IsEmptyAsync());
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
                           new KeyboardDeviceInstance().DefaultLayout);

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

        [Fact]
        public async Task AddKeyboardMappingsAsync_Test()
        {
            var stoneProvider = new StoneProvider();
            foreach (var testmappings in stoneProvider.Controllers.Values)
            {
                var mapcol = new ControllerElementMappings("Keyboard",
                           "TEST_CONTROLLER",
                           InputDriverType.Keyboard,
                           IDeviceEnumerator.VirtualVendorID,
                           new KeyboardDeviceInstance().DefaultLayout);

                var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
                var elementStore = new ControllerElementMappingsStore(optionsBuilder);

                await elementStore.AddMappingsAsync(mapcol, "default");

                var retStore = await elementStore.GetMappingsAsync(
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

        [Fact]
        public void ControllerElementMappingActualLayout_Test()
        {
            var stone = new StoneProvider();
            var nes = stone.Controllers["NES_CONTROLLER"];
            var mapcol = new ControllerElementMappings("Keyboard",
                nes,
                           InputDriverType.Keyboard,
                           IDeviceEnumerator.VirtualVendorID,
                           new KeyboardDeviceInstance().DefaultLayout);
            Assert.Equal(DeviceCapability.None, mapcol[ControllerElement.Button0]);
            Assert.Equal(nes.Layout.Count(), mapcol.Count());
        }
    }
}
