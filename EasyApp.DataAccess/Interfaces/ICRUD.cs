using EasyApp.Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyApp.DataAccess.Interfaces
{
    interface ICRUD<T>
    {
        bool Insert(T Entity);
        bool Delete(T Entity);
        IEnumerable<T> GetAll();
        bool Update(T Entity);

    }
}
