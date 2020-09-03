using hdvatob.Data.Interfaces;
using hdvatob.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hdvatob.Data.Repository
{
    public class CttachveRepository : Repository<Cttachve>, ICttachveRepository
    {
        public CttachveRepository(hdvatobDbContext context) : base(context)
        {
        }

        public List<Cttachve> listCttachvebyId(string Idhoadon,string chinhanh)
        {
            return _context.Cttachve.Where(x => x.Idhoadon == Idhoadon && x.chinhanh==chinhanh).ToList();
        }
    }
}
