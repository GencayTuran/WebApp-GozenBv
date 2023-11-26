using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp_GozenBv.Models
{
    public class CarMaintenance
    {
        public int Id { get; set; }

        [Required]
        public int MaintenanceType { get; set; }
        public int CarId { get; set; }
        public int? MaintenanceKm { get; set; }
        [DataType(DataType.Date)]
        public DateTime? MaintenanceDate { get; set; }
        public string MaintenanceInfo { get; set; }
        public bool Done { get; set; }

    }
}
