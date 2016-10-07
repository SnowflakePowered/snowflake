using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Platform;
using Snowflake.Service;

namespace Snowflake.Exports
{
    public class Stone
    {
        private readonly ICoreService coreService;
        public Stone(ICoreService coreService)
        {
            this.coreService = coreService;
        }

        public IList<IPlatformInfo> GetPlatformInfos()
        {
            return this.coreService.Get<IStoneProvider>().Platforms.Values.ToList();
        }

    }
}
