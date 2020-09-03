using hdvatob.Data.Model;
using hdvatob.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace hdvatob.Data.Interfaces
{
    public interface IHoadonRepository : IRepository<Hoadon>
    {
        IPagedList<Hoadon> ListHoadon(string searchString, string chinhanh, int? page);
        //string newStt(string maviettat);
        string newhdvat(string kyhieu);
        string newId(string chinhanh);

        List<DataTuVetour> listdatavetour(string tour, string tungay, string denngay, string tuyentq, string chinhanh);

      
        Hoadon getHoadonbyStt(string stt);

        //List<DataFromTourViewModel> listDataFromTour(string tour, string tungay, string denngay, string tuyentq, string chinhanh);

        //List<ChitietVetourViewModel> listChitietVetour(string tour,string serial);

        VetourBySerial GetVetourBySerial(string tour, string serial,string chinhanh);

        TienCoupon GetTienCoupon(string tour, decimal Idvetour);

        int huyhoadontrongthang(string idhoadon, string chinhanh);
       
    }
}
