using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Core.Manager.Interface
{
    public interface ILoadableManager
    {
        void LoadAll();
        IDictionary<string, Type> Registry { get; }
        string LoadablesLocation { get;  }

    }
}
