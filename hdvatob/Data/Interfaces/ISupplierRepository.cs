using hdvatob.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace hdvatob.Data.Interfaces
{
    public interface ISupplierRepository:IRepository<supplier>
    {
        IPagedList<supplier> ListSupplier(string searchString, string chinhanh, int? page);

        List<supplier> ListSupplierByCode(string code,string chinhanh);

        supplier getSupplerByCode(string code, string chinhanh);

    }
}
