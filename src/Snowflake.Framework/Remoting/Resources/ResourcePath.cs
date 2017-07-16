using Snowflake.Remoting.Marshalling;
using Snowflake.Remoting.Requests;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Snowflake.Remoting.Resources
{
    public class ResourcePath : IResourcePath
    {
        public string this[int i] => this.path[i];
        public IParameter this[string key] => this.parameters[key];
        public IEnumerable<IParameter> ResourceParameters => this.parameters.Values;
        public IEnumerable<string> PathNodes => this.path;

        private readonly IImmutableList<string> path;
        private readonly IImmutableDictionary<string, IParameter> parameters;
        public ResourcePath(IEnumerable<IParameter> parameters, params string[] path)
        {
            this.parameters = parameters.ToDictionary(p => p.Key, p => p).ToImmutableDictionary();
            this.path = ImmutableList.Create(path);
        }

        public bool Match(IRequestPath requestPath)
        {
            if (requestPath.PathNodes.Count != this.path.Count) return false;
            for (int i = 0; i < requestPath.PathNodes.Count; i++)
            {
                if (this.path[i].StartsWith(":")) continue;
                if (this.path[i] != requestPath.PathNodes[i]) return false;
            }
            return true;
        }

        public IEnumerable<ISerializedArgument> MatchArguments(IRequestPath requestPath)
        {
            if (!this.Match(requestPath)) yield break;
            for (int i = 0; i < requestPath.PathNodes.Count; i++)
            {
                if (!this.path[i].StartsWith(":")) continue;
                var parameter = this[this.path[i].Substring(1)];
                var value = requestPath.PathNodes[i];
                yield return new SerializedArgument(parameter.Key, value, parameter.Type);
            }

        }

    }
}
