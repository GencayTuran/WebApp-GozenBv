using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp_GozenBv.Models
{
    public class StockDamaged
    {
        public int Id { get; set; }
        public string LogCode { get; set; }
        public int StockId { get; set; }
        public int StockAmount { get; set; }
        public Stock Stock { get; set; }
    }
}
