using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.ViewModels
{
    public class LogCreatedEditViewModel
    {
        public string LogId { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime LogDate { get; set; }
        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public List<MaterialLogItem> Items { get; set; }
    }
}
