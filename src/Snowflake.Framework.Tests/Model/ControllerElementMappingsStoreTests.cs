using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Snowflake.Input.Controller;
using Snowflake.Input.Device;
using Snowflake.Model.Database;
using Snowflake.Model.Database.Models;
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

                var retStore = elementStore.GetMappings(mapcol.ProfileGuid);

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

                var retStore = await elementStore.GetMappingsAsync(mapcol.ProfileGuid);

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
                elementStore.UpdateMappings(mapcol);

                mapcol[ControllerElement.ButtonB] = DeviceCapability.Axis0Positive;
                elementStore.UpdateMappings(mapcol);
                var retStore = elementStore.GetMappings(mapcol.ProfileGuid);

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
                elementStore.UpdateMappings(mapcol);

                mapcol[ControllerElement.ButtonB] = DeviceCapability.Axis0Positive;
                elementStore.UpdateMappings(mapcol);

                var retStore = await elementStore.GetMappingsAsync(mapcol.ProfileGuid);

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

                var retStore = elementStore.GetMappings(mapcol.ProfileGuid);
                Assert.NotNull(retStore);
                elementStore.DeleteMappings(mapcol.ProfileGuid);
                var deletedRetStore = elementStore.GetMappings(mapcol.ProfileGuid);
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

                var retStore = await elementStore.GetMappingsAsync(mapcol.ProfileGuid);
                Assert.NotNull(retStore);
                await elementStore.DeleteMappingsAsync(mapcol.ProfileGuid);
                var deletedRetStore = elementStore.GetMappings(mapcol.ProfileGuid);
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
                var mapcol2 = new ControllerElementMappingProfile("Keyboard",
                            "TEST_CONTROLLER",
                            InputDriver.Keyboard,
                            IDeviceEnumerator.VirtualVendorID,
                            new XInputDeviceInstance(0).DefaultLayout);

                var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
                var elementStore = new ControllerElementMappingProfileStore(optionsBuilder);

                elementStore.AddMappings(mapcol, "default");
                elementStore.AddMappings(mapcol2, "default2");

                Assert.Equal(2, elementStore.GetProfileNames(mapcol.ControllerID, InputDriver.Keyboard, mapcol.DeviceName,
                    IDeviceEnumerator.VirtualVendorID)
                    .Count());
                elementStore.DeleteMappings(mapcol.ProfileGuid);
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

                var retStore = elementStore.GetMappings(mapcol.ProfileGuid);
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

                var retStore = await elementStore.GetMappingsAsync(mapcol.ProfileGuid);
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
