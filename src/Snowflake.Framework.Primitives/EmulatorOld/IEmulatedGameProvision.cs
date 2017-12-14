using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Snowflake.EmulatorOld
{
    public interface IEmulatedGameProvision
    {
        Guid ProvisionGuid { get; }
        DirectoryInfo ProvisionDirectory { get; }
    }
}
