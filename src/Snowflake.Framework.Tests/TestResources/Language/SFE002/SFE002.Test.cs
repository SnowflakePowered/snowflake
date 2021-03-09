using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.TestResources.Language.SFE001
{
    using Snowflake.Extensibility;
    using Snowflake.Extensibility.Provisioning;
    using Snowflake.Extensibility.Provisioning.Standalone;

    [Plugin("common")]
    public class BadStandalonePluginImpl : StandalonePlugin
    {
        public BadStandalonePluginImpl()
            : base(typeof(BadStandalonePluginImpl))
        {
        }
    }
}
