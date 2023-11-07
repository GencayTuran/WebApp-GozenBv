using System.ComponentModel.DataAnnotations;
using System;

namespace WebApp_GozenBv.Models
{
    public class LogEditHistory
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
    }
}
