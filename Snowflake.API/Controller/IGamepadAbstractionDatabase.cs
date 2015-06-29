using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Emulator.Input.Constants;
namespace Snowflake.Controller
{
    public interface IGamepadAbstractionDatabase
    {
        IGamepadAbstraction GetGamepadAbstraction(string deviceName);
        void SetGamepadAbstraction(string deviceName, IGamepadAbstraction gamepadAbstraction);
        void RemoveGamepadAbstraction(string deviceName);
        IList<IGamepadAbstraction> GetAllGamepadAbstractions();
    }
}
