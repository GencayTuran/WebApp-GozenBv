using System.ComponentModel.DataAnnotations;
using System;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.ViewModels
{
    public class MaterialLogViewModel
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime LogDate { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string EmployeeFullName { get; set; }
        public string LogId { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ReturnDate { get; set; }

        public bool Damaged { get; set; }
        public int Status { get; set; }
        public bool Approved { get; set; }
    }
}
