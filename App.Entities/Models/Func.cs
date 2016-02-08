using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.Models
{
    /// <summary>
    /// type of parameters
    /// </summary>
    public enum DataTypes
    {
        _guid,
        _int,
        _double,
        _decimal,
        _bool,
        _string
    }

    /// <summary>
    /// Class for Column Name and Value as Parameter
    /// </summary>
   public  class Func
    {
        public string ColumnName { get; set; }
        public string Value { get; set; }

        /// <summary>
        /// Just for Output Parameter in SP
        /// </summary>
        public DataTypes TypeOutParam { get; set; }
    }
}
