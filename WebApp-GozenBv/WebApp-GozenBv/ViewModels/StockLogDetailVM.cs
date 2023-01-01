using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.ViewModels
{
    public class StockLogDetailVM
    {
        [DataType(DataType.Date)]
        public DateTime? CompletionDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime StockLogDate { get; set; }
        public string EmployeeFullNameFirma { get; set; }
        public string LogCode { get; set; }
        public List<StockLogItem> StockLogItems { get; set; }
        public StockLog StockLog { get; set; }
        public StockLogItem StockLogItem { get; set; }
        public string DamagedStock { get; set; }

    }
}
