using hdvatob.Data.Interfaces;
using hdvatob.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace hdvatob.Data.Repository
{
    public class DmhttcRepository : Repository<dmhttc>,IDmhttcRepository
    {
        public DmhttcRepository(hdvatobDbContext context) : base(context)
        {
        }

        public List<dmhttc> ListHttc()
        {
            return _context.dmhttc.ToList();
        }
    }
}
