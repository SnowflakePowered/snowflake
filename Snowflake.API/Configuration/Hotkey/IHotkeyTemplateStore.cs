using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration.Hotkey
{
    public interface IHotkeyTemplateStore
    {
        /// <summary>
        /// Gets a hotkey template given the filename of the template, template filename and template types
        /// </summary>
        /// <param name="templateFilename">The filename of the template</param>
        /// <param name="templateType"></param>
        /// <returns></returns>
        T GetTemplate<T>(string templateFilename) where T : IHotkeyTemplate, new();
        T GetTemplate<T>() where T: IHotkeyTemplate, new();
        void SetTemplate(IHotkeyTemplate template);
    }
}
