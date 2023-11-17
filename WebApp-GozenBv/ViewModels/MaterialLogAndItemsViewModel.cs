using Microsoft.Graph;
using System.Collections.Generic;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.ViewModels
{
    public class MaterialLogAndItemsViewModel
    {
        public MaterialLogViewModel MaterialLog { get; set; }
        public MaterialLogItemViewModel MaterialLogItem { get; set; }
        public List<MaterialLogItemViewModel> MaterialLogItems { get; set; }
    }
}
