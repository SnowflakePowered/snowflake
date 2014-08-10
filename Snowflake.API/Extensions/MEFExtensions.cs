using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;

namespace Snowflake.Extensions
{
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
