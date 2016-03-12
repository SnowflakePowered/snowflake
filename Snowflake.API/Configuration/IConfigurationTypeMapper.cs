using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration
{
    public interface IConfigurationTypeMapper
    {
        void Add<T>(Func<T, string> converter);
        string ConvertValue<T>(T value);
    }
}
