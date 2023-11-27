using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.ViewModels
{
    public class CarCreateViewModel
    {
        public CarPark Car { get; set; }
        public CarMaintenance CarMaintenance { get; set; }
        public string CarMaintenances { get; set; }
    }
}

