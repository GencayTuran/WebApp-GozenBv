using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Models
{
    public class MaterialLog
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime LogDate { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string LogId { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ReturnDate { get; set; }

        public bool Damaged { get; set; }
        public int Status { get; set; }
        public bool Approved { get; set; }

        public int Version { get; set; }
    }
}