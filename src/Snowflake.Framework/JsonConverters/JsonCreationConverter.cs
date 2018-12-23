using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Snowflake.JsonConverters
{
    /// <summary>Base Generic JSON Converter that can help quickly define converters for specific types by automatically
    /// generating the CanConvert, ReadJson, and WriteJson methods, requiring the implementer only to define a strongly typed Create method.</summary>
    internal abstract class JsonCreationConverter<T> : JsonConverter
    {
        [ThreadStatic]
        static bool writerDisabled;

        // Disables the converter in a thread-safe manner.
        private bool WriterDisabled { get { return writerDisabled; } set { writerDisabled = value; } }

        /// <inheritdoc/>
        public override bool CanWrite => !this.WriterDisabled;

        /// <summary>Create an instance of objectType, based properties in the JSON object</summary>
        /// <param name="objectType">type of object expected</param>
        /// <param name="jObject">contents of JSON object that will be deserialized</param>
        protected abstract T Create(Type objectType, JObject jObject);

        /// <summary>
        /// Gives a chance to transform the object before being serialized
        /// </summary>
        /// <param name="value">The value to transform</param>
        /// <returns>The transformed object graph to be serialized</returns>
        protected virtual object? Transform(T value)
        {
            return value;
        }

        /// <summary>Determines if this converted is designed to deserialization to objects of the specified type.</summary>
        /// <param name="objectType">The target type for deserialization.</param>
        /// <returns>True if the type is supported.</returns>
        public override bool CanConvert(Type objectType)
        {
            // FrameWork 4.5
            // return typeof(T).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
            // Otherwise
            return typeof(T).IsAssignableFrom(objectType);
        }

        /// <summary>Parses the json to the specified type.</summary>
        /// <param name="reader">Newtonsoft.Json.JsonReader</param>
        /// <param name="objectType">Target type.</param>
        /// <param name="existingValue">Ignored</param>
        /// <param name="serializer">Newtonsoft.Json.JsonSerializer to use.</param>
        /// <returns>Deserialized Object</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            // Load JObject from stream
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject
            T target = this.Create(objectType, jObject);

            return target;
        }

        /// <summary>Serializes to the specified type</summary>
        /// <param name="writer">Newtonsoft.Json.JsonWriter</param>
        /// <param name="value">Object to serialize.</param>
        /// <param name="serializer">Newtonsoft.Json.JsonSerializer to use.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JObject jo = new JObject();
            Type type = value.GetType();
            foreach (var prop in type.GetProperties())
            {
                if (!prop.CanRead)
                {
                    continue;
                }

                object propVal = prop.GetValue(value, null);
                if (propVal != null)
                {
                    jo.Add(prop.Name, JToken.FromObject(propVal, serializer));
                }
            }

            jo.WriteTo(writer);
        }
    }
}