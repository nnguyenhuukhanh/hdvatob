using hdvatob.Data.Model;
using hdvatob.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hdvatob.Data.Interfaces
{
    public interface IDsdangkyhdRepository:IRepository<dsdangkyhd>
    {
        dsdangkyhd getthongtinhd(string machinhanh, string kyhieu);
        Dsdangkyhoadondientu getDkhdById(int id);

        List<ListDangkyHoadon> listDangkyhoadon();

        int updateMainkey(int id);
        int updateSohoadon(int id,decimal sohoadon);

        List<Dsdangkyhoadondientu> listDsDangkyhoadondientu ();
        int themDangkyhoadondientu(Dsdangkyhoadondientu entity);
        int capnhatDangkyhoadondientu(Dsdangkyhoadondientu entity);
    }
}
