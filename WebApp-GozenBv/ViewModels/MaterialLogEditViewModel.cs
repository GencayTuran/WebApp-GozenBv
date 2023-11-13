using Microsoft.Graph;
using System.Collections.Generic;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.ViewModels
{
    public class MaterialLogEditViewModel
    {
        public MaterialLogViewModel MaterialLog { get; set; }
        public List<MaterialLogItemViewModel> Items { get; set; }
    }
}
