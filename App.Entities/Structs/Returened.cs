using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.Structs
{
    public struct Returened
    {
        /// <summary>
        /// State for operation in db
        /// </summary>
        public bool State { get; set; }
        /// <summary>
        /// Error message if existed
        /// </summary>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// Row ID in Table that error happend with
        /// </summary>
        public dynamic RowID { get; set; }


        /// <summary>
        /// Clear Values
        /// </summary>
        public void Clear()
        {
            State = default(bool);
            ErrorMessage = default(string);
            RowID = default(dynamic);
        }
    };
}
