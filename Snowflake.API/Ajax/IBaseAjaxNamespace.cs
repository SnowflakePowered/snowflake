using System.Collections.Generic;
using System.ComponentModel.Composition;
using Snowflake.Plugin;

namespace Snowflake.Ajax
{
    /// <summary>
    /// Methods to be callable via the AjaxManager should be contained in an IBaseAjaxNamespace plugin.
    /// <see cref="Snowflake.Ajax.BasePlugin"/> for the implementation
    /// </summary>
    [InheritedExport(typeof(IBaseAjaxNamespace))]
    public interface IBaseAjaxNamespace : IBasePlugin
    {
        /// <summary>
        /// The generated dictionary of Javascript Methods
        /// </summary>
        IDictionary<string, IJSMethod> JavascriptMethods { get; }
    }
}
