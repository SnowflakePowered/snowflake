using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Serialization
{
    public interface IConfigurationSerializationContext<T>
    {
        T Result { get; }
        T EnterScope(string scope);
        T ExitScope();
        T[] GetFullScope();
        T GetCurrentScope();

        int ScopeLevel { get; }
        void Append(T content);
    }
}
