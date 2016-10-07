using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FastMember;
using Snowflake.Configuration.Attributes;
using Snowflake.Records.Metadata;
using Snowflake.Utility;

namespace Snowflake.Configuration.Records
{
    public class ConfigurationValue : IConfigurationValue
    {
        public static readonly Guid OrphanedConfigurationGuid = new Guid("b1c873f8-e9c6-4c59-bc3b-8da869ad99d3");
        public IMetadataCollection Metadata { get; }
        public Guid Guid { get; }

        public object Value
        {
            get { return this.accessor[this.instance, this.keyName]; }
            set { setter(value); }
        }

        public Type Type { get; }
        public Guid Record { get; }
        public Guid Section { get; }

        private readonly object instance;
        private readonly TypeAccessor accessor;
        private readonly Action<object> setter;
        private string keyName;
        internal ConfigurationValue(PropertyInfo propertyInfo, object instance, Guid sectionGuid, Guid gameRecord)
        {
            this.instance = instance;
            this.keyName = propertyInfo.Name;
            this.setter = value =>
            {
                if (value != null) this.accessor[this.instance, propertyInfo.Name] = value;
                else propertyInfo.SetValue(this.instance, null); // setting a value to null will incur an nre unless with reflection
            };
            this.Type = propertyInfo.PropertyType;
            this.accessor = TypeAccessor.Create(instance.GetType());
            this.Record = gameRecord;
            this.Guid = GuidUtility.Create(this.Record, $"{sectionGuid}::{keyName}");
            this.Section = sectionGuid;
        }
    }
}
