using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Tooling.RestClient.Commands
{
    internal class CommandDiscovery
    {
        public  IList<(Type commandType, Type optionType)> Types { get; }
        public IDictionary<Type, object> Commands { get; } 
        public CommandDiscovery()
        {
            this.Types = this.DiscoverCommands().ToList();
            this.Commands = this.Types.ToDictionary(t => t.optionType, t => Instantiate.CreateInstance(t.commandType));
        }
      
        public int Execute(object parsed)
        {
            var optionType = parsed.GetType();
            return Task.Run(async () =>
            {
                var command = await (Task<int>)this.Commands[optionType].GetType().GetRuntimeMethod("Execute", new[] { optionType })
                   .Invoke(this.Commands[optionType], new object[] { parsed });
                return command;
            }).Result;
        }

        public IEnumerable<(Type, Type)> DiscoverCommands()
        {
            return (from types in Assembly.GetExecutingAssembly().GetTypes()
                            let interfaces = types.GetInterfaces()
                            from intf in interfaces
                            where intf.IsGenericType && intf.GetGenericTypeDefinition() == typeof(ICommand<>)
                            where types.GetConstructor(Type.EmptyTypes) != null
                            let optionType = intf.GetGenericArguments().First()
                            select (types, optionType));
        }
    }
}
