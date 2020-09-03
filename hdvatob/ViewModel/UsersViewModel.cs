using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hdvatob.ViewModel
{
    public class UsersViewModel
    {
        public string username { get; set; }
        public string hoten { get; set; }
        public string password { get; set; }
        public string accounthddt { get; set; }
        public string passwordhddt { get; set; }
        public string maviettat { get; set; }
        public string chinhanh { get; set; }
        public bool isAdmin { get; set; }
        public string logfile { get; set; }
        public DateTime? ngaytao { get; set; }
        public string nguoitao { get; set; }
    }
}
