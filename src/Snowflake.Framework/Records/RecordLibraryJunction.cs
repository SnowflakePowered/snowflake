using Dapper;
using Snowflake.Persistence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Snowflake.Records
{
    internal class RecordLibraryJunction <T1, T2> where T1 : IRecord where T2 : IRecord
    {
        private readonly ISqlDatabase backingDatabase;
        public string JunctionName { get; }
        private RecordLibrary<T1> ParentLibrary { get; }
        private RecordLibrary<T2> ChildLibrary { get; }
        public RecordLibraryJunction(ISqlDatabase database, RecordLibrary<T1> parentLibrary, RecordLibrary<T2> childLibrary)
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
                $"{this.ParentLibrary.LibraryName}_uuid REFERENCES {this.ParentLibrary.LibraryName}(uuid)",
                $"{this.ChildLibrary.LibraryName}_uuid REFERENCES {this.ChildLibrary.LibraryName}(uuid)",
                $"PRIMARY KEY ({this.ParentLibrary.LibraryName}_uuid, {this.ChildLibrary.LibraryName}_uuid)"
            );
        }

        //select files.* from games_files join files on files.uuid = files_uuid and games_uuid = x'45379b93e1eb064a9bb63deda29a242d'

        public void MakeRelation(T1 parentRelation, IEnumerable<T2> childRelation, IDbConnection dbConnection)
        {
            dbConnection.Execute($@"INSERT OR REPLACE into {this.JunctionName}({this.ParentLibrary.LibraryName}_uuid, {this.ChildLibrary.LibraryName}_uuid)
                                   VALUES (@parentUuid, @childUuid)", childRelation.Select( c => new { parentUuid = parentRelation.Guid, childUuid = c.Guid} ));
        }
      
        public void DeleteAllRelations(T1 parentRelation, IDbConnection dbConnection)
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
