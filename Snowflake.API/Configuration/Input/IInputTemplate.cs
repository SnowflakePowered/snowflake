using System.Collections.Generic;
using Snowflake.Input.Controller;

namespace Snowflake.Configuration.Input
{
    public interface IInputTemplate<out T> : IInputTemplate, IConfigurationSection<T> where T : class, IInputTemplate<T>
    {
        T Template { get; }
    }

    public interface IInputTemplate : IConfigurationSection
    {
        int PlayerIndex { get; }
        new IDictionary<string, ControllerElement> Values { get; }
        new IEnumerable<IInputOption> Options { get; }
        ControllerElement this[ControllerElement virtualElement] { set; }
    }
}