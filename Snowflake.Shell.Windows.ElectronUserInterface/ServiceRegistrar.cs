using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Service;
using Snowflake.Plugin;

namespace Snowflake.Shell.Windows.ElectronUserInterface
{
    public class ServiceRegistrar : GeneralPlugin
    {
        [ImportingConstructor]
        public ServiceRegistrar([Import("coreInstance")] ICoreService coreInstance)
            : base(Assembly.GetExecutingAssembly(), coreInstance)
        {
            this.CoreInstance.RegisterService(new ElectronInterface(coreInstance, this.PluginDataPath));
        }
    }
}
    

