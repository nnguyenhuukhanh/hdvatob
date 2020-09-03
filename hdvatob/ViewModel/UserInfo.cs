using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hdvatob.ViewModel
{
    public class UserInfo
    {
        [Key]
        public long id { get; set; }
        public string username { get; set; }
        public string accounthddt { get; set; }
        public string passwordhddt { get; set; }
        public string mausohd { get; set; }
        public string kyhieuhd { get; set; }
        public bool isAdmin { get; set; }
        public string maviettat { get; set; }
        public string chinhanh { get; set; }
    }
}
