using Snowflake.Input.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration.Internal
{
    /// <summary>
    /// Automatically implemented by generated configuration collection proxies
    /// </summary>
    public interface IInputConfigurationTemplate
    {
        IReadOnlyDictionary<string, DeviceCapability> GetValueDictionary();
        DeviceCapability this[string keyName] { set; }
    }
}
