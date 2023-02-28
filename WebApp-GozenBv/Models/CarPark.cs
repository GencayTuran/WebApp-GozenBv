using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp_GozenBv.Models
{
    public class CarPark
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Alleen Nummers en Letters ingeven!" )]
        public string LicencePlate { get; set; }
        [MinLength(17)]
        [MaxLength(17)]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Alleen Nummers en Letters toegestaan!")]
        public string ChassisNumber { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public double Km { get; set; }
        public string DriverName { get; set; }
        [DataType(DataType.Date)]
        [Required]
        public DateTime KeuringDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime DeadlineKeuringDate { get; set; }
    }
}
