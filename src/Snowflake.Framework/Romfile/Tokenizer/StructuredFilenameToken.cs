using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Romfile.Tokenizer
{
    public class StructuredFilenameToken
    {
        public string Value { get; }
        public FieldType Type { get; }
        public NamingConvention NamingConvention { get; }

        public StructuredFilenameToken(string value, FieldType type, NamingConvention convention)
        {
            this.Value = value;
            this.Type = type;
            this.NamingConvention = convention;
        }
    }

    public enum FieldType
    {
        Title,
        Version,
        Date,
        Demo,
        Publisher,
        System,
        Video,
        Country,
        Language,
        CopyrightStatus,
        DevelopmentStatus,
        MediaType,
        MediaLabel,
        DumpInfo,
    }
}
