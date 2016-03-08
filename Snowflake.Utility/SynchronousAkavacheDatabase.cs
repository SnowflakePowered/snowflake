using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Akavache;
using Akavache.Sqlite3;
using Akavache.Internal;

namespace Snowflake.Utility
{
    /// <summary>
    /// Exposes a synchronous wrapper over an akavache database
    /// </summary>
    public abstract class SynchronousAkavacheDatabase : IDisposable
    {
        private readonly SQLitePersistentBlobCache blobCache;
        public string FileName { get; }
        protected SynchronousAkavacheDatabase(string fileName) 
        {
            this.FileName = fileName;

            if (!File.Exists(this.FileName))
            {
                SQLiteConnection.CreateFile(this.FileName);
            }
            this.blobCache = new SQLitePersistentBlobCache(this.FileName);
        }

        public async Task<T> GetObjectAsync<T>(string key)
        {
            return await this.blobCache.GetObject<T>(key);
        }

        public async Task<IEnumerable<T>> GetAllObjectsAsync<T>()
        {
            return await this.blobCache.GetAllObjects<T>();
        }

        public async Task<IDictionary<string, T>> GetObjectsAsync<T>(IEnumerable<string> keys)
        {
            return await this.blobCache.GetObjects<T>(keys);

        }

        public async Task InsertObjectAsync<T>(string key, T value)
        {
            await this.blobCache.InsertObject(key, value);
        }

        public async Task InsertObjectsAsync<T>(IDictionary<string, T> keyValuePairs)
        {
            await this.blobCache.InsertObjects(keyValuePairs);
        }

        public async Task DeleteObjectAsync<T>(string key)
        {
            await this.blobCache.InvalidateObject<T>(key);
        }

        public async Task DeleteObjectsAsync<T>(IEnumerable<string> keys)
        {
            await this.blobCache.InvalidateObjects<T>(keys);
        }

        public T GetObject<T>(string key)
        {
            return this.GetObjectAsync<T>(key).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public IEnumerable<T> GetAllObjects<T>()
        {
            return this.GetAllObjectsAsync<T>().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public IDictionary<string, T> GetObjects<T>(IEnumerable<string> keys)
        {
            return this.GetObjectsAsync<T>(keys).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public void InsertObject<T>(string key, T value)
        {
            this.InsertObjectAsync(key, value).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public void InsertObjects<T>(IDictionary<string, T> keyValuePairs)
        {
            this.InsertObjectsAsync(keyValuePairs).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public void DeleteObject<T>(string key)
        {
           this.DeleteObjectAsync<T>(key).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public void DeleteObjects<T>(IEnumerable<string> keys)
        {
            this.DeleteObjectsAsync<T>(keys).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            Task.Run(async () =>
            {
                await this.blobCache.Flush();
                this.blobCache.Dispose();
                await this.blobCache.Shutdown;
            }).ConfigureAwait(false).GetAwaiter().GetResult();

        }
    }
}
