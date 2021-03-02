using Snowflake.Filesystem;
using Snowflake.Orchestration.Saving.SaveProfiles;
using Snowflake.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Snowflake.Orchestration.Saving.Tests
{
    public class NoneStrategySaveProfileTests
    {
        [Fact]
        public async Task CreateSimple_Test()
        {
            var profileRoot = TestUtilities.GetTemporaryDirectory();
            var saveContents = TestUtilities.GetTemporaryDirectory();
            var extract = TestUtilities.GetTemporaryDirectory();

            saveContents.OpenFile("savecontent").WriteAllText("test content");

            var profileGuid = Guid.NewGuid();
            var profile = new NoneStrategySaveProfile(profileGuid, "Test", "testsave", profileRoot);
            var save = await profile.CreateSave(saveContents.AsReadOnly());
            var _save = await profile.CreateSave(save);
            var retrievedSave = profile.GetHeadSave();

            await retrievedSave.ExtractSave(extract);
            Assert.IsType<EmptySaveGame>(retrievedSave);
            Assert.NotEqual(save.CreatedTimestamp, retrievedSave.CreatedTimestamp);
        }
    }
}
