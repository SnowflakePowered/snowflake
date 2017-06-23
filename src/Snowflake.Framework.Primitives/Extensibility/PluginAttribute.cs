using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Extensibility
{

    /// <summary>
    /// All plugins must be marked with this attribute to be loaded properly.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class PluginAttribute : Attribute
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginAttribute"/> class.
        /// </summary>
        /// <param name="pluginName">The name of the plugin. Must be the same as in plugin.json</param>
        public PluginAttribute(string pluginName)
        {
            this.PluginName = pluginName;
        }

        /// <summary>
        /// The name of the plugin. Must be the same as in plugin.json
        /// </summary>
        public string PluginName { get; }
        /// <summary>
        /// The name of the plugin author
        /// </summary>
        public string Author { get; set; } = "Generic Author";
        /// <summary>
        /// A short description of the plugin
        /// </summary>
        public string Description { get; set; } = "No Description Set";
        /// <summary>
        /// The version of the plugin
        /// </summary>
        public Version Version { get; set; } = new Version(0, 0, 0);
    }
}
