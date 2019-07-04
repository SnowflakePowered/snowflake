using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Serialization.Serializers
{
    public static class StringSerializationContextExtensions
    {
        public static void AppendLine(this IConfigurationSerializationContext<string> @this, string content)
        {
            @this.Append($"{content}{Environment.NewLine}");
        }
    }
}
