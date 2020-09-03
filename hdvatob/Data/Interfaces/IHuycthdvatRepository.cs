using hdvatob.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hdvatob.Data.Interfaces
{
    public  interface IHuycthdvatRepository:IRepository<Huycthdvat>
    {
        List<Huycthdvat> Listhuycthdvat(string Idhoadon,string chinhanh);
    }
}
