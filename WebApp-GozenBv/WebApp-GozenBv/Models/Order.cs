using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp_GozenBv.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderCode { get; set; } //GUID
        public int Amount { get; set; }
        public string ProductId { get; set; }
        public Stock Stock { get; set; }
    }
}
