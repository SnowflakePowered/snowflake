using Dapper;
using Snowflake.Persistence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Snowflake.Records
{
    /// <summary>
    /// A junction between two record libraries
    /// </summary>
    /// <typeparam name="TParent">The parent library.</typeparam>
    /// <typeparam name="TChildren">The child library to junction with.</typeparam>
    internal class RecordLibraryJunction <TParent, TChildren> where TParent : IRecord where TChildren : IRecord
    {
        private readonly ISqlDatabase backingDatabase;
        public string JunctionName { get; }
        private SqliteRecordLibrary<TParent> ParentLibrary { get; }
        private SqliteRecordLibrary<TChildren> ChildLibrary { get; }
        public RecordLibraryJunction(ISqlDatabase database, SqliteRecordLibrary<TParent> parentLibrary, SqliteRecordLibrary<TChildren> childLibrary)
        {
            this.backingDatabase = database;
            this.JunctionName = $"{parentLibrary.LibraryName}_{childLibrary.LibraryName}";
            this.ParentLibrary = parentLibrary;
            this.ChildLibrary = childLibrary;
            this.CreateDatabase();
        }

        private void CreateDatabase()
        {
            this.backingDatabase.CreateTable(this.JunctionName,
                $"{this.ParentLibrary.LibraryName}_uuid UUID REFERENCES {this.ParentLibrary.LibraryName}(uuid)",
                $"{this.ChildLibrary.LibraryName}_uuid UUID REFERENCES {this.ChildLibrary.LibraryName}(uuid)",
                $"PRIMARY KEY ({this.ParentLibrary.LibraryName}_uuid, {this.ChildLibrary.LibraryName}_uuid)"
            );
        }


        public void MakeRelation(TParent parentRelation, IEnumerable<TChildren> childRelation, IDbConnection dbConnection)
        {
            dbConnection.Execute($@"INSERT OR REPLACE into {this.JunctionName}({this.ParentLibrary.LibraryName}_uuid, {this.ChildLibrary.LibraryName}_uuid)
                                   VALUES (@parentUuid, @childUuid)", childRelation.Select( c => new { parentUuid = parentRelation.Guid, childUuid = c.Guid} ));
        }
      
        public void DeleteAllRelations(TParent parentRelation, IDbConnection dbConnection)
        {
            dbConnection.Execute($@"DELETE FROM {this.JunctionName} WHERE {this.ParentLibrary.LibraryName}_uuid = @Guid", parentRelation);
        }
        public void DeleteAllRelations(Guid parentRelation, IDbConnection dbConnection)
        {
            dbConnection.Execute($@"DELETE FROM {this.JunctionName} WHERE {this.ParentLibrary.LibraryName}_uuid = @Guid", new { Guid = parentRelation });
        }

        public void DeleteAllRelations(IEnumerable<Guid> parentRelation, IDbConnection dbConnection)
        {
            dbConnection.Execute($@"DELETE FROM {this.JunctionName} WHERE {this.ParentLibrary.LibraryName}_uuid IN @Guids", new { Guids = parentRelation });
        }
    }
}
