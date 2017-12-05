using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensibility;
using Snowflake.Services;
using Snowflake.Support.PluginManager;
using Xunit;

namespace Snowflake.Services.Tests
{
    public interface ITestPlugin : IPlugin
    {
        bool Test();
    }

    public class PluginManagerTests
    {
    }
}
