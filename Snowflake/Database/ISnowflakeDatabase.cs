using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Database
{
    public interface ISnowflakeDatabase
    {
        string FileName { get; }
        void CreateDatabase();
    }
}
