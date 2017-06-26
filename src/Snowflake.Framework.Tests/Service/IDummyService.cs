using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Service
{
    interface IDummyService
    {
        string Test { get; }
    }
    
    class DummyService : IDummyService
    {
        public string Test => "Test";
    }
}
