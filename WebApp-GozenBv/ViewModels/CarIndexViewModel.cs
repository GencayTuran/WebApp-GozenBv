using System;
using System.Collections.Generic;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.ViewModels
{
    public class CarIndexViewModel
    {

        public CarPark Car { get; set; }
        public List<CarMaintenance> CarMaintenances { get; set; }
        public CarMaintenance CarMaintenance { get; set; }

    }
}

