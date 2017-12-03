using Snowflake.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowflake.Records.Metadata;

namespace Snowflake.Records
{
    /// <summary>
    /// An Sqlite backed record library that allows junctions.
    /// </summary>
    /// <typeparam name="TRecord"></typeparam>
    internal abstract class SqliteRecordLibrary<TRecord> : IRecordLibrary<TRecord> where TRecord : IRecord 
    {
        private readonly ISqlDatabase backingDatabase;
        public string LibraryName { get; }
        public abstract IMetadataLibrary MetadataLibrary { get; }

        public SqliteRecordLibrary(ISqlDatabase database, string libraryName, params string[] columns)
        {
            this.backingDatabase = database;
            this.LibraryName = libraryName;
            this.CreateDatabase(columns);
        }

        private void CreateDatabase(string[] columns)
        {
            this.backingDatabase.CreateTable(this.LibraryName,
                columns.Prepend("uuid UUID PRIMARY KEY").ToArray());
        }

     
        /// <summary>
        /// Creates a junction between two record libraries
        /// </summary>
        /// <typeparam name="T">The type of the other record</typeparam>
        /// <param name="junctionWith">The other record library to junction with.</param>
        /// <returns>A record junction</returns>
        protected RecordLibraryJunction<TRecord, T> CreateJunction<T>(SqliteRecordLibrary<T> junctionWith) where T: IRecord
        {
            return new RecordLibraryJunction<TRecord, T>(this.backingDatabase, this, junctionWith);
        }

        public abstract IEnumerable<TRecord> SearchByMetadata(string key, string likeValue);
        public abstract IEnumerable<TRecord> GetByMetadata(string key, string exactValue);
        public abstract void Set(TRecord record);
        public abstract void Set(IEnumerable<TRecord> records);
        public abstract void Remove(TRecord record);
        public abstract void Remove(IEnumerable<TRecord> records);
        public abstract void Remove(Guid guid);
        public abstract void Remove(IEnumerable<Guid> guids);
        public abstract TRecord Get(Guid guid);
        public abstract IEnumerable<TRecord> Get(IEnumerable<Guid> guids);
        public abstract IEnumerable<TRecord> GetAllRecords();
    }
}
