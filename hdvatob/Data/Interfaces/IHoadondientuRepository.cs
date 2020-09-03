using hdvatob.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hdvatob.Data.Interfaces
{
    public interface IHoadondientuRepository: IRepository<ChinhanhHoadondientu>
    {
        List<ChinhanhHoadondientu> dsChinhanhHoadondientu();
        int themChinhanhHoadondientu(ChinhanhHoadondientu entity );
        int capnhatChinhanhHoadondientu(ChinhanhHoadondientu entity);
    }
}
