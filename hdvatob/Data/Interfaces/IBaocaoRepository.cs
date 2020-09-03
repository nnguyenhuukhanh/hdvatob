using hdvatob.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hdvatob.Data.Interfaces
{
    public interface IBaocaoRepository:IRepository<DoanhthungayhdViewModel>
    {
        IEnumerable<DoanhthungayhdViewModel> listDoanhthungayhd(string tungay, string denngay,string tour, string chinhanh);
        IEnumerable<NgayhuyhdViewModel> listNgayhuyhd(string tungay, string denngay,string tour, string chinhanh);
    }
}
