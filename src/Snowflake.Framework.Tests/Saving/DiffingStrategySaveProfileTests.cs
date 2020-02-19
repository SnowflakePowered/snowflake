using Snowflake.Filesystem;
using Snowflake.Orchestration.Saving.SaveProfiles;
using Snowflake.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Snowflake.Orchetsration.Saving.Tests
{
    public class DiffStrategySaveProfileTests
    {
        [Fact]
        public async Task CreateSimple_Test()
        {
            var profileRoot = TestUtilities.GetTemporaryDirectory();
            var saveContents = TestUtilities.GetTemporaryDirectory();

            saveContents.OpenFile("savecontent").WriteAllText("test content");

            var profileGuid = Guid.NewGuid();
            var profile = new DiffingStrategySaveProfile(profileGuid, "Test", "testsave", profileRoot);
            var save = await profile.CreateSave(saveContents);

            var retrievedSave = profile.GetHeadSave();

            Assert.Equal(save.CreatedTimestamp, retrievedSave.CreatedTimestamp);
        }

        [Fact]
        public async Task CreateDirectory_Test()
        {
            var profileRoot = TestUtilities.GetTemporaryDirectory();
            var saveContents = TestUtilities.GetTemporaryDirectory();

            saveContents.OpenFile("savecontent").WriteAllText("test content");
            saveContents.OpenDirectory("nestedcontent").OpenFile("savecontentnested").WriteAllText("test content 2");
            saveContents.OpenDirectory("nestedcontent").OpenDirectory("nestedtwo").OpenFile("savecontentnested").WriteAllText("test content 3");

            var profileGuid = Guid.NewGuid();
            var profile = new DiffingStrategySaveProfile(profileGuid, "Test", "testsave", profileRoot);
            var save = await profile.CreateSave(saveContents);

            saveContents.OpenFile("copyContent").WriteAllText("copy content");

            var retrievedSave = profile.GetHeadSave();

            await profile.CreateSave(saveContents);
            Assert.Equal(save.CreatedTimestamp, retrievedSave.CreatedTimestamp);

            // todo: ensure directory structure
        }


        [Fact]
        public async Task ExtractDirectory_Test()
        {
            var profileRoot = TestUtilities.GetTemporaryDirectory();
            var saveContents = TestUtilities.GetTemporaryDirectory();

            saveContents.OpenFile("savecontent").WriteAllText("test content");
            saveContents.OpenDirectory("nestedcontent").OpenFile("savecontentnested").WriteAllText("test content 2");
            saveContents.OpenDirectory("nestedcontent").OpenDirectory("nestedtwo").OpenFile("savecontentnested").WriteAllText("test content 3");

            var profileGuid = Guid.NewGuid();
            var profile = new DiffingStrategySaveProfile(profileGuid, "Test", "testsave", profileRoot);
            var save = await profile.CreateSave(saveContents);

            saveContents.OpenFile("copyContent").WriteAllText("copy content");

            var retrievedSave = profile.GetHeadSave();

            var created = await profile.CreateSave(saveContents);

            var extractPath = TestUtilities.GetTemporaryDirectory(nameof(DiffingStrategySaveProfile) + ".extract");
            await created.ExtractSave(extractPath);
            Assert.Equal(save.CreatedTimestamp, retrievedSave.CreatedTimestamp);

            // todo: ensure directory structure
        }
    }
}
