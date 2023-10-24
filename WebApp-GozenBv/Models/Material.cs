﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Models
{
    public class Material
    {
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string ProductCode { get; set; }
        [Required]
        public int QuantityNew { get; set; }
        [Required]
        public int MinQuantity { get; set; }
        public int QuantityUsed { get; set; }
        public bool NoReturn { get; set; }
        public double? Cost { get; set; }
    }
}