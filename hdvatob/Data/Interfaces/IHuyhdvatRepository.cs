using hdvatob.Data.Model;
using hdvatob.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace hdvatob.Data.Interfaces
{
    public interface IHuyhdvatRepository:IRepository<Huyhdvat>
    {
        IPagedList<Huyhdvat> Listhuyhoadon(string searchString, string chinhanh, int? page);

        string newStt(string maviettat);
        string newhdvat(string kyhieu);
        string newId();

        List<DataTuVetour> listdatatuhuytour(string tour, string tungay, string denngay, string tuyentq, string chinhanh);
        //List<DataFromTourViewModel> listdatatuhuytour(string tour, string tungay, string denngay, string tuyentq, string chinhanh);

    }
}
