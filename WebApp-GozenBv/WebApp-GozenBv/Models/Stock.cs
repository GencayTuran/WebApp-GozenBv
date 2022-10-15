using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp_GozenBv.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public bool Used { get; set; }
        public double Cost { get; set; }
    }
}
