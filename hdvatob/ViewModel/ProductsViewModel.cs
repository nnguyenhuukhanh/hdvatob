using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hdvatob.ViewModel
{
    public class ProductsViewModel
    {
        [Key]
        public int ProdId { get; set; }
        public string ProdName { get; set; }
        public int ProdUnit { get; set; }
        public int ProdQuantity { get; set; }
        public double ProdPrice { get; set; }
        public double Total { get; set; }
        public double Extra1 { get; set; }
        public double VATRate { get; set; }
        public double VATAmount { get; set; }
        public double Amount { get; set; }

    }
}
