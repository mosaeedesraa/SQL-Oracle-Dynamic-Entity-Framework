using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.Models
{
    /// <summary>
    /// Where Condition
    /// </summary>
    public class Where
    {
        /// <summary>
        /// Conditions for where Constructore
        /// </summary>
        public DuplicatesList Conditions { get; set; }
        /// <summary>
        /// Where condition is contain groub by
        /// </summary>
        public bool isGroupBy { get; set; }
        /// <summary>
        /// Groub by Statement
        /// </summary>
        public string GroupBy { get; set; }
    }
}
