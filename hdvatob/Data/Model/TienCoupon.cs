using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hdvatob.Data.Model
{
    public class TienCoupon
    {
        [Key]
        public decimal Idvetour { get; set; }
        public decimal Coupon { get; set; }
    }
}
