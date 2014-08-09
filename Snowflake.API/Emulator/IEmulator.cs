using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;

namespace Snowflake.Plugin.Interface
{
    [InheritedExport(typeof(IEmulator))]
    public interface IEmulator : IPlugin
    {
        void Run(string gameUUID);
    }
}
