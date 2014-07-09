using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Snowflake.Core
{
    public static class IDictionaryExtensions
    {
        public static bool ContainsKeyWithValue<KeyType, KeyValue>(
            this IDictionary<KeyType, ValueType> Dictionary,
            KeyType Key, ValueType Value)
        {
            return (Dictionary.ContainsKey(Key) && Dictionary[Key].Equals(Value));
        }
    }

    public static class MEFExtensions
    {
        public static IEnumerable<T> GetExportedValues<T>(this CompositionContainer Container,
            Func<IDictionary<string, object>, bool> Predicate)
        {
            var result = new List<T>();

            foreach (var PartDef in Container.Catalog.Parts)
            {
                foreach (var ExportDef in PartDef.ExportDefinitions)
                {
                    if (ExportDef.ContractName == typeof(T).FullName)
                    {
                        if (Predicate(ExportDef.Metadata))
                            result.Add((T)PartDef.CreatePart().GetExportedValue(ExportDef));
                    }
                }
            }

            return result;
        }
    }
}
