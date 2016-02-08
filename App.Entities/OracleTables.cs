using App.Entities.Models;
using App.Entities.Structs;
using App.Entities.IModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace App.Entities
{
    /// <summary>
    /// Designed by Eng:Mohamad Alsaid for call Oracle Tables
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OracleTables<T> : TModel, IRepository<T>
    {
        #region Variable
        OracleConnection conn;
        OracleCommand cmd;
        Returened _res;
        DbReturned<T> _returnedValues;
        Returened _returnedState;

        #endregion

        #region Constructor

        public OracleTables()
        {
            conn = new OracleConnection();
            conn.ConnectionString = "";//Connections.ConnectionString(1); // Default ConnectionString (First)
            cmd = new OracleCommand();
            cmd.Connection = conn;
            _res = new Returened();
            _returnedValues = new DbReturned<T>();
        }

        public OracleTables(string connectionString)
        {
            conn = new OracleConnection();
            conn.ConnectionString = connectionString;
            cmd = new OracleCommand();
            cmd.Connection = conn;
            _res = new Returened();
            _returnedValues = new DbReturned<T>();
        }

        #endregion

        #region Method

        /// <summary>
        /// Clear Info Sql Returned from Every Function
        /// </summary>
        public void InitParams()
        {
            // Set Sql Parameters values
            cmd.Parameters.Clear();
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;

            // Clear Obj returned from methods
            _returnedValues.Clear();
        }

        /// <summary>
        /// Stop Conection with Sql Database
        /// </summary>
        public void StopConnect()
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }

        /// <summary>
        /// Set Error and State Information for methods
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="RowID"></param>
        /// <returns></returns>
        public DbReturned<T> SetException(Exception exp, Int32 RowID)
        {
            // Set Parameters
            DbReturned<T> res = new DbReturned<T>();
            _returnedState.State = false;
            _returnedValues.RowEffected = 0;
            _returnedState.ErrorMessage = exp.Message;
            _returnedState.RowID = RowID;
            _returnedValues.Exp = exp;
            res.Returened = _returnedState;
            return res;
        }

        /// <summary>
        /// Select Data from Tables
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        public DbReturned<T> Select(StoredProcedure sp)
        {
            try
            {
                InitParams();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = sp.StoredProcedureName;
                if (sp.arr != null)
                {
                    for (int i = 0; i < sp.arr.Count; i++)
                    {
                        OracleParameter spData = new OracleParameter();
                        spData.ParameterName = sp.Params[i];
                        spData.Value = sp.arr[i];
                        cmd.Parameters.Add(spData);
                    }
                }
                List<T> results;
                using (OracleDataReader dr = cmd.ExecuteReader())
                {
                    results = new List<T>().FromDataReader(dr).ToList();
                }
                conn.Close();
                _returnedValues.Data = results;
                _returnedState.State = true;
                _returnedValues.RowEffected = results.Count;
                _returnedValues.Returened = _returnedState;
                return _returnedValues;


            }
            catch (Exception exp)
            {
                return SetException(exp, 0);
            }
            finally
            {
                StopConnect();
            }
        }


        /// <summary>
        /// Get All Rows from Table
        /// </summary>
        /// <returns></returns>
        public DbReturned<T> AllData()
        {
            try
            {
                InitParams();
                cmd.CommandType = CommandType.Text;
                Type TableType = typeof(T);
                string Query = string.Format("SELECT * FROM  {0}", TableType.Name);
                cmd.CommandText = Query;
                List<T> results;
                using (OracleDataReader dr = cmd.ExecuteReader())
                {
                    results = new List<T>().FromDataReader(dr).ToList();
                }
                conn.Close();
                _returnedValues.Data = results;
                _returnedState.State = true;
                _returnedValues.RowEffected = results.Count;
                _returnedValues.Returened = _returnedState;
                return _returnedValues;


            }
            catch (Exception exp)
            {
                return SetException(exp, 0);
            }
            finally
            {
                StopConnect();
            }
        }


        /// <summary>
        /// Insert Row to table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        public DbReturned<T> Insert(T param)
        {
            try
            {
                InitParams();
                cmd.CommandType = CommandType.Text;
                string query = string.Empty;
                string Columns = string.Empty;
                Type TableType = typeof(T);

                string parameters = string.Empty;
                for (int i = 0; i < TableType.GetProperties().Length; i++)
                {
                    if (i != 0)
                    {
                        var type = param.GetType()
                            .GetProperty(TableType.GetProperties()[i].Name).PropertyType;
                        if (type == typeof(System.DateTime))
                        {
                            var sqlFormattedDate = Convert.ToDateTime(param.GetType().GetProperty(TableType.GetProperties()[i].Name).GetValue(param, null))
                             .Date.ToString();
                            // Dealing with fuck in datetime in Oracle
                            parameters += "TO_DATE('" + Convert.ToDateTime(sqlFormattedDate).ToString("yyyy/MM/dd") + "', 'yyyy/mm/dd')  ,";

                            Columns += TableType.GetProperties()[i].Name + " ,";
                        }
                        else
                        {
                            parameters += " '" + param.GetType()
                             .GetProperty(TableType.GetProperties()[i].Name).GetValue(param, null) + "' ,";


                            Columns += TableType.GetProperties()[i].Name + " ,";
                        }
                    }
                }
                parameters = parameters.TrimEnd(',');
                Columns = Columns.TrimEnd(',');
                query = String.Format("INSERT INTO   {0} ({1}) VALUES( {2} )", TableType.Name, Columns, parameters); cmd.CommandText = query;
                cmd.ExecuteNonQuery();
                conn.Close();
                _returnedState.State = true;
                _returnedValues.RowEffected = 1;
                _returnedValues.Returened = _returnedState;
                return _returnedValues;

            }
            catch (Exception exp)
            {
                return SetException(exp, 0);
            }
            finally
            {
                StopConnect();
            }
        }

        /// <summary>
        /// Update Row in Table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        public DbReturned<T> Update(T param)
        {
            Int32 RowID = 0;
            try
            {
                InitParams();
                cmd.CommandType = CommandType.Text;
                string query = string.Empty;
                Type TableType = typeof(T);

                string parameters = string.Empty;
                for (int i = 0; i < TableType.GetProperties().Length; i++)
                {
                    if (i != 0)
                    {
                        var type = param.GetType()
                            .GetProperty(TableType.GetProperties()[i].Name).PropertyType;
                        if (type == typeof(System.DateTime))
                        {
                            var sqlFormattedDate = Convert.ToDateTime(param.GetType().GetProperty(TableType.GetProperties()[i].Name).GetValue(param, null))
                            .Date.ToString();
                            parameters += "TO_DATE('" + Convert.ToDateTime(sqlFormattedDate).ToString("yyyy/MM/dd") + "', 'yyyy/mm/dd')  ,";
                        }
                        else
                        {
                            parameters += TableType.GetProperties()[i].Name
                                + " =  '" + param.GetType()
                                .GetProperty(TableType.GetProperties()[i].Name).GetValue(param, null) + "' ,";
                        }

                    }
                    else
                    {
                        RowID = Convert.ToInt32(param.GetType()
                                .GetProperty(TableType.GetProperties()[i].Name).GetValue(param, null));
                    }
                }
                parameters = parameters.TrimEnd(',');

                string where = string.Empty;
                where += TableType.GetProperties()[0].Name + " = " +
                    param.GetType().GetProperty(TableType.GetProperties()[0].Name).GetValue(param, null);


                query = String.Format("UPDATE  {0} SET {1} WHERE {2}", TableType.Name, parameters, where);
                cmd.CommandText = query;

                cmd.ExecuteNonQuery();
                conn.Close();
                _returnedState.State = true;
                _returnedState.RowID = RowID;
                _returnedValues.RowEffected = 1;
                _returnedValues.Returened = _returnedState;
                return _returnedValues;

            }
            catch (Exception exp)
            {
                return SetException(exp, RowID);
            }
            finally
            {
                StopConnect();
            }
        }




        /// <summary>
        /// First Record in Table
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public DbReturned<T> FirstOrDefault()
        {
            try
            {
                InitParams();
                cmd.CommandType = CommandType.Text;


                Type TableType = typeof(T);
                cmd.CommandText = String.Format("SELECT TOP(1)* FROM {0}", TableType.Name);
                List<T> results;
                using (OracleDataReader dr = cmd.ExecuteReader())
                {
                    results = new List<T>().FromDataReader(dr).ToList();
                }
                conn.Close();
                _returnedState.State = true;
                _returnedValues.SingleData = results.FirstOrDefault();
                _returnedValues.RowEffected = 1;
                _returnedValues.Returened = _returnedState;
                return _returnedValues;

            }
            catch (Exception exp)
            {
                return SetException(exp, 0);
            }
            finally
            {
                StopConnect();
            }
        }




        /// <summary>
        /// Delete Row where PK is Int or Guid
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public DbReturned<T> Delete(dynamic ID)
        {
            try
            {
                InitParams();
                cmd.CommandType = CommandType.Text;


                Type TableType = typeof(T);
                cmd.CommandText = String.Format("DELETE FROM {0} WHERE {1} = '{2}'",
                    TableType.Name, TableType.GetProperties()[0].Name, ID);

                Int32 RowEffect = cmd.ExecuteNonQuery();
                conn.Close();
                _returnedState.State = true;
                _returnedState.RowID = ID;
                _returnedValues.RowEffected = RowEffect;
                _returnedValues.Returened = _returnedState;
                return _returnedValues;
            }
            catch (Exception exp)
            {
                return SetException(exp, 0);
            }
            finally
            {
                StopConnect();
            }
        }


        /// <summary>
        /// Last Record in Table
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public DbReturned<T> LastOrDefault()
        {
            try
            {
                InitParams();
                cmd.CommandType = CommandType.Text;
                Type TableType = typeof(T);

                cmd.CommandText = String.Format("SELECT TOP(1)* FROM {0} ORDER BY {1} DESC",
                    TableType.Name, TableType.GetProperties()[0].Name);
                List<T> results;
                using (OracleDataReader dr = cmd.ExecuteReader())
                {
                    results = new List<T>().FromDataReader(dr).ToList();
                }
                conn.Close();
                _returnedState.State = true;
                _returnedState.RowID = 0;
                _returnedValues.SingleData = results.FirstOrDefault();
                _returnedValues.RowEffected = 1;
                _returnedValues.Returened = _returnedState;
                return _returnedValues;

            }
            catch (Exception exp)
            {
                return SetException(exp, 0);
            }
            finally
            {
                StopConnect();
            }
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
            try
            {
                InitParams();
                cmd.CommandType = CommandType.Text;
                Type TableType = typeof(T);
                cmd.CommandText = String.Format("SELECT * FROM {0} WHERE {1} = '{2}'",
                    TableType.Name, TableType.GetProperties()[0].Name, ID);
                List<T> results;
                using (OracleDataReader dr = cmd.ExecuteReader())
                {
                    results = new List<T>().FromDataReader(dr).ToList();
                }
                conn.Close();

                _returnedState.State = true;
                _returnedState.RowID = 0;
                _returnedValues.SingleData = results.FirstOrDefault();
                _returnedValues.RowEffected = 1;
                _returnedValues.Returened = _returnedState;
                return _returnedValues;
            }
            catch (Exception exp)
            {
                return SetException(exp, 0);
            }
            finally
            {
                StopConnect();
            }
        }


        /// <summary>
        /// Get all rows that a specefic word inside cell
        /// </summary>
        /// <param name="ColumnName"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public DbReturned<T> Contains(Func func)
        {
            try
            {
                InitParams();
                cmd.CommandType = CommandType.Text;
                Type TableType = typeof(T);
                cmd.CommandText = String.Format("SELECT * FROM {0} WHERE {1} LIKE '%{2}%'",
                    TableType.Name, func.ColumnName, func.Value);
                List<T> results;
                using (OracleDataReader dr = cmd.ExecuteReader())
                {
                    results = new List<T>().FromDataReader(dr).ToList();
                }
                conn.Close();

                _returnedState.State = true;
                _returnedState.RowID = 0;
                _returnedValues.Data = results;
                _returnedValues.RowEffected = results.Count;
                _returnedValues.Returened = _returnedState;
                return _returnedValues;


            }
            catch (Exception exp)
            {
                return SetException(exp, 0);
            }
            finally
            {
                StopConnect();
            }
        }




        /// <summary>
        /// Not Supported in Oracle
        /// </summary>
        /// <param name="trigger"></param>
        /// <returns></returns>
        public DbReturned<T> Trigger(Trigger trigger)
        {
            return new DbReturned<T>()
            {
                Returened = new Returened() { ErrorMessage = " Not Supported", State = false }
            };
        }

        /// <summary>
        /// Not Supported in Oracle
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public DbReturned<T> Transaction(Trans param)
        {
            return new DbReturned<T>()
            {
                Returened = new Returened() { ErrorMessage = " Not Supported", State = false }
            };
        }
        /// <summary>
        /// Not Supported in Oracle
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        public DbReturned<T> Execute(StoredProcedure sp)
        {
            return new DbReturned<T>()
            {
                Returened = new Returened() { ErrorMessage = " Not Supported", State = false }
            };
        }


        #endregion

        #region Function

        /// <summary>
        /// Build Insert Statement
        /// </summary>
        /// <param name="D"></param>
        /// <returns></returns>
        public string GetInsertStatement(dynamic D)
        {
            string query = string.Empty;


            Type TableType = D.GetType();
            string parameters = string.Empty;
            for (int i = 0; i < TableType.GetProperties().Length; i++)
            {
                if (i != 0)
                {
                    parameters += " '" + D.GetType().GetProperty(TableType.GetProperties()[i].Name).GetValue(D, null) + "' ,";

                }
            }
            parameters = parameters.TrimEnd(',');
            query = String.Format("INSERT INTO   {0} VALUES( {1} )", TableType.Name, parameters);
            return query;
        }


        /// <summary>
        /// Buils Update Statement
        /// </summary>
        /// <param name="D"></param>
        /// <returns></returns>
        public string GetUpdateStatement(dynamic D)
        {
            string query = string.Empty;

            Type TableType = D.GetType();

            string parameters = string.Empty;
            for (int i = 0; i < TableType.GetProperties().Length; i++)
            {
                if (i != 0)
                {
                    parameters += TableType.GetProperties()[i].Name
                        + " =  '" + D.GetType().GetProperty(TableType.GetProperties()[i].Name).GetValue(D, null) + "' ,";

                }
            }
            parameters = parameters.TrimEnd(',');

            string where = string.Empty;
            where += TableType.GetProperties()[0].Name + " = " +
                D.GetType().GetProperty(TableType.GetProperties()[0].Name).GetValue(D, null);


            query = String.Format("UPDATE  {0} SET {1} WHERE {2}", TableType.Name, parameters, where);
            return query;
        }

        /// <summary>
        /// Build Delete Statement
        /// </summary>
        /// <param name="D"></param>
        /// <returns></returns>
        public string GetDeleteStatement(dynamic D)
        {
            string query = string.Empty;
            Type TableType = D.GetType();
            query = String.Format("DELETE FROM   {0}  WHERE {1} = '{2}'", TableType.Name,
                TableType.GetProperties()[0].Name,
                D.GetType().GetProperty(TableType.GetProperties()[0].Name).GetValue(D, null));
            return query;
        }



        #endregion

        #region DataTable

        /// <summary>
        /// Return Data Table from Stored Procedure
        /// </summary>
        /// <param name="StoredProcedureName"></param>
        /// <returns></returns>
        public DbReturned<T> GetListDataTable(StoredProcedure sp)
        {
            try
            {
                InitParams();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = sp.StoredProcedureName;
                if (sp.arr != null)
                {
                    for (int i = 0; i < sp.arr.Count; i++)
                    {
                        OracleParameter spData = new OracleParameter();
                        spData.ParameterName = sp.Params[i];
                        spData.Value = sp.arr[i];
                        cmd.Parameters.Add(spData);
                    }
                }

                DataTable lResults = new DataTable();
                OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                adapter.Fill(lResults);

                _returnedState.State = true;
                _returnedState.RowID = 0;
                _returnedValues.dataTable = lResults;
                _returnedValues.RowEffected = lResults.Rows.Count;
                _returnedValues.Returened = _returnedState;
                return _returnedValues;
            }
            catch (Exception exp)
            {
                return SetException(exp, 0);
            }
            finally
            {
                StopConnect();
            }
        }


        /// <summary>
        /// Convet Select * from Table to DataTable
        /// </summary>
        /// <returns></returns>
        public DbReturned<T> GetListDataTable()
        {
            try
            {
                cmd.Parameters.Clear();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                Type TableType = typeof(T);
                string Query = string.Format("SELECT * FROM  {0}", TableType.Name);
                cmd.CommandText = Query;

                DataTable lResults = new DataTable();
                OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                adapter.Fill(lResults);
                _returnedState.State = true;
                _returnedState.RowID = 0;
                _returnedValues.dataTable = lResults;
                _returnedValues.RowEffected = lResults.Rows.Count;
                _returnedValues.Returened = _returnedState;
                return _returnedValues;
            }
            catch (Exception exp)
            {
                return SetException(exp, 0);
            }
            finally
            {
                StopConnect();
            }
        }

        #endregion

        ~OracleTables() // dtor or finalize
        {

        }
    }
}
