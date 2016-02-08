using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.IModels
{
    public interface IReflection
    {
        void FillObjectWithProperty(ref object objectTo, string propertyName, object propertyValue);
    }
}
