using System.Collections.Generic;
using Snowflake.Remoting.Marshalling;
using Snowflake.Remoting.Requests;

namespace Snowflake.Remoting.Resources
{
    public interface IResourcePath
    {
        string this[int i] { get; }
        IParameter this[string key] { get; }

        IEnumerable<string> PathNodes { get; }
        IEnumerable<IParameter> ResourceParameters { get; }

        bool Match(IRequestPath requestPath);
        IEnumerable<ISerializedArgument> SerializePathArguments(IRequestPath requestPath);
    }
}