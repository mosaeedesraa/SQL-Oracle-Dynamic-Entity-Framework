using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.Models
{
    public class Trigger
    {
        /// <summary>
        /// Data base Name
        /// </summary>
        public string DataBaseName { get; set; }
        /// <summary>
        /// Trigger Name
        /// </summary>
        public string TriggerName { get; set; }
        /// <summary>
        /// Trigger will execute after insert T table
        /// </summary>
        public bool Insert { get; set; }
        /// <summary>
        /// Trigger will execute after update T table
        /// </summary>
        public bool Update { get; set; }
        /// <summary>
        /// Trigger will execute after delete T table
        /// </summary>
        public bool Delete { get; set; }
        /// <summary>
        /// code Trigger
        /// </summary>
        public string Code { get; set; }
    }
}
