using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration.Hotkey
{
    //todo write tests
    public interface IHotkeyTemplateStore
    {
        T GetTemplate<T>() where T : IHotkeyTemplate, new();
        void SetTemplate(IHotkeyTemplate template);
    }
}
