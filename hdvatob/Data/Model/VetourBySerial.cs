using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hdvatob.Data.Model
{
    public class VetourBySerial
    {
        [Key]
        public decimal id { get; set; }
        public string serial { get; set; }
        public string tuyentq { get; set; }
        public string sgtcode { get; set; }
        public string tenkhach { get; set; }
        public decimal sotiennt { get; set; }
        public decimal doanhthunn { get; set; }
        public string loaitien { get; set; }
        public decimal tygia { get; set; }
        public string ghichu { get; set; }
        public string diengiai { get; set; }
        public string xuatve { get; set; }
        public int sk { get; set; }
    }
}
