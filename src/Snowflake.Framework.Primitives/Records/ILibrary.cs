using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.Metadata;

namespace Snowflake.Records
{
    /// <summary>
    /// Represents a generic library of metadata assignable items
    /// </summary>
    /// <typeparam name="T">The type of metadata assignable item</typeparam>
    public interface ILibrary<T>
    {
        /// <summary>
        /// Adds or updates a record to the library
        /// </summary>
        /// <param name="record"></param>
        void Set(T record);

        /// <summary>
        /// Adds or updates a list of records to the library
        /// </summary>
        /// <param name="records"></param>
        void Set(IEnumerable<T> records);

        /// <summary>
        /// Removes a record from the library
        /// </summary>
        /// <param name="record">The record to remove</param>
        void Remove(T record);

        /// <summary>
        /// Removes a set of records from the library
        /// </summary>
        /// <param name="records">The records to remove</param>
        void Remove(IEnumerable<T> records);

        /// <summary>
        /// Removes a record by Guid lookup
        /// </summary>
        /// <param name="guid">The guid of the record</param>
        void Remove(Guid guid);

        /// <summary>
        /// Remove a set of IDs
        /// </summary>
        /// <param name="guids"></param>
        void Remove(IEnumerable<Guid> guids);

        /// <summary>
        /// Gets a record from the libary
        /// </summary>
        /// <param name="guid">The guid to get</param>
        /// <returns>The record</returns>
        T Get(Guid guid);

        /// <summary>
        /// Gets multiple records from the library
        /// </summary>
        /// <param name="guids">The list of guids</param>
        /// <returns></returns>
        IEnumerable<T> Get(IEnumerable<Guid> guids);

        /// <summary>
        /// Gets all the records in the library
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetAllRecords();
    }
}
