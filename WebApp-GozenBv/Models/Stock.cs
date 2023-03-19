using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp_GozenBv.Models
{
    public class Stock
    {
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string ProductCode { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int MinQuantity { get; set; }
        public int QuantityUsed { get; set; }
        public bool NoReturn { get; set; }
        public double? Cost { get; set; }
    }
}
