using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Snowflake.Emulator
{
    public interface IEmulatorProvision
    {
        Guid ProvisionGuid { get; }
        DirectoryInfo ProvisionRoot { get; }
    }
}
