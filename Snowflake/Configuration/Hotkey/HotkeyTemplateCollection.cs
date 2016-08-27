using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration.Hotkey
{
    public class HotkeyTemplateCollection : IHotkeyTemplateCollection
    {
        public HotkeyTemplateCollection(IHotkeyTemplate keyboardTemplate, IHotkeyTemplate controllerTemplate)
        {
            this.KeyboardHotkeyTemplate = keyboardTemplate;
            this.ControllerHotkeyTemplate = controllerTemplate;
        }

        public IHotkeyTemplate KeyboardHotkeyTemplate { get; }
        public IHotkeyTemplate ControllerHotkeyTemplate { get; }
    }
}
