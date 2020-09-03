using hdvatob.Data.Model;
using System.Collections.Generic;


namespace hdvatob.Data.Interfaces
{
    public interface IDmhttcRepository:IRepository<dmhttc>
    {
        List<dmhttc> ListHttc();
    }
}
