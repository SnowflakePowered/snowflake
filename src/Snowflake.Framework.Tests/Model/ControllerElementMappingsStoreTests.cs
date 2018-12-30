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
        public void AddMappedInputCollectionGamepad_Test()
        {
            var stoneProvider = new StoneProvider();
            foreach (var testmappings in stoneProvider.Controllers.Values)
            {
                var realmapping =
                 JsonConvert.DeserializeObject<ControllerLayout>(
                     TestUtilities.GetStringResource("InputMappings.xinput_device.json"));
                var mapcol = ControllerElementMappings.GetDefaultMappings(realmapping, testmappings);
                var elementStore = new SqliteMappedControllerElementCollectionStore(new SqliteDatabase(Path.GetTempFileName()));
                elementStore.SetMappingProfile(mapcol);
                var retStore = elementStore.GetMappingProfile(mapcol.ControllerId, mapcol.DeviceId);
                foreach (var element in retStore)
                    {
                        Assert.Contains(element.LayoutElement, mapcol.Select(x => x.LayoutElement));
                        Assert.Equal(element.DeviceElement, mapcol.First(x => x.LayoutElement == element.LayoutElement).DeviceElement);
                    }
            }
        }

        [Fact]
        public void AddMappedInputCollectionKeyboard_Test()
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

                //elementStore.SetMappingProfile(mapcol);
                //var retStore = elementStore.GetMappingProfile(mapcol.ControllerId, mapcol.DeviceId);
                //foreach (var element in retStore)
                //{
                //    Assert.Contains(element.LayoutElement, mapcol.Select(x => x.LayoutElement));
                //    Assert.Equal(element.DeviceElement, mapcol.First(x => x.LayoutElement == element.LayoutElement).DeviceElement);
                //}
            }
        }
    }
}
