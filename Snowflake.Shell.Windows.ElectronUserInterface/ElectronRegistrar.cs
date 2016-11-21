using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Services;
using Snowflake.Extensibility;

namespace Snowflake.Shell.Windows.ElectronUserInterface
{
    [Plugin("ui-electron-win64")]
    public class ElectronRegistrar : Plugin
    {
        public ElectronRegistrar(ICoreService coreInstance)
            : base(coreInstance.AppDataDirectory)
        {
            //Because we rely on plugin data path, we can't simply register this as a service.
            coreInstance.RegisterService<IUserInterface>(new ElectronInterface(coreInstance, this.PluginDataPath));
        }
    }
}
    

