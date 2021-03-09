using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.TestResources.Language.SFE001
{
    using Snowflake.Extensibility.Provisioning;
    using Snowflake.Extensibility.Provisioning.Standalone;

    public class NonAttributedStandalonePluginImpl : StandalonePlugin
    {
        public NonAttributedStandalonePluginImpl()
            : base(typeof(NonAttributedStandalonePluginImpl))
        {
        }
    }
}
