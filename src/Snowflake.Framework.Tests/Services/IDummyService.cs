using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Services
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
