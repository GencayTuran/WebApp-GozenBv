using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.ViewModels
{
    public class MaterialLogDetailViewModel
    {
        //public int MaterialLogId { get; set; }
        //public string LogId { get; set; }
        //[DataType(DataType.Date)]
        //public DateTime? ReturnDate { get; set; }
        //[DataType(DataType.Date)]
        //public DateTime MaterialLogDate { get; set; }
        //public int EmployeeId { get; set; }
        //public int Status { get; set; }
        //public bool IsDamaged { get; set; }
        public MaterialLog MaterialLog { get; set; }
        public MaterialLogItem MaterialLogItem { get; set; }
        public List<MaterialLogItem> Items { get; set; }
        public string EmployeeFullName { get; set; }
        public string DamagedMaterial { get; set; }
    }
}
