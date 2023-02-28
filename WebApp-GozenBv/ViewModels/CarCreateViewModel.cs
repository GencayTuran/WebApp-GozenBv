using System;
using System.ComponentModel.DataAnnotations;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.ViewModels
{
    public class CarCreateViewModel
    {

        //public int Id { get; set; }
        //[Required]
        //public string LicencePlate { get; set; }
        //[Required]
        //public string ChassisNumber { get; set; }
        //[Required]
        //public string Brand { get; set; }
        //[Required]
        //public string Model { get; set; }
        //[Required]
        //public double Km { get; set; }
        //[DataType(DataType.Date)]
        //[Required]
        //public DateTime KeuringDate { get; set; }
        //[DataType(DataType.Date)]
        //public DateTime DeadlineKeuringDate { get; set; }
        //public string DriverName { get; set; }

        //public int? MaintenanceKm { get; set; }
        //public DateTime? MaintenanceDate { get; set; }
        //public string MaintenanceInfo { get; set; }

        public CarPark Car { get; set; }
        public CarMaintenance CarMaintenance { get; set; }
    }
}

