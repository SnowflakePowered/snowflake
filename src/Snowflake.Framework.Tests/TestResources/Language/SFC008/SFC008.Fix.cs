﻿namespace Snowflake.Framework.Tests.Configuration
{
    using Snowflake.Configuration;

    [ConfigurationCollection]
    public partial interface TestInterface
    {
        string TestProperty { set; get; }
    }
}
