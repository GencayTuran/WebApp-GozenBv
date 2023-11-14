using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.ViewModels
{
    public class MaterialLogDetailViewModel
    {
        public MaterialLogViewModel MaterialLog { get; set; }
        public MaterialLogItemViewModel MaterialLogItem { get; set; }
        public List<MaterialLogItemViewModel> Items { get; set; }
        public List<MaterialLogItemViewModel> ItemsDamaged { get; set; }
    }
}
