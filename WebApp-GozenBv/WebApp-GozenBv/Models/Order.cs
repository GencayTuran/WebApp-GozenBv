using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp_GozenBv.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string ProductId { get; set; }
        public Stock Stock { get; set; }
    }
}
