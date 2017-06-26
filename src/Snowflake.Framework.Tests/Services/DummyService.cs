using Snowflake.Tests.Composable;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Services
{
    class DummyService : IDummyComposable
    {
        public string Test => "Test";
    }
}
