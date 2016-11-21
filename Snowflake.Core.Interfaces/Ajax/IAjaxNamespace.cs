using System.Collections.Generic;
using System.ComponentModel.Composition;
using Snowflake.Extensibility;

namespace Snowflake.Ajax
{
    /// <summary>
    /// Methods to be callable via the AjaxManager should be contained in an IAjaxNamespace plugin.
    /// <see cref="Snowflake.Ajax.BasePlugin"/> for the implementation
    /// </summary>
    public interface IAjaxNamespace : IPlugin
    {
        /// <summary>
        /// The generated dictionary of Javascript Methods
        /// </summary>
        IDictionary<string, IJSMethod> JavascriptMethods { get; }
    }
}
