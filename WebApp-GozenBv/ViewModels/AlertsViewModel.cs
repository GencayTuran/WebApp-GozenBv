using Microsoft.Graph;
using System.Collections.Generic;

namespace WebApp_GozenBv.ViewModels
{
    public class AlertsViewModel
    {
        public List<CarAlertViewModel> CarAlerts { get; set; }
        public List<StockAlertViewModel> StockAlerts { get; set; }
    }
}
