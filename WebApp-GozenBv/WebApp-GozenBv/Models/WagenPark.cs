using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp_GozenBv.Models
{
    public class WagenPark
    {
        public int Id { get; set; }
        public string LicencePlate { get; set; }
        public string ChassisNumber { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public double Km { get; set; }
        [DataType(DataType.Date)]
        public DateTime KeuringDate { get; set; }
        public int FirmaId{ get; set; }
        public Firma Firma { get; set; }
    }
}
