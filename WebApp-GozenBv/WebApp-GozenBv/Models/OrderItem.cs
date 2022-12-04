using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp_GozenBv.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public int Amount { get; set; }
        public int ProductId { get; set; } 
        public Stock Stock { get; set; }
        public Order Order { get; set; }
    }
}
