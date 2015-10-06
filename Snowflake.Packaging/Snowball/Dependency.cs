using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NuGet;

namespace Snowflake.Packaging.Snowball
{
    [JsonConverter(typeof(ToStringConverter))]
    public class Dependency
    {
        public string PackageName { get; }
        public SemanticVersion DependencyVersion { get; }

        public Dependency(string dependencyString)
        {
            var _dependency = dependencyString.Split('@');
            this.PackageName = _dependency[0];
            this.DependencyVersion = new SemanticVersion(_dependency[1]);
        }

        public override string ToString()
        {
            return $"{this.PackageName}@{this.DependencyVersion}";
        }


    }
    public class ToStringConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return false;
        }
    }
}
