﻿using Newtonsoft.Json;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Services;
using Snowflake.Tests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Utility;
using Xunit;
using Snowflake.Persistence;

namespace Snowflake.Input.Tests
{
    public class MappedControllerElementCollectionStore
    {
        [Fact]
        public void GetProfileNames_Test()
        {
            var stoneProvider = new StoneProvider();
            var testmappings = stoneProvider.Controllers.First().Value;
            var realmapping =
                JsonConvert.DeserializeObject<ControllerLayout>(
                    TestUtilities.GetStringResource("InputMappings.xinput_device.json"));
            var mapcol = MappedControllerElementCollection.GetDefaultMappings(realmapping, testmappings);
            var elementStore = new SqliteMappedControllerElementCollectionStore(new SqliteDatabase(Path.GetTempFileName()));
            elementStore.SetMappingProfile(mapcol);
            var retStore = elementStore.GetMappingProfile(mapcol.ControllerId, mapcol.DeviceId);
            Assert.Equal("default", elementStore.GetProfileNames(mapcol.ControllerId, mapcol.DeviceId).First());
        }
        [Fact]
        public void AddMappedInputCollectionGamepad_Test()
        {
            var stoneProvider = new StoneProvider();
            foreach(var testmappings in stoneProvider.Controllers.Values)
            {
                var realmapping =
                 JsonConvert.DeserializeObject<ControllerLayout>(
                     TestUtilities.GetStringResource("InputMappings.xinput_device.json"));
                    var mapcol = MappedControllerElementCollection.GetDefaultMappings(realmapping, testmappings);
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
                var mapcol = MappedControllerElementCollection.GetDefaultMappings(realmapping, testmappings);
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
    }
}
