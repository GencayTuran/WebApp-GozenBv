using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp_GozenBv.Models
{
    public class MaterialLogItem
    {
        public int Id { get; set; }
        public string LogId { get; set; }
        public int MaterialId { get; set; }
        public int MaterialAmount { get; set; }
        public string ProductNameCode { get; set; }
        public bool NoReturn { get; set; }
        public double? Cost { get; set; }
        public bool Used { get; set; }
        public bool IsDamaged { get; set; }
        public int? DamagedAmount { get; set; }
        public int? RepairedAmount { get; set; }
        public int? DeletedAmount { get; set; }

        public int EditStatus { get; set; }
        public int Version { get; set; }
    }
}
