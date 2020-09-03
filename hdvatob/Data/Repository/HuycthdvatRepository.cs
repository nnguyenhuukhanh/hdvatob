using hdvatob.Data.Interfaces;
using hdvatob.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hdvatob.Data.Repository
{
    public class HuycthdvatRepository : Repository<Huycthdvat>, IHuycthdvatRepository
    {
        public HuycthdvatRepository(hdvatobDbContext context) : base(context)
        {
        }

        public List<Huycthdvat> Listhuycthdvat(string Idhoadon,string chinhanh)
        {
            return _context.Huycthdvat.Where(x => x.Idhoadon == Idhoadon && x.chinhanh==chinhanh).OrderBy(x => x.Id).ToList();
        }

    }
}
