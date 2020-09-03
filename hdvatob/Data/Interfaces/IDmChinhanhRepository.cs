using hdvatob.Data.Model;
using hdvatob.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace hdvatob.Data.Interfaces
{
    public interface IDmChinhanhRepository:IRepository<dmChinhanh>
    {
        dmChinhanh getChinhanhById(string macn);
        List<ListChinhanh> GetListChinhanh();
       
    }
}
