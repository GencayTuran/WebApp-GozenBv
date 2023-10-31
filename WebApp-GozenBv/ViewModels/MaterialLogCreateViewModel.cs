using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.ViewModels
{
    public class MaterialLogCreateViewModel
    {
        public string SelectedProducts { get; set; }
        public int EmployeeId { get; set; }
        [DataType(DataType.Date)]
        public DateTime MaterialLogDate { get; set; }
    }
}
