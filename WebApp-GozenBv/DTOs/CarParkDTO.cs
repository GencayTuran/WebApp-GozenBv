using System.Collections.Generic;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.DTOs
{
    public class CarParkDTO
    {
        public CarPark Car { get; set; }
        public List<CarMaintenance> CarMaintenances { get; set; }
    }
}
