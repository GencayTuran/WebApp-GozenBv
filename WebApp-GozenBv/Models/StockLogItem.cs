using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp_GozenBv.Models
{
    public class StockLogItem
    {
        public int Id { get; set; }
        public string LogCode { get; set; }
        public int StockId { get; set; }
        public int StockAmount { get; set; }
        public string ProductNameCode { get; set; }
        public bool NoReturn { get; set; }
        public double? Cost { get; set; }
        public bool IsDamaged { get; set; }
        public int? DamagedAmount { get; set; }
        public int? RepairedAmount { get; set; }
        public int? DeletedAmount { get; set; }
    }
}
