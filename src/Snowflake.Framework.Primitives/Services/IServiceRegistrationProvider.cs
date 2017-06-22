using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Services
{
    public interface IServiceRegistrationProvider
    {
        void RegisterService<T>(T serviceObject);
    }
}
