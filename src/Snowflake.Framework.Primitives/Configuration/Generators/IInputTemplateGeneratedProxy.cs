using Snowflake.Input.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration.Generators
{
    /// <summary>
    /// Automatically implemented by generated configuration collection proxies
    /// </summary>
    public interface IInputTemplateGeneratedProxy
    {
        IReadOnlyDictionary<string, DeviceCapability> Values { get; }
        DeviceCapability this[string keyName] { set; }
    }
}
