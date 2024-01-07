using System;

namespace WebApp_GozenBv.ViewModels
{
    public class CarMaintenanceCreateViewModel
    {
        public int MaintenanceType { get; set; }
        public DateTime? MaintenanceDate { get; set; }
        public int? MaintenanceKm { get; set; }
        public string MaintenanceInfo { get; set; }
    }
}