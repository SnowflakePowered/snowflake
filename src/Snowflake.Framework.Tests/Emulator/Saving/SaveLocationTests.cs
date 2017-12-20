using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Snowflake.Support.ExecutionSupport;
using Xunit;

namespace Snowflake.Execution.Saving.Tests
{
    public class SaveLocationTests
    {
        [Fact]
        public void SaveLocationSerialization_Test()
        {
            var location = new SaveLocation(Guid.NewGuid(), "sram", new DirectoryInfo(Path.GetTempPath()), Guid.NewGuid(), DateTimeOffset.Now);
            string serialized = JsonConvert.SerializeObject(location.ToManifest());
            ISaveLocation deserialized = JsonConvert.DeserializeObject<SaveLocationManifest>(serialized).ToSaveLocation();
            Assert.Equal(location.LocationGuid, deserialized.LocationGuid);
        }
    }
}
