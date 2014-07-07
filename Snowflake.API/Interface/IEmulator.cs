using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.API.Interface
{
    public interface IEmulator
    {
        void Run(string gameUUID);
    }
}
