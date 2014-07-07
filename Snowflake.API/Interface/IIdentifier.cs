using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Snowflake.API.Interface
{
    public interface IIdentifier
    {
        string IdentifyRom(string fileName);

        string IdentifyRom()
    }
}
