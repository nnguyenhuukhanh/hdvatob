using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hdvatob.Data.Model
{
    public class ListChinhanh
    {
        [Key]
        public string machinhanh { get; set; }
        public string tenchinhanh { get; set; }
        public string maviettat { get; set; }
    }
}
