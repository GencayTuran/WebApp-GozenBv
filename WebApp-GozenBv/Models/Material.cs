using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Models
{
    public class Material
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public int NewQty { get; set; }
        [Required]
        public int UsedQty { get; set; }
        [Required]
        public int MinQty { get; set; }
        [Required]
        public int InUseAmount { get; set; }
        [Required]
        public int InDepotAmount { get; set; }
        [Required]
        public int InRepairQty { get; set; }

        public int DeletedQty { get; set; }

        public double? Cost { get; set; }
        public bool NoReturn { get; set; }
    }
}
