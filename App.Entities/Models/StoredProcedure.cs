using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.Models
{
    /// <summary>
    /// Stored Procedure Parameteres and Name
    /// </summary>
    public class StoredProcedure
    {
        /// <summary>
        /// Stored Procedure Name
        /// </summary>
        public string StoredProcedureName { get; set; }
        /// <summary>
        /// Array of Stored Procedure Parameters values
        /// </summary>
        public ArrayList arr { get; set; }
        /// <summary>
        /// Array of Stored Procedure Parameters Names 
        /// </summary>
        public List<string> Params { get; set; }
        /// <summary>
        /// Stored Procedure Output Parameter
        /// </summary>
        public Func OutParam { get; set; }

     }
}
