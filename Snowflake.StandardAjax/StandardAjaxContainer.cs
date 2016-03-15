using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Ajax;
using Snowflake.Extensibility;
using Snowflake.Service;
using Snowflake.Service.Manager;

namespace Snowflake.StandardAjax
{
    public class StandardAjaxContainer : IPluginContainer
    {
        public void Compose(ICoreService coreInstance)
        {
            coreInstance.Get<IPluginManager>().Register<IAjaxNamespace>(new StandardAjax(coreInstance));
        }
    }
}
