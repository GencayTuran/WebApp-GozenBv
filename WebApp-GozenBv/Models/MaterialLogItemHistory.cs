using System.ComponentModel.DataAnnotations;
using System;

namespace WebApp_GozenBv.Models
{
    public class MaterialLogItemHistory
    {
        public int Id { get; set; }
        public int MaterialLogItemId { get; set; }
        public string LogId { get; set; }
        public int Version { get; set; } 

        [DataType(DataType.Date)]
        public DateTime EditTimestamp { get; set; }

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
