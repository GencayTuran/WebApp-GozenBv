using System;
using System.Collections.Generic;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.ViewModels
{
    public class CarDetailsViewModel
    {
        public CarPark Car { get; set; }
        public IEnumerable<CarMaintenance> CarMaintenances { get; set; }
        public CarMaintenance CarMaintenance { get; set; }
    }
}

