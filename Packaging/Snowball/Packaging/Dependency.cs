using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet;

namespace Snowball.Packaging
{
    [JsonConverter(typeof (DependencyStringConverter))]
    public class Dependency
    {
        public string PackageName { get; }
        public SemanticVersion DependencyVersion { get; }

        public Dependency(string dependencyString)
        {
            var _dependency = dependencyString.Split('@');
            this.PackageName = _dependency[0];
            this.DependencyVersion = _dependency.Length == 2 ? new SemanticVersion(_dependency[1]) : null;
        }

        public override string ToString()
        {
            return this.DependencyVersion == null ? this.PackageName : $"{this.PackageName}@{this.DependencyVersion}";
        }
    }

    public class DependencyStringConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            // Load JObject from stream.  Turns out we're also called for null arrays of our objects,
            // so handle a null by returning one.
            var jToken = JToken.Load(reader);
            if (jToken.Type == JTokenType.Null)
                return null;
            if (jToken.Type != JTokenType.String)
                throw new InvalidOperationException("Json: expected string ; got " + jToken.Type);
            return new Dependency((string) jToken);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof (Dependency);
        }
    }
}