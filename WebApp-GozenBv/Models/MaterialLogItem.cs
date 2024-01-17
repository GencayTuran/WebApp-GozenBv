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
        public Material Material { get; set; }
        public int MaterialAmount { get; set; }
        public bool Used { get; set; }
        public bool IsDamaged { get; set; }
        public int? DamagedAmount { get; set; }
        public int? RepairAmount { get; set; }
        public int? DeleteAmount { get; set; }
    }
}
