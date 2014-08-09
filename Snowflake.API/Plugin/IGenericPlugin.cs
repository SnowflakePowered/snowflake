using System.ComponentModel.Composition;

namespace Snowflake.Plugin
{
    [InheritedExport(typeof(IGenericPlugin))]
    public interface IGenericPlugin:IPlugin
    {
    }
}
