using hdvatob.Data.Interfaces;
using hdvatob.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace hdvatob.Data.Repository
{
    public class SupplierRepository : Repository<supplier>, ISupplierRepository
    {
        public SupplierRepository(hdvatobDbContext context) : base(context)
        {
        }

        public supplier getSupplerByCode(string code, string chinhanh)
        {
            return _context.supplier.Where(x => x.chinhanh == chinhanh && x.code == code).FirstOrDefault();
        }

        public IPagedList<supplier> ListSupplier(string searchString, string chinhanh, int? page)
        {
            if (page.HasValue && page < 1)
                return null;

            var list = _context.supplier.AsQueryable();
            if (String.IsNullOrEmpty(searchString))
            {
                list = list.Where(x => x.chinhanh == chinhanh && x.active==true).OrderByDescending(x => x.name);
            }
            else
            {
                list = list.Where(x => x.chinhanh == chinhanh && x.active==true && (x.code.Contains(searchString)|| x.name.Contains(searchString) || x.nation.Contains(searchString) || x.taxcode.Contains(searchString) || x.address.Contains(searchString) || x.telephone.Contains(searchString))).OrderByDescending(x => x.name);
            }

            const int pageSize = 10;
            var listPaged = list.ToPagedList(page ?? 1, pageSize);

            // return a 404 if user browses to pages beyond last page. special case first page if no items exist
            if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
                return null;

            return listPaged;
        }

        public List<supplier> ListSupplierByCode(string search,string chinhanh)
        {
            return _context.supplier.Where(x =>x.chinhanh==chinhanh &&( x.code.Contains(search) || x.name.Contains(search))).ToList();
        }
    }
}
