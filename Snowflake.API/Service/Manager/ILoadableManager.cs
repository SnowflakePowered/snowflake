using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Service.Manager
{
    /// <summary>
    /// Generic interface for loadable managers such as IAjaxManager and IPluginManager
    /// </summary>
    public interface ILoadableManager
    {
        /// <summary>
        /// Loads all MEF plugins
        /// </summary>
        void LoadAll();
        /// <summary>
        /// A registry of all loaded plugins
        /// </summary>
        IReadOnlyDictionary<string, Type> Registry { get; }
        /// <summary>
        /// The location of the loadable plugins
        /// </summary>
        string LoadablesLocation { get;  }

    }
}
