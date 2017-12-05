using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Tests.Composable;

namespace Snowflake.Services
{
    class DummyService : IDummyComposable
    {
        /// <inheritdoc/>
        public string Test => "Test";
    }
}
