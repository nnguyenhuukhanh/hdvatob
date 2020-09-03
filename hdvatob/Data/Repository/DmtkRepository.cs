using hdvatob.Data.Interfaces;
using hdvatob.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hdvatob.Data.Repository
{
    public class DmtkRepository : Repository<dmtk>, IDmtkRepository
    {
        public DmtkRepository(hdvatobDbContext context) : base(context)
        {
        }

        public List<dmtk> Listtaikhoan()
        {
            return _context.dmtk.Where(x => x.sudung == true).ToList();
        }
    }
}
