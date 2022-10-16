using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp_GozenBv.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public bool Used { get; set; }
        public double Cost { get; set; }
        public int ProductBrandId { get; set; }
        public ProductBrand ProductBrand { get; set; }
    }
}
