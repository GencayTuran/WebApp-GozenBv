using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp_GozenBv.Models
{
    public class WagenPark
    {
        public int Id { get; set; }
        public string LicencePlate { get; set; }
        public string ChassisNumber { get; set; }
        public double Km { get; set; }
        public string FirmaName { get; set; }
        public DateTime KeuringDate { get; set; }
    }
}
