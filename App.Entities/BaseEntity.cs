using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Entities.IModels;
using App.Entities.Structs;
using App.Entities.Models;
namespace App.Entities
{
    public enum Databases
    {
        Oracle = 1,
        Sql = 2
    }
    public class BaseEntity<T>
        where T : class , new()
    {
        IRepository<T> repository = null;
        public BaseEntity()
        {
            repository = new SqlTables<T>();
        }


        public BaseEntity(string connectionString, Databases database)
        {
            if (database == Databases.Sql)
                repository = new SqlTables<T>(connectionString);
            else repository = new OracleTables<T>(connectionString);
        }

        /// <summary>
        /// Get All Rows from Table
        /// </summary>
        /// <returns></returns>
        public DbReturned<T> AllData()
        {
            return repository.AllData();
        }

        /// <summary>
        /// Get All Rows from Table async
        /// </summary>
        /// <returns></returns>
        public async Task<DbReturned<T>> AllDataAsync()
        {
            Func<DbReturned<T>> asyncData = delegate
            {
                return repository.AllData();
            };
            return await Task.Run(() => asyncData());
        }

        /// <summary>
        /// Get all rows that a specefic word inside cell
        /// </summary>
        /// <param name="ColumnName"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public DbReturned<T> Contains(Func func)
        {
            return repository.Contains(func);
        }

        /// <summary>
        /// Delete Row where PK is Int or Guid
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public DbReturned<T> Delete(dynamic ID)
        {
            return repository.Delete(ID);
        }

        /// <summary>
        /// Delete Row where PK is Int or Guid async
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async Task<DbReturned<T>> DeleteAsync(dynamic ID)
        {
            Func<DbReturned<T>> asyncData = delegate
            {
                return repository.Delete(ID);
            };
            return await Task.Run(() => asyncData());
        }

        /// <summary>
        ///  Execute Function with Parameter Out
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        public DbReturned<T> Execute(StoredProcedure sp)
        {
            return repository.Execute(sp);
        }

        /// <summary>
        ///  Execute Function with Parameter Out async
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async Task<DbReturned<T>> ExecuteAsync(StoredProcedure sp)
        {
            Func<DbReturned<T>> asyncData = delegate
            {
                return repository.Execute(sp);
            };
            return await Task.Run(() => asyncData());
        }


        /// <summary>
        /// Find Record by ID
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="ColumnName"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public DbReturned<T> Find(dynamic ID)
        {
            return repository.Find(ID);
        }


        /// <summary>
        /// First Record in Table
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public DbReturned<T> FirstOrDefault()
        {
            return repository.FirstOrDefault();
        }

        /// <summary>
        /// Convet Select * from Table to DataTable
        /// </summary>
        /// <returns></returns>
        public DbReturned<T> GetListDataTable()
        {
            return repository.GetListDataTable();
        }


        /// <summary>
        /// Return Data Table from Stored Procedure
        /// </summary>
        /// <param name="StoredProcedureName"></param>
        /// <returns></returns>
        public DbReturned<T> GetListDataTable(StoredProcedure sp)
        {
            return repository.GetListDataTable(sp);
        }

        /// <summary>
        /// Insert Row to table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        public DbReturned<T> Insert(T param)
        {
            return repository.Insert(param);
        }

        /// <summary>
        /// Insert Row to table async
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<DbReturned<T>> InsertAsync(T param)
        {
            Func<DbReturned<T>> asyncData = delegate
            {
                return repository.Insert(param);
            };
            return await Task.Run(() => asyncData());
        }


        /// <summary>
        /// Last Record in Table
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public DbReturned<T> LastOrDefault()
        {
            return repository.LastOrDefault();
        }


        /// <summary>
        /// Select Data from Tables
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        public DbReturned<T> Select(StoredProcedure sp)
        {
            return repository.Select(sp);
        }

        /// <summary>
        /// Select Data from Tables async
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<DbReturned<T>> SelectAsync(StoredProcedure sp)
        {
            Func<DbReturned<T>> asyncData = delegate
            {
                return repository.Select(sp);
            };
            return await Task.Run(() => asyncData());
        }


        /// <summary>
        /// Execute Transaction
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public DbReturned<T> Transaction(Trans param)
        {
            return repository.Transaction(param);
        }

        /// <summary>
        /// Build trigger for table (Insert - Update - Delete)
        /// </summary>
        /// <param name="trigger"></param>
        /// <returns></returns>
        public DbReturned<T> Trigger(Trigger trigger)
        {
            return repository.Trigger(trigger);
        }

        /// <summary>
        /// Update Row in Table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        public DbReturned<T> Update(T param)
        {
            return repository.Update(param);
        }

        /// <summary>
        /// Update Row in Table async
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<DbReturned<T>> UpdateAsync(T param)
        {
            Func<DbReturned<T>> asyncData = delegate
            {
                return repository.Update(param);
            };
            return await Task.Run(() => asyncData());
        }

    }
}
