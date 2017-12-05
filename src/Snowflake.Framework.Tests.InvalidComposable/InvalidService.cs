using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Tests.Composable;

namespace Snowflake.Tests.InvalidComposable
{
    class InvalidService : IInvalidService
    {
        /// <inheritdoc/>
        public string Test => "Test";
    }
}
