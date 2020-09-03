using hdvatob.Data.Model;
using hdvatob.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hdvatob.Data.Interfaces
{
    public interface ICthdvatRepository:IRepository<cthdvat>
    {
        List<cthdvat> ListChitietHoadon(string Idhoadon,string chinhanh);

        List<cthdvat> ListChitietHoadonhuy(string Idhoadon,string chinhanh);

        List<DataFromTourViewModel> ListCtVetourBySerial(string tour,string serial);
    }
}
