using Microsoft.Graph;
using System.Collections.Generic;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.ViewModels
{
    public class StockAlertViewModel
    {
        public int Status { get; set; }
        public Stock Stock { get; set; }
    }
}
