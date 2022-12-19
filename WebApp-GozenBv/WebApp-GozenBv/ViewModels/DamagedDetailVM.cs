using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.ViewModels
{
    public class DamagedDetailVM
    {
        public StockLog StockLog { get; set; }
        public StockDamaged StockDamaged { get; set; }
        public List<StockDamagedVM> DamagedItemsVM { get; set; }
        public string EmployeeFullNameFirma { get; set; }
    }
}
