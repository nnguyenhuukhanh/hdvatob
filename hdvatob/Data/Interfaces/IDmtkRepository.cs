using hdvatob.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hdvatob.Data.Interfaces
{
    public interface IDmtkRepository:IRepository<dmtk>
    {
        List<dmtk> Listtaikhoan();
        
    }
}
