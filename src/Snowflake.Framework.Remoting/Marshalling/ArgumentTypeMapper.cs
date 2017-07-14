using Snowflake.Framework.Remoting.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Framework.Remoting.Marshalling
{
    public abstract class ArgumentTypeMapper
    {
        public static ArgumentTypeMapper Default { get; }

        static ArgumentTypeMapper()
        {
            ArgumentTypeMapper.Default = new DefaultArgumentTypeMapper();
        }
        
        private readonly IDictionary<Type, Func<string, object>> typeMappers;
        protected ArgumentTypeMapper()
        {
            this.typeMappers = new Dictionary<Type, Func<string, object>>();
        }

        public void Add<T>(Func<string, T> converter)
        {
            this.typeMappers.Add(typeof(T), strValue => converter.Invoke(strValue));
        }

        public T ConvertValue<T>(string value) => (T)this.ConvertValue(typeof(T), value);

        public object ConvertValue(Type targetType, string value)
        {
            if (targetType == typeof(string)) return value;
            return this.typeMappers.ContainsKey(targetType) ?
                this.typeMappers[targetType].Invoke(value) :
                null;
        }

        public IEnumerable<TypedArgument> CastArguments(IEnumerable<SerializedArgument> pathArgs,
           IEnumerable<SerializedArgument> endpointArgs)
        {
            //ensures that endpointArgs never override path args.
            var endpointArgsWithoutPathArgs = endpointArgs.Where(e => !pathArgs.Any(p => p.Key == e.Key));
            return this.CastArguments(pathArgs.Concat(endpointArgsWithoutPathArgs));
        }

        private IEnumerable<TypedArgument> CastArguments(IEnumerable<SerializedArgument> arguments)
        {
            return (from arg in arguments
                    let castedValue = this.ConvertValue(arg.Type, arg.StringValue)
                    select new TypedArgument(arg.Key, castedValue, arg.Type));
        }
    }
}
