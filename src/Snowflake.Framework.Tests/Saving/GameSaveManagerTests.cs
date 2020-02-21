using Snowflake.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Snowflake.Orchestration.Saving.Tests
{
    public class GameSaveManagerTests
    {
        [Fact]
        public async Task CreateProfile()
        {
            var profilesRoot = TestUtilities.GetTemporaryDirectory("GameSaveManager");
            IGameSaveManager manager = new GameSaveManager(profilesRoot);
            var profile = manager.CreateProfile("TestProfile", "sram", SaveManagementStrategy.Copy);
            var retrievedProfile = manager.GetProfiles().First();
            Assert.Equal(profile.ProfileName, retrievedProfile.ProfileName);
            Assert.Equal(profile.Guid, retrievedProfile.Guid);
            Assert.Equal(profile.ManagementStrategy, retrievedProfile.ManagementStrategy);
            Assert.Equal(profile.SaveType, retrievedProfile.SaveType);
        }

        [Fact]
        public async Task DeleteProfile()
        {
            var profilesRoot = TestUtilities.GetTemporaryDirectory("GameSaveManager");
            IGameSaveManager manager = new GameSaveManager(profilesRoot);
            var profile = manager.CreateProfile("TestProfile", "sram", SaveManagementStrategy.Copy);
            var retrievedProfile = manager.GetProfiles().First();
            Assert.Equal(profile.ProfileName, retrievedProfile.ProfileName);
            Assert.Equal(profile.Guid, retrievedProfile.Guid);
            Assert.Equal(profile.ManagementStrategy, retrievedProfile.ManagementStrategy);
            Assert.Equal(profile.SaveType, retrievedProfile.SaveType);
            manager.DeleteProfile(profile.Guid);
            Assert.Empty(manager.GetProfiles());
        }
    }
}
