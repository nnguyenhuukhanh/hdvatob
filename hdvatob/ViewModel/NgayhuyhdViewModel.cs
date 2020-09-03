using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hdvatob.ViewModel
{
    public class NgayhuyhdViewModel
    {
        [Key]
        public long stt { get; set; }
        public DateTime ngayhoadon { get; set; }
        public string hdvat { get; set; }
        public string tenkhach { get; set; }
        public string diengiai { get; set; }
        public string sgtcode { get; set; }
        public string serial { get; set; }
        public decimal sotiennt { get; set; }
        public decimal tienvnd { get; set; }
        public decimal ppv { get; set; }
        public decimal tienppv { get; set; }
        public int vat { get; set; }
        public decimal thuevat { get; set; }

    }
}
