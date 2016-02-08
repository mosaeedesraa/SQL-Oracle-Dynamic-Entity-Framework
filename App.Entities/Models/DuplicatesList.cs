using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.Models
{
    public class DuplicatesList : List<KeyValuePair<Func, string>>
    {
        /// <summary>
        /// Add Key and Value to  this list
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(Func key, string value)
        {
            var element = new KeyValuePair<Func, string>(key, value);
            this.Add(element);
        }
    }
}
