using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hdvatob.Data.Model
{
    public class dmhttc
    {
        [Key]
        public string httc { get; set; }
        public string diengiai { get; set; }
        public string ma_in { get; set; }
    }
}
