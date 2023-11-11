using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.ViewModels
{
    public class LogItemsCreatedEditViewModel
    {
        [Required]
        public List<MaterialLogItem> Items { get; set; }
    }
}
