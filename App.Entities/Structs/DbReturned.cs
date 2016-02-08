using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.Structs
{
    public struct DbReturned<T>
    {
        /// <summary>
        /// Whats is Error in Operation
        /// </summary>
        public Returened Returened { get; set; }
        /// <summary>
        /// List Data returned from SQL
        /// </summary>
        public List<T> Data { get; set; }

        /// <summary>
        /// Return one row from methods (FirstOrDefault - ..... )
        /// </summary>
        public T SingleData { get; set; }
        /// <summary>
        /// Numbers of rows effected by operation
        /// </summary>
        public Int32 RowEffected { get; set; }
        /// <summary>
        /// Execute Stored procedure results (IO or In) or Max - Min - Count
        /// Sum and Transaction
        /// </summary>
        public dynamic Return { get; set; }

        /// <summary>
        /// Get DataTable result
        /// </summary>
        public DataTable dataTable { get; set; }

        /// <summary>
        /// Error information if happend
        /// </summary>
        public Exception Exp { get; set; }

        /// <summary>
        /// Clear Values
        /// </summary>
        public void Clear()
        {
            Returened.Clear();
            Data = default(List<T>);
            SingleData = default(T);
            RowEffected = default(int);
            Return = default(dynamic);
            dataTable = default(DataTable);
            Exp = default(Exception);
        }

    }
}
