using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
namespace Snowflake.Plugin
{
    /// <summary>
    /// A plugin that is not an emulator, identifier, ajax or scraper should inherit IGeneralPlugin.
    /// It has no extra functionality from IBasePlugin, however simply inheriting from IBasePlugin will not
    /// allow the plugin to be exported and loaded as a plugin. Inheriting from IGeneralPlugin tells the IPluginManager
    /// that the plugin is not specialized.
    /// </summary>
    [InheritedExport(typeof(IGeneralPlugin))]
    public interface IGeneralPlugin : IBasePlugin
    {
    }
}
