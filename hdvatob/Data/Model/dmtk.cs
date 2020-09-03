using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hdvatob.Data.Model
{
    public class dmtk
    {
        [Key]
        public decimal id { get; set; }
        public string   tkhoan { get; set; }
        public string tkhoan1 { get; set; }
        public string tkhoan2 { get; set; }
        public string tentk { get; set; }
        public string tkhoancu { get; set; }
        public bool sudung { get; set; }
        public string ghichu { get; set; }
    }
}
