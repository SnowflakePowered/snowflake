using System;

namespace Snowflake.Configuration.Attributes
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = true)]
    public class ConfigurationFileAttribute : Attribute
    {
        public string Key { get; }
        public string FileName { get; }

        public ConfigurationFileAttribute(string key, string filename)
        {
            this.Key = key;
            this.FileName = filename;
        }
    }
}
