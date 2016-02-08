using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.IModels
{
    public interface IIENumerableExtensions<T>
    {
        IEnumerable<T> FromDataReader<T>( DbDataReader dr);
        T ConvertFromDBVal<T>(object obj);
    }
}
