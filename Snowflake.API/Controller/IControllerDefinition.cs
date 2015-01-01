using System;
using System.Collections.Generic;
namespace Snowflake.Controller
{
    public interface IControllerDefinition
    {
        string ControllerID { get; }
        IReadOnlyDictionary<string, IControllerInput> ControllerInputs { get; }
    }
}
