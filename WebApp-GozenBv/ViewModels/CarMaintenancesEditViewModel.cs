using System.Collections.Generic;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.ViewModels
{
    public class CarMaintenancesEditViewModel
    {
        public CarPark Car { get; set; }
        public List<CarMaintenance> CarMaintenances { get; set; }
    }
}
