using Snowflake.Input.Controller;
using Snowflake.Model.Game;
using Snowflake.Support.StoneProviders;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Snowflake.Support.StoneProvider.JsonConverters
{
    internal class StoneDataConverter : JsonConverter<StoneData>
    {
        internal static ControllerLayoutConverter ControllerLayoutConverter = new ControllerLayoutConverter();
        internal static PlatformInfoConverter PlatformInfoConverter = new PlatformInfoConverter();

        public override StoneData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Dictionary<PlatformId, IPlatformInfo> platforms = new Dictionary<PlatformId, IPlatformInfo>();
            Dictionary<ControllerId, IControllerLayout> controllers = new Dictionary<ControllerId, IControllerLayout>();
            Version? version = null;
            
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    break;
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }

                string propertyName = reader.GetString();

                if (propertyName == "Controllers")
                {
                    reader.Read(); // StartObject Controllers
                    reader.Read(); // PropertyName
                    while (reader.TokenType == JsonTokenType.PropertyName)
                    {
                        string controllerId = reader.GetString();
                        reader.Read(); // StartObject
                        var parsedController = ControllerLayoutConverter.Read(ref reader, typeof(ControllerLayout), options);
                        if (parsedController.LayoutID != controllerId) throw new JsonException("Failed to match parsed controller ID");
                        controllers.Add(controllerId, parsedController);
                        reader.Read(); // EndObject
                    }
                }

                if (propertyName == "Platforms")
                {
                    reader.Read(); // StartObject Platforms
                    reader.Read(); // PropertyName
                    while (reader.TokenType == JsonTokenType.PropertyName)
                    {
                        string platformId = reader.GetString();
                        reader.Read(); // StartObject
                        var parsedPlatform = PlatformInfoConverter.Read(ref reader, typeof(PlatformInfo), options);
                        if (parsedPlatform.PlatformID != platformId) throw new JsonException("Failed to match parsed platform ID");
                        platforms.Add(platformId, parsedPlatform);
                        reader.Read(); // Move to next object. EndObject
                    }
                }

                if (propertyName == "version")
                {
                    reader.Read();
                    version = new Version(reader.GetString().Split('-')[0]);
                    reader.Read(); // Move to next object.
                }
            }
            return new StoneData(platforms, controllers, version!);
        }

        public override void Write(Utf8JsonWriter writer, StoneData value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
