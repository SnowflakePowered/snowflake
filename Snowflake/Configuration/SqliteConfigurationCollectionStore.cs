using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;
using Snowflake.Configuration.Input;
using Snowflake.Records.Game;
using Snowflake.Utility;
using Dapper;
using FastMember;
using Newtonsoft.Json;

namespace Snowflake.Configuration
{
    public class SqliteConfigurationCollectionStore
    {
        private readonly SqliteDatabase backingDatabase;

        public SqliteConfigurationCollectionStore(SqliteDatabase sqliteDatabase)
        {
            this.backingDatabase = sqliteDatabase;
            this.CreateDatabase();
        }

        /// <summary>
        /// Enums are stored as their string representation
        /// Strings are stored as strings
        /// Primitives are stored as primitive
        /// </summary>
        private void CreateDatabase()
        {
            this.backingDatabase.CreateTable("configuration",
                "CollectionFilename TEXT",
                "SectionName TEXT",
                "GameRecordId TEXT",
                "OptionKey TEXT",
                "OptionValue TEXT",
                "PRIMARY KEY (CollectionFilename, SectionName, GameRecordId, OptionKey)");
        }

    }
}
