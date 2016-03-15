using System.ComponentModel.Composition;
using System.Reflection;
using Snowflake.Service;

namespace Snowflake.Extensibility
{
 
    [InheritedExport(typeof(IPluginContainer))]
    public interface IPluginContainer
    {
        void Compose(ICoreService coreInstance);
    }
}
