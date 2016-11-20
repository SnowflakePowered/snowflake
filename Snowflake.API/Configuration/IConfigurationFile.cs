using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration
{
    public interface IConfigurationFile
    {
        string Destination { get; }
        string Key { get; }
        IBooleanMapping BooleanMapping { get; }
    }
}
