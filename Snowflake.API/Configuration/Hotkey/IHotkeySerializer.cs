using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Input;

namespace Snowflake.Configuration.Hotkey
{
    public interface IHotkeySerializer
    {
        string SerializeKeyboard(IHotkeyTemplate template, IInputMapping inputMapping, int playerIndex = 0);

        string SerializeController(IHotkeyTemplate template, IInputMapping inputMapping,
            int playerIndex = 0);
    }

}
