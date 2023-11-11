using Microsoft.Graph;
using System.Collections.Generic;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.ViewModels
{
    public class MaterialLogEditViewModel
    {
        public List<MaterialLogItem> Items { get; set; }
        public MaterialLog MaterialLog { get; set; }
    }
}
