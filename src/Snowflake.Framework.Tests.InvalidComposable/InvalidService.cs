using Snowflake.Tests.Composable;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Tests.InvalidComposable
{
    class InvalidService : IInvalidService
    {
        public string Test => "Test";
    }
}
