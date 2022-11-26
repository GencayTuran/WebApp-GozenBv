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
        public StockLog()
        {
            //OrderItems = new List<Order>();
        }
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public string Action { get; set; } //ophalen / terugbrengen

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public string OrderCode { get; set; } //GUID
        public List<OrderItem> OrderItems { get; set; }
        public OrderItem OrderItem { get; set; }

    }
}
