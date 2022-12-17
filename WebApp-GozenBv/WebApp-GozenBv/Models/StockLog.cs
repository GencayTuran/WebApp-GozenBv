using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Models
{
    public class StockLog
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime StockLogDate { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string LogCode { get; set; }
        
        //Confirm section
        //public bool Confirmed { get; set; }
        [DataType(DataType.Date)]
        public DateTime? CompletionDate { get; set; } //null when not confirmed

    }
}
