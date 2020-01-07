using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Snowflake.Configuration.Serialization.Serializers
{
    public sealed class StringSerializationContext : IConfigurationSerializationContext<string>
    {
        private StringBuilder Builder { get; }
        private Stack<string> ContextStack { get; }

        public StringSerializationContext()
        {
            this.Builder = new StringBuilder();
            this.ContextStack = new Stack<string>();
        }

        public string EnterScope(string scopeName)
        {
            this.ContextStack.Push(scopeName);
            return this.ContextStack.Peek();
        }

        public string ExitScope()
        {
            if (this.ContextStack.TryPop(out string? scope)) return scope;
            return String.Empty;
        }

        public string[] GetFullScope()
        {
            return this.ContextStack.Reverse().ToArray();
        }

        public string GetCurrentScope()
        {
            if (this.ContextStack.TryPeek(out string? scope)) return scope;
            return String.Empty;
        }

        public int ScopeLevel => this.ContextStack.Count;

        public void Append(string content)
        {
            this.Builder.Append(content);
        }

        public string Result => this.Builder.ToString();
    }
}
