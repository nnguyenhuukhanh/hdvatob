using hdvatob.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hdvatob.Data.Interfaces
{
    public interface ITachveRepository:IRepository<Tachve>
    {
        //string newStt(string maviettat);
        DataTuVetour TachvetourBySerial(string serial, string chinhanh);

        string newId();

        List<Tachve> listHoadontach(string tungay, string denngay,string chinhanh);
    }
}
