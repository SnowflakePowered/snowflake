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
            var stoneProvider = new StoneProvider();
            var testmappings = stoneProvider.Controllers.First().Value;
            var realmapping =
                JsonConvert.DeserializeObject<ControllerLayout>(
                    TestUtilities.GetStringResource("InputMappings.xinput_device.json"));
            var mapcol = ControllerElementMappings.GetDefaultMappings(realmapping, testmappings);

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
                var realmapping =
                    JsonConvert.DeserializeObject<ControllerLayout>(
                        TestUtilities.GetStringResource("InputMappings.xinput_device.json"));
                var mapcol = ControllerElementMappings.GetDefaultMappings(realmapping, testmappings);

                var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");

                var elementStore = new ControllerElementMappingsStore(optionsBuilder);

                elementStore.AddMappings(mapcol, "default");

                var retStore = elementStore.GetMappings(mapcol.ControllerId, mapcol.DeviceId, "default");

                foreach (var element in retStore)
                {
                    Assert.Contains(element.LayoutElement, mapcol.Select(x => x.LayoutElement));
                    Assert.Equal(element.DeviceElement,
                        mapcol.First(x => x.LayoutElement == element.LayoutElement).DeviceElement);
                }
            }
        }

        [Fact]
        public void UpdateMappedInputCollectionGamepad_Test()
        {
            var stoneProvider = new StoneProvider();
            foreach (var testmappings in stoneProvider.Controllers.Values)
            {
                var realmapping =
                    JsonConvert.DeserializeObject<ControllerLayout>(
                        TestUtilities.GetStringResource("InputMappings.xinput_device.json"));
                var mapcol = ControllerElementMappings.GetDefaultMappings(realmapping, testmappings);

                var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");

                var elementStore = new ControllerElementMappingsStore(optionsBuilder);

                elementStore.AddMappings(mapcol, "default");

                // map buttonA to buttonB.
                mapcol[ControllerElement.ButtonA] = ControllerElement.ButtonB;

                elementStore.UpdateMappings(mapcol, "default");
                var retStore = elementStore.GetMappings(mapcol.ControllerId, mapcol.DeviceId, "default");

                foreach (var element in retStore)
                {
                    Assert.Contains(element.LayoutElement, mapcol.Select(x => x.LayoutElement));
                    Assert.Equal(element.DeviceElement,
                        mapcol.First(x => x.LayoutElement == element.LayoutElement).DeviceElement);
                }

                Assert.Equal(ControllerElement.ButtonB, retStore[ControllerElement.ButtonA]);
            }
        }

        [Fact]
        public void DeleteMappings_Test()
        {
            var stoneProvider = new StoneProvider();
            foreach (var testmappings in stoneProvider.Controllers.Values)
            {
                var realmapping =
                    JsonConvert.DeserializeObject<ControllerLayout>(
                        TestUtilities.GetStringResource("InputMappings.keyboard_device.json"));
                var mapcol = ControllerElementMappings.GetDefaultMappings(realmapping, testmappings);

                var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
                var elementStore = new ControllerElementMappingsStore(optionsBuilder);

                elementStore.AddMappings(mapcol, "default");

                var retStore = elementStore.GetMappings(mapcol.ControllerId, mapcol.DeviceId, "default");
                Assert.NotNull(retStore);
                elementStore.DeleteMappings(mapcol.ControllerId, mapcol.DeviceId, "default");
                var deletedRetStore = elementStore.GetMappings(mapcol.ControllerId, mapcol.DeviceId, "default");
                Assert.Null(deletedRetStore);
            }
        }

        [Fact]
        public void GetMultipleMappings_Test()
        {
            var stoneProvider = new StoneProvider();
            foreach (var testmappings in stoneProvider.Controllers.Values)
            {
                var realmapping =
                    JsonConvert.DeserializeObject<ControllerLayout>(
                        TestUtilities.GetStringResource("InputMappings.keyboard_device.json"));
                var mapcol = ControllerElementMappings.GetDefaultMappings(realmapping, testmappings);

                var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
                var elementStore = new ControllerElementMappingsStore(optionsBuilder);

                elementStore.AddMappings(mapcol, "default");
                elementStore.AddMappings(mapcol, "default2");

                Assert.Equal(2, elementStore.GetMappings(mapcol.ControllerId, mapcol.DeviceId).Count());
                elementStore.DeleteMappings(mapcol.ControllerId, mapcol.DeviceId, "default");
                Assert.Single(elementStore.GetMappings(mapcol.ControllerId, mapcol.DeviceId));
                elementStore.DeleteMappings(mapcol.ControllerId, mapcol.DeviceId);
                Assert.Empty(elementStore.GetMappings(mapcol.ControllerId, mapcol.DeviceId));
            }
        }

        [Fact]
        public void AddKeyboardMappings_Test()
        {
            var stoneProvider = new StoneProvider();
            foreach (var testmappings in stoneProvider.Controllers.Values)
            {
                var realmapping =
                    JsonConvert.DeserializeObject<ControllerLayout>(
                        TestUtilities.GetStringResource("InputMappings.keyboard_device.json"));
                var mapcol = ControllerElementMappings.GetDefaultMappings(realmapping, testmappings);

                var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
                var elementStore = new ControllerElementMappingsStore(optionsBuilder);

                elementStore.AddMappings(mapcol, "default");

                var retStore = elementStore.GetMappings(mapcol.ControllerId, mapcol.DeviceId, "default");
                foreach (var element in retStore)
                {
                    Assert.Contains(element.LayoutElement, mapcol.Select(x => x.LayoutElement));
                    Assert.Equal(element.DeviceElement,
                        mapcol.First(x => x.LayoutElement == element.LayoutElement).DeviceElement);
                }
            }
        }
    }
}
