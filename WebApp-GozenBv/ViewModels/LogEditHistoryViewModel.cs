using System.ComponentModel.DataAnnotations;
using System;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.ViewModels
{
    public class LogEditHistoryViewModel
    {
        public int Id { get; set; }
        public string LogId { get; set; }
        public int Version { get; set; }

        [DataType(DataType.Date)]
        public DateTime EditTimestamp { get; set; }

        [DataType(DataType.Date)]
        public DateTime LogDate { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ReturnDate { get; set; }
        public bool Damaged { get; set; }

        public string EmployeeName { get; set; }
    }
}