using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Services
{
    public interface ILocalWebService
    {
        int Port { get; }
        string Protocol { get; }
        string Name { get; }
        void Start();
    }
}
