using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.ViewModels
{
    public class StockLogVM
    {
        public StockLogVM()
        {
            SelectedProducts = new List<SelectedProductsVM>();
        }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public string Action { get; set; }
        public int EmployeeId { get; set; }
        public int OrderId { get; set; }

        public List<SelectedProductsVM> SelectedProducts { get; set; }

    }
}

