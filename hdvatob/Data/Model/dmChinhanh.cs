using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hdvatob.Data.Model
{
    public class dmChinhanh
    {
        [Key]
        public int id { get; set; }
        public string macn { get; set; }
        public string tencn { get; set; }
        public string diachi { get; set; }
        public string thanhpho { get; set; }
        public string dienthoai { get; set; }
        public string fax { get; set; }
        public string masothue { get; set; }
        public bool trangthai { get; set; }
    }
}
