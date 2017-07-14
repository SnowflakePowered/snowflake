using Snowflake.Framework.Remoting.Marshalling;
using Snowflake.Framework.Remoting.Requests;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Snowflake.Framework.Remoting.Resources
{
    public class ResourcePath
    {
        public string this[int i] => this.path[i];
        public Parameter this[string key] => this.parameters[key];
        public IEnumerable<Parameter> ResourceParameters => this.parameters.Values;
        public IEnumerable<string> PathNodes => this.path;

        private readonly IImmutableList<string> path;
        private readonly IImmutableDictionary<string, Parameter> parameters;
        public ResourcePath(IEnumerable<Parameter> parameters, params string[] path)
        {
            this.parameters = parameters.ToDictionary(p => p.Key, p => p).ToImmutableDictionary();
            this.path = ImmutableList.Create(path);
        }

        public bool Match(RequestPath requestPath)
        {
            if (requestPath.PathNodes.Count != this.path.Count) return false;
            for (int i = 0; i < requestPath.PathNodes.Count; i++)
            {
                if (this.path[i].StartsWith(":")) continue;
                if (this.path[i] != requestPath.PathNodes[i]) return false;
            }
            return true;
        }

        public IEnumerable<SerializedArgument> MatchArguments(RequestPath requestPath)
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
