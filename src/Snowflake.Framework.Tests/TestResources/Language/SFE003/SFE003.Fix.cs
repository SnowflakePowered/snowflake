using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.TestResources.Language.SFE001
{
    using Snowflake.Loader;
    using Snowflake.Services;
    
    public interface TestService
    {

    }

    public class TestComposable : IComposable
    {
        [ImportService(typeof(TestService))]
        public void Compose(IModule composableModule, IServiceRepository serviceContainer)
        {
            serviceContainer.Get<TestService>();
        }
    }
}
