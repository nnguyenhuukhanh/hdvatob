using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hdvatob.Data.Model
{
    public class Nguonhd
    {
        [Key]
        public string IdNguonhd { get; set; }
        public string Diengiai { get; set; }
        public bool active { get; set; }
    }
}
