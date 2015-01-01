using System;
using System.Collections.Generic;
namespace Snowflake.Controller
{
    public interface IControllerProfile
    {
        string ControllerID { get; }
        IReadOnlyDictionary<string, string> InputConfiguration { get; }
        string PlatformID { get; }
        ControllerProfileType ProfileType { get; }
        IDictionary<string, dynamic> ToSerializable();
    }
}
