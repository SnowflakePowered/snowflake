using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration.Hotkey
{
    /// <summary>
    /// Stores a hotkey template, differentiated by type name alone.
    /// </summary>
    public interface IHotkeyTemplateStore
    {
        /// <summary>
        /// Gets the hotkey template from the store of type T
        /// </summary>
        /// <typeparam name="T">The type of hotkey template</typeparam>
        /// <returns>The stored hotkey template</returns>
        T GetTemplate<T>() where T : IHotkeyTemplate, new();
        /// <summary>
        /// Sets a hotkey template to the store.
        /// </summary>
        /// <param name="template">The template to store.</param>
        void SetTemplate(IHotkeyTemplate template);
    }
}
