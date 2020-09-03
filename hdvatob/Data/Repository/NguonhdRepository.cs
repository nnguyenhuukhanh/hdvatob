using hdvatob.Data.Interfaces;
using hdvatob.Data.Model;


namespace hdvatob.Data.Repository
{
    public class NguonhdRepository : Repository<Nguonhd>, INguonhdRepository
    {
        public NguonhdRepository(hdvatobDbContext context) : base(context)
        {
        }
    }
}
