using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;


namespace Snowflake.Extensibility.Configuration
{
    public interface IPluginConfigOption
    {
        /// <summary>
        /// The config option key
        /// </summary>
        string Key { get; }
        /// <summary>
        /// Description
        /// </summary>
        string Description { get; }
        /// <summary>
        /// A list of valid select options
        /// </summary>
        IList<IPluginSelectOption> SelectValues { get; }
        /// <summary>
        /// The minimum value permitted if this is an INT_FLAG type
        /// 0 if no minimum or not INT_FLAG
        /// </summary>
        int RangeMin { get; }
        /// <summary>
        /// The maximum value permitted if this is an INT_FLAG type
        /// 0 if no maximum or not INT_FLAG
        /// </summary>
        int RangeMax { get; }
        /// <summary>
        /// The type of configuration flag
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        PluginOptionType Type { get; }
    }
}
