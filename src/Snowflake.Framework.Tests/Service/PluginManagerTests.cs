using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensibility;
using Xunit;
using Snowflake.Services;
using Snowflake.Support.PluginManager;

namespace Snowflake.Service.Tests
{

    public interface ITestPlugin : IPlugin
    {
        bool Test();
    }

    public class PluginManagerTests
    {
        
    }
}
