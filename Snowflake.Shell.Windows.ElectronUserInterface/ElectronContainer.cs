using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensibility;
using Snowflake.Service;
using Snowflake.Service.Manager;

namespace Snowflake.Shell.Windows.ElectronUserInterface
{
    public class ElectronContainer : IPluginContainer
    {
        public void Compose(ICoreService coreService)
        {
            coreService.Get<IPluginManager>().Register(new ElectronRegistrar(coreService));
        }
    }
}
