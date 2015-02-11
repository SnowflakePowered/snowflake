
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Tests.Fakes
{
    internal class FakePlatformPreferencesDatabase : Snowflake.Platform.IPlatformPreferenceDatabase
    {
        public void AddPlatform(Platform.IPlatformInfo platformInfo)
        {
            throw new NotImplementedException();
        }

        public Platform.IPlatformDefaults GetPreferences(Platform.IPlatformInfo platformInfo)
        {
            throw new NotImplementedException();
        }

        public void SetEmulator(Platform.IPlatformInfo platformInfo, string value)
        {
            throw new NotImplementedException();
        }

        public void SetScraper(Platform.IPlatformInfo platformInfo, string value)
        {
            throw new NotImplementedException();
        }

        public void SetIdentifier(Platform.IPlatformInfo platformInfo, string value)
        {
            throw new NotImplementedException();
        }
    }
}
