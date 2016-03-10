using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Snowflake.Utility;

namespace Snowflake.Information.Database
{
    internal class DapperTypeMappedDatabase : BaseDatabase
    {
        private static readonly IList<Assembly> mapperAssemblies = new List<Assembly>();
        public DapperTypeMappedDatabase(string fileName, Type mapper) : base(fileName)
        {
            DapperExtensions.DapperExtensions.SqlDialect = new DapperExtensions.Sql.SqliteDialect();
            DapperTypeMappedDatabase.AddMapperAssembly(mapper);
        }

        private static void AddMapperAssembly(Type type)
        {
            DapperTypeMappedDatabase.mapperAssemblies.Add(type.Assembly);
            DapperExtensions.DapperExtensions.SetMappingAssemblies(DapperTypeMappedDatabase.mapperAssemblies);
        }
    }
}
