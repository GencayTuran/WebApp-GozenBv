using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.ViewModels
{
    public class StockLogVM : StockLog
    {
        public string SelectedProducts { get; set; } //JSON

    }
}
