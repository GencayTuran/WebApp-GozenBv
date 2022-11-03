using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp_GozenBv.ViewModels
{
    public class StockLogCreationVM
    {
        public int EmployeeId { get; set; }
        public string EmployeeFullNameFirma { get; set; }
        public int ProductId { get; set; }
        public string ProductNameBrand { get; set; }
    }
}
