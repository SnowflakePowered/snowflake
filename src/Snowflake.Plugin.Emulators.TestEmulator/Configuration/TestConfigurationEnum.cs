using Snowflake.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Plugin.Emulators.TestEmulator.Configuration
{
    public enum TestConfigurationEnum
    {
        [SelectionOption("selectOne")]
        TestOne,
        [SelectionOption("selectTwo")]
        TestTwo,
        [SelectionOption("selectThree")]
        TestThree
    }
}
