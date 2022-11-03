using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp_GozenBv.Models
{
    public class StockLog
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public string Action { get; set; } //ophalen / terugbrengen

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
