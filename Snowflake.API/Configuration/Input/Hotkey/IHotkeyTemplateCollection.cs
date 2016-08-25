using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration.Input.Hotkey
{
    public interface IHotkeyTemplateCollection
    {
        IHotkeyTemplate KeyboardHotkeyTemplate { get; }
        IHotkeyTemplate ControllerHotkeyTemplate { get; }
    }
}
