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
        public string ProductBrand { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int MinQuantity { get; set; }
        [Required]
        public double Cost { get; set; }
        [Required]
        public bool Used { get; set; }

        private string _productCode;
        public string ProductCode
        {
            get
            {
                _productCode = (ProductName.Substring(0, 3) + "-" + ProductBrand.Substring(0, 3)).ToUpper();
                return _productCode;
            }
        }
    }
}
