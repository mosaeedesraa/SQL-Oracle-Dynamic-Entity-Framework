using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.Models
{
    /// <summary>
    /// Transaction Class for IUD operations
    /// </summary>
    public class Trans
    {
        /// <summary>
        /// Transaction state is Insert
        /// </summary>
        public bool Insert { get; set; }
        /// <summary>
        /// Transaction state is Update
        /// </summary>
        public bool Update { get; set; }
        /// <summary>
        /// Transaction state is Deleet
        /// </summary>
        public bool Delete { get; set; }
        /// <summary>
        /// List od Codes or Parameters for this Transaction
        /// </summary>
        public List<dynamic> lstTypes { get; set; }
    }
}
