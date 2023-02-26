using System;

namespace WebApp_GozenBv.Models
{
    public class CarMaintenance
    {
        public int Id { get; set; }

        public int CarId { get; set; } 
        public int? MaintenanceKm { get; set; }
        public DateTime? MaintenanceDate { get; set; }
        public string MaintenanceInfo { get; set; }
        public bool Done { get; set; }

    }
}
