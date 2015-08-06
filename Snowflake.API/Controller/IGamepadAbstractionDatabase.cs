using System.Collections.Generic;

namespace Snowflake.Controller
{
    public interface IGamepadAbstractionDatabase
    {
        IGamepadAbstraction GetGamepadAbstraction(string deviceName);
        void SetGamepadAbstraction(string deviceName, IGamepadAbstraction gamepadAbstraction);
        IGamepadAbstraction this[string deviceName] { get; set; }
        void RemoveGamepadAbstraction(string deviceName);
        IList<IGamepadAbstraction> GetAllGamepadAbstractions();
    }
}
